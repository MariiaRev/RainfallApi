using Rainfall.ApiModels;
using System.Net;

namespace Rainfall.Core.FloodMonitoring
{
    public interface IFloodMonitoringService
    {
        Task<(HttpStatusCode, IResponse)> Get(string stationId, int limit);
    }
}
