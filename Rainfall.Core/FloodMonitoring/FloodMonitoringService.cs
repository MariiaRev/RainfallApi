using Newtonsoft.Json;
using Rainfall.ApiModels;
using Rainfall.Core.FloodMonitoring.Models;
using System.Net;
using static Rainfall.Consts.RainfallConsts;
using static System.Net.HttpStatusCode;

namespace Rainfall.Core.FloodMonitoring
{
    internal class FloodMonitoringService: IFloodMonitoringService
    {
        private readonly HttpClient _httpClient;
        public FloodMonitoringService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("rainfallClient");
        }

        public async Task<(HttpStatusCode, IResponse)> Get(string stationId, int limit)
        {
            string url = $"flood-monitoring/data/readings/?parameter=rainfall&stationReference={stationId}&_limit={limit}";

            var responseMessage = await _httpClient.GetAsync(url);
            var message = responseMessage.RequestMessage?.ToString() ?? "";
                
            if (!responseMessage.IsSuccessStatusCode)
            {
                // handle errors from remote site
                var error = GetError(responseMessage.StatusCode, message);
                return (responseMessage.StatusCode, error);
            }

            if (responseMessage.Content is null)
            {
                // handle badrequest
                var error = GetError(BadRequest, message);
                return (BadRequest, error);
            }

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            FloodMonitoringResponse? response = JsonConvert.DeserializeObject<FloodMonitoringResponse>(responseContent);

            if (response is null)
            {
                // handle our 500
                var error = GetError(InternalServerError, message);
                return (InternalServerError, error);
            }
                
            if (response.Items.Count == 0) 
            {
                // handle not found
                var error = GetError(NotFound, message);
                return (NotFound, error);
            }

            RainfallReadingResponse rainfallReadingResponse = ToRainfallReadingResponse(response);
            return (OK, rainfallReadingResponse);
        }

        private static string GetErrorMessage(HttpStatusCode statusCode) => 
            statusCode switch
            {
                HttpStatusCode.OK => OkMessage,
                HttpStatusCode.NotFound => NotFoundMessage,
                HttpStatusCode.BadRequest => BadRequestMessage,
                HttpStatusCode.InternalServerError => InternalServerErrorMessage,
                _ => ""
            };

        private static Error GetError(HttpStatusCode statusCode, string detailMessage)
        {
            ErrorDetail errorDetail = new() { Message = detailMessage };
            Error error = new()
            {
                Message = GetErrorMessage(statusCode),
                Detail = new() { errorDetail }
            };

            return error;
        }

        private static RainfallReading ToRainfallReading(Reading reading)
        {
            RainfallReading rainfallReading = new()
            {
                DateMeasured = reading.DateTime,
                AmountMeasured = reading.Value
            };

            return rainfallReading;
        }

        private static RainfallReadingResponse ToRainfallReadingResponse(FloodMonitoringResponse floodMonitoringResponse)
        {
            List<RainfallReading> rainfallReadings = 
                floodMonitoringResponse.Items
                .Select(ToRainfallReading)
                .ToList();

            RainfallReadingResponse rainfallReadingResponse = new() { Readings = rainfallReadings };
            return rainfallReadingResponse;
        }
    }
}
