using Swashbuckle.AspNetCore.Annotations;

namespace Rainfall.ApiModels
{
    [SwaggerSchema(Title = "Rainfall reading response", Description = "Details of a rainfall reading")]
    public class RainfallReadingResponse: IResponse
    {
        public List<RainfallReading>? Readings { get; set; }
    }
}
