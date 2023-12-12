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
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class SalesItemController : Controller
    {
        private readonly IConfiguration _configuration;
        public SalesItemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var lst = await new SalesItemDTO().GetListAsync();
                if (lst == null)
                {
                    return BadRequest(new { message = "No sales items found" });
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
        public async Task<ActionResult<SalesItemDTO>> GetSalesItemAsync(int id)
        {
            try
            {
                var sItem = await new SalesItemDTO().GetSalesItemAsync(id);

                if (sItem == null)
                    return BadRequest(new { message = "Sales item with id:" + id + " not found" });

                return Ok(sItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{byName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesItemsByNameAsync(string name)
        {
            try
            {
                var lst = await new SalesItemDTO().GetSalesItemByNameAsync(name);
                if (lst == null)
                {
                    return BadRequest(new { message = "No sales items found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<SalesItemDTO>> GetListWithCategory(string category)
        {
            try
            {
                var sItem = await new SalesItemDTO().GetListWithCategory(category);

                if (sItem == null)
                    return BadRequest(new { message = "Category:" + category + " not found" });

                return Ok(sItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<ActionResult<SalesItemDTO>> GetAvailableList()
        {
            try
            {
                var lst = await new SalesItemDTO().GetListAvailable();
                if (lst == null)
                {
                    return BadRequest(new { message = "No sales items found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("available/category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<SalesItemDTO>> GetAvailableListWithCategory(string category)
        {
            try
            {
                var lst = await new SalesItemDTO().GetListAvailableWithCategory(category);
                if (lst == null)
                {
                    return BadRequest(new { message = "No sales items found" });
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       
        [HttpPost("createSalesItem")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SalesItemDTO body)
        {
            try
            {
                if(body.id == 0)
                {
                    body.id = await ((SalesItemRepository)body.GetRepository()).findNextAvailableIndex();
                }
                //If i want the sales Items to only be created by a specific user type
                //var userID = _userManager.GetUserId(User);
                if (!(await ((SalesItemRepository)body.GetRepository()).PostIntoDB(body /*,userID*/)))
                    return NotFound(new { message = "Invalid sales item" });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT: api/SalesItem/5
        [HttpPut("{id}")]
        //[Authorize()]
        public async Task<IActionResult> PutSalesItem([FromRoute] int id, [FromBody] SalesItemDTO body)
        {
            if (body == null)
                return NotFound(new { message = "SalesItem with id:" + id + " not found" });
            try
            {
                if (await ((SalesItemRepository)body.GetRepository()).EditAsync(body))
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
        // PUT: api/SalesItem/editThumbnail/5
        [HttpPut("editThumbnail/{id}")]
        //[Authorize()]
        public async Task<IActionResult> PutThumbnailSalesItem([FromRoute] int id, [FromBody] SalesItemDTO body)
        {
            if (body == null)
                return NotFound(new { message = "SalesItem with id:" + id + " not found" });
            try
            {
                if (await ((SalesItemRepository)body.GetRepository()).EditThumbnailAsync(body))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        // PUT: api/SalesItem/editAvailable/5
        [HttpPut("editAvailable/{id}")]
        //[Authorize()]
        public async Task<IActionResult> PutPublishSalesItem([FromRoute] int id, [FromBody] SalesItemDTO body)
        {

            if (body == null)
                return NotFound(new { message = "SalesItem with id:" + id + " not found" });
            try
            {
                if (await ((SalesItemRepository)body.GetRepository()).EditAvailableAsync(body))
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
        [HttpDelete("deleteSalesItem/{id}")]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = Roles.ADMINISTRATORS)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sItem = await new SalesItemDTO().GetSalesItemAsync(id);

                if (sItem == null)
                    return NotFound(new { message = "SalesItem with id:" + id + " not found" });

                if (await ((SalesItemRepository)new SalesItemDTO().GetRepository()).DeleteAsync(sItem.GetPrimaryKey()))
                    return Ok();
                else
                    return NotFound(new { message = "Error eliminating salesItem" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
