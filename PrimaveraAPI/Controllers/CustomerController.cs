using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PrimaveraAPI.Class_;
using PrimaveraAPI.DTO;
using PrimaveraAPI.Repository;
using System;
using System.Threading.Tasks;

namespace PrimaveraAPI.Controllers
{
    [EnableCors(Cors.CorsPolicy)]
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var lst = await new CustomerDTO().GetListAsync();
                if (lst == null)
                {
                    return BadRequest(new { message = "No costumer found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("customer/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerDTO>> GetCustomerAsync(int id)
        {
            try
            {
                var sItem = await new CustomerDTO().GetCustomerAsync(id);

                if (sItem == null)
                    return BadRequest(new { message = "Customer with id:" + id + " not found" });

                return Ok(sItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{byName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCustomersByNameAsync(string name)
        {
            try
            {
                var lst = await new CustomerDTO().GetCustomersByNameAsync(name);
                if (lst == null)
                {
                    return BadRequest(new { message = "No customers found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("createCustomer")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CustomerDTO body)
        {
            try
            {
                if (body.id == 0)
                {
                    body.id = await ((CustomerRepository)body.GetRepository()).findNextAvailableIndex();
                }
                if (!(await ((CustomerRepository)body.GetRepository()).PostIntoDB(body /*,userID*/)))
                    return NotFound(new { message = "Invalid customer" });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT: api/Customer/5
        [HttpPut("{id}")]
        //[Authorize()]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] CustomerDTO body)
        {
            if (body == null)
                return NotFound(new { message = "Customer with id:" + id + " not found" });
            try
            {
                body.id = id;
                if (await ((CustomerRepository)body.GetRepository()).EditAsync(body))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

            //return Ok();
        }
        [HttpDelete("deleteCustomer/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sItem = await new CustomerDTO().GetCustomerAsync(id);

                if (sItem == null)
                    return NotFound(new { message = "Customer with id:" + id + " not found" });

                if (await ((CustomerRepository)new CustomerDTO().GetRepository()).DeleteAsync(sItem.GetPrimaryKey()))
                    return Ok();
                else
                    return NotFound(new { message = "Error eliminating Customer" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
