using Swashbuckle.AspNetCore.Annotations;

namespace Rainfall.ApiModels
{
    [SwaggerSchema(Title = "Rainfall reading", Description = "Details of a rainfall reading")]
    public class RainfallReading
    {
        public DateTime DateMeasured { get; set; }
        public decimal AmountMeasured { get; set; }
    }
}
