using System;
using System.Collections.Generic;
using System.Linq;
using Api.Utils;
using CSharpFunctionalExtensions;
using Logic.Bookings;
using Logic.Customers;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;


namespace Api.Customers
{
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {
        private readonly CustomerRepository _customerRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly RoomService _roomService;

        public CustomerController(UnitOfWork unitOfWork, CustomerRepository customerRepository, 
            BookingRepository bookingRepository, RoomService roomService) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _bookingRepository = bookingRepository;
            _roomService = roomService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            var dto = new CustomerDto()
            {
                Id = customer.Id,
                Name = customer.Name.Value,
                Email = customer.Email.Value,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.Type.ToString(),
                CurrentBookings = customer.CurrentBookings,
                PaidBookings = customer.PaidBookings,
            };

            return Json(dto);
        }


        [HttpGet]
        public JsonResult GetList()
        {
            IReadOnlyList<Customer> customers = _customerRepository.GetList();
            IReadOnlyList<CustomerDto> dtos = customers.Select(x => new CustomerDto
            {
                Id = x.Id,
                Name = x.Name.Value,
                Email = x.Email.Value,
                MoneySpent = x.MoneySpent,
                Status = x.Status.Type.ToString(),
                CurrentBookings = x.CurrentBookings,
                PaidBookings = x.PaidBookings,
            }).ToList();
            return Json(dtos);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            try
            {
                Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
                Result<Email> emailOrError = Email.Create(item.Email);

                Result result = Result.Combine(customerNameOrError, emailOrError);
                if (result.IsFailure)
                    return BadRequest(result.Error);

                if (_customerRepository.GetByEmail(emailOrError.Value) != null)
                    return BadRequest("Email is already by another user " + item.Email);

                var customer = new Customer(customerNameOrError.Value, emailOrError.Value);
                _customerRepository.Add(customer);

                return Ok(item);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
        {
            try
            {
                Result<CustomerName> customerNameOrError = CustomerName.Create(item.Name);
                if (customerNameOrError.IsFailure)
                    return BadRequest(customerNameOrError.Error);

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                    return BadRequest("Invalid customer id: " + id);

                customer.Name = customerNameOrError.Value;

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            try
            {
                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                    return BadRequest("Invalid customer id: " + id);

                Result promotionCheck = customer.CanPromote();
                if (promotionCheck.IsFailure)
                    return BadRequest(promotionCheck.Error);

                customer.Promote();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new {error = e.Message});
            }
        }

        [HttpPost]
        [Route("{id}/bookings")]
        public IActionResult CreateBooking(long id, [FromBody] CreateBookingDto item)
        {
            try
            {
                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                    return BadRequest("Invalid customer id: " + id);
                Booking booking = new Booking(customer, item.StartDate, item.EndDate, item.Rooms, item.BookingExpirationType, _roomService);
                customer.AddBooking(booking);
                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{customerId}/bookings/{bookingId}")]
        public IActionResult ConfirmBooking(long customerId, long bookingId)
        {
            try
            {
                Customer customer = _customerRepository.GetById(customerId);
                if (customer == null)
                    return BadRequest("Invalid customer id: " + customerId);

                Booking booking = _bookingRepository.GetById(bookingId);
                if (booking == null)
                    return BadRequest("Invalid booking id: " + customerId);

                customer.ConfirmBooking(booking);
                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}

