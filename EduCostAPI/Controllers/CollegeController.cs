using System;
using Microsoft.AspNetCore.Mvc;

namespace EduCostAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollegeController : ControllerBase
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
        [Route("/College/Cost/{CollegeName?}/{RoomAndBoard?}")]
        public virtual IActionResult Cost(string CollegeName, bool RoomAndBoard = true)
        {
            if (CollegeName == null || CollegeName.ToLower().Equals("college")) return this.BadRequest("Error: College name is required.");
            
            double cost = new EduCost().getCost(CollegeName, RoomAndBoard);

            if (cost == -1) return this.NotFound("Error: College not found.");

            return new ObjectResult(Math.Round(cost,2));
        }
    }
}