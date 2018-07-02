using System;
using System.Collections.Generic;
using Logic.Entities;
using Logic.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepository;
        private readonly CustomerService _customerService;

        public CustomerController(
            CustomerRepository customerRepository,
            CustomerService customerService)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
        }

        [HttpGet]
        public JsonResult GetList()
        {
            IReadOnlyList<Customer> customers = _customerRepository.GetList();
            return Json(customers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Json(customer);
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (_customerRepository.GetByEmail(customer.Email) != null)
                {
                    return BadRequest("Email is already in use: " + customer.Email);
                }

                customer.Status = CustomerStatus.Regular;
                _customerRepository.Add(customer);
                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new {error = e.Message});
            }
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] Customer item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Customer customer = _customerRepository.GetById(id);
                if (customer == null)
                {
                    return BadRequest("Invalid customer id: " + id);
                }

                customer.Name = item.Name;
                _customerRepository.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, new {error = e.Message});
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
                {
                    return BadRequest("Invalid customer id: " + id);
                }

                if (customer.Status == CustomerStatus.Vip)
                {
                    return BadRequest("The customer already has the Vip status");
                }

                bool success = _customerService.PromoteCustomer(customer);
                if (!success)
                {
                    return BadRequest("Cannot promote the customer");
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
