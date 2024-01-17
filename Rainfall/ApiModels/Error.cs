using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Swashbuckle.AspNetCore.Annotations;

namespace Rainfall.ApiModels
{
    [SwaggerSchema(Title = "Error response", Description = "Details of a rainfall reading")]
    public class Error: IResponse
    {
        public string Message { get; set; } = default!;

        public List<ErrorDetail> Detail { get; set; } = new();
    }
}
