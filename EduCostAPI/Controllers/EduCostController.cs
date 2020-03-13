using System;
using Microsoft.AspNetCore.Mvc;

namespace EduCostAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EduCostController : ControllerBase
    {
        /// <summary>
        /// Returns total cost
        /// </summary>
        /// <remarks>Calculates total cost of college including tuition and room and board (optional).</remarks>
        /// <param name="CollegeName">Name of College</param>
        /// <param name="RoomAndBoard">Include room and board fees?</param>
        /// <response code="200">Successful: Total cost in US Dollar.</response>
        /// <response code="400">Error: College name is required. (none given)</response>
        /// <response code="404">Error: College not found.</response>
        [HttpGet]
        public virtual IActionResult Cost(string CollegeName, bool RoomAndBoard = true)
        {
            if (CollegeName == null) return this.BadRequest("College name is required.");
            
            double cost = new EduCost().getCost(CollegeName, RoomAndBoard);

            if (cost == -1) return this.NotFound("College not found.");

            return new ObjectResult(Math.Round(cost,2));
        }
    }
}
