using Swashbuckle.AspNetCore.Annotations;

namespace Rainfall.ApiModels
{
    [SwaggerSchema(Description = "Details of invalid request property")]
    public class ErrorDetail
    {
        public string PropertyName { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
