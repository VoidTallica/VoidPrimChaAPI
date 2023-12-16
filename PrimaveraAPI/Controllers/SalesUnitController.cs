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
    public class SalesUnitController : Controller
    {
        private readonly IConfiguration _configuration;
        public SalesUnitController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var lst = await new SalesUnitDTO().GetListAsync();
                if (lst == null)
                {
                    return BadRequest(new { message = "No sales unit found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SalesUnitDTO>> GetSalesUnitAsync(int id)
        {
            try
            {
                var sItem = await new SalesUnitDTO().GetSalesUnitAsync(id);

                if (sItem == null)
                    return BadRequest(new { message = "Sales unit with id:" + id + " not found" });

                return Ok(sItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{byName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesUnitsByNameAsync(string name)
        {
            try
            {
                var lst = await new SalesUnitDTO().GetSalesUnitsByNameAsync(name);
                if (lst == null)
                {
                    return BadRequest(new { message = "No sales units found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("createSalesUnit")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SalesUnitDTO body)
        {
            try
            {
                if (body.id == 0)
                {
                    body.id = await ((SalesUnitRepository)body.GetRepository()).findNextAvailableIndex();
                }
                if (!(await ((SalesUnitRepository)body.GetRepository()).PostIntoDB(body /*,userID*/)))
                    return NotFound(new { message = "Invalid sales unit" });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT: api/SalesUnit/5
        [HttpPut("{id}")]
        //[Authorize()]
        public async Task<IActionResult> PutSalesUnit([FromRoute] int id, [FromBody] SalesUnitDTO body)
        {
            if (body == null)
                return NotFound(new { message = "Sales Unit with id:" + id + " not found" });
            try
            {
                body.id = id;
                if (await ((SalesUnitRepository)body.GetRepository()).EditAsync(body))
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
        [HttpDelete("deleteSalesUnit/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sItem = await new SalesUnitDTO().GetSalesUnitAsync(id);

                if (sItem == null)
                    return NotFound(new { message = "Sales Unit with id:" + id + " not found" });

                if (await ((SalesUnitRepository)new SalesUnitDTO().GetRepository()).DeleteAsync(sItem.GetPrimaryKey()))
                    return Ok();
                else
                    return NotFound(new { message = "Error eliminating Sales Unit" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
