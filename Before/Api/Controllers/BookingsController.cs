using System;
using System.Collections.Generic;
using Logic.Entities;
using Logic.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class BookingsController : Controller
    {
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerService _customerService;
        private readonly BookingRepository _bookingRepository;
        private readonly BookingsService _bookingsService;

        public BookingsController(
            CustomerRepository customerRepository,
            CustomerService customerService,
            BookingRepository bookingRepository,
            BookingsService bookingsService)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
            _bookingRepository = bookingRepository;
            _bookingsService = bookingsService;
        }

        [HttpGet]
        public JsonResult GetList()
        {
            IReadOnlyList<Booking> customers = _bookingRepository.GetList();
            return Json(customers);
        }

        [HttpGet("{customerId}")]
        public IActionResult Get(int id)
        {
            Booking booking = _bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Json(booking);
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody]long customerId, [FromBody] Booking booking)
        {
            try
            {
                Customer customer = _customerRepository.GetById(customerId);
                if (customer == null)
                {
                    return BadRequest("Invalid customer customerId: " + customerId);
                }

                if (!_bookingsService.CheckBookingDates(booking))
                {
                    return BadRequest("Invalid booking dates");
                }

                if (!_bookingsService.CheckRoomsAvailability(booking))
                {
                    return BadRequest("Unavailable rooms");
                }

                booking.ExpirationDate = BookingExpirationType.Short;
                booking.Price = _bookingsService.CalculatePrice(booking);
                if (!_customerService.AddBooking(customer, booking))
                {
                    return BadRequest("Unable to add new booking");
                }

                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{id}/bookings")]
        public IActionResult ConfirmBooking(long id, [FromBody] long bookingId)
        {
            try
            {
                Booking booking = _bookingRepository.GetById(bookingId);
                if (booking == null)
                {
                    return BadRequest("Invalid booking id: " + bookingId);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return BadRequest("Invalid customer id: " + id);
                }

                if (!_bookingsService.ConfirmBooking(customer, booking))
                {
                    return BadRequest("Unable to confirm booking");
                }

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

