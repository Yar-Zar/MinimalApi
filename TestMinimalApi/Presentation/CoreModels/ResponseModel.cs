using System.Net;

namespace TestMinimalApi.Presentation.CoreModels
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
