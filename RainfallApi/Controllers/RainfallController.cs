using Microsoft.AspNetCore.Mvc;
using Rainfall.ApiModels;
using Rainfall.Core.FloodMonitoring;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace RainfallApi.Controllers
{
    /// <summary>
    /// Operations relating to rainfall
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : Controller
    {
        private readonly IFloodMonitoringService _floodMonitoringService;

        /// <summary>
        /// Rainfall controller's constructor
        /// </summary>
        /// <param name="floodMonitoringService"></param>
        public RainfallController(IFloodMonitoringService floodMonitoringService)
        {
            _floodMonitoringService = floodMonitoringService;
        }

        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <remarks>Retrieve the latest readings for the specified stationId</remarks>
        /// <param name="stationId">The id of the reading station</param>
        /// <param name="count">The number of readings to return</param>
        /// <response code="200">A list of rainfall readings successfully retrieved</response>
        /// <response code="400">Invalid request</response>
        /// <response code="404">No readings found for the specified stationId</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("id/{stationId}/readings")]
        [Tags("Rainfall")]
        [EndpointDescription("Retrieve the latest readings for the specified stationId")]
        [ProducesResponseType(typeof(RainfallReadingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(Error), 500)]
        [Produces("application/json")]
        public async Task<IActionResult> GetRainfall([NotNull] string stationId, [Range(1, 100)] int count = 10)
        {
            try
            {
                var (statusCode, result) = await _floodMonitoringService.Get(stationId, count);

                return statusCode switch
                {
                    HttpStatusCode.OK => Ok(result),
                    HttpStatusCode.BadRequest => BadRequest(result),
                    HttpStatusCode.NotFound => NotFound(result),
                    _ => InternalServerError(result),
                };
            }
            catch(Exception ex)
            {
                Error error = new() { Message = ex.Message, Detail = new() };
                return InternalServerError(error);
            }
        }

        private static ObjectResult InternalServerError(IResponse response)
        {
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}