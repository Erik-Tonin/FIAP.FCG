using Microsoft.AspNetCore.Mvc;

namespace FIAP.FCG.Application.Implementations
{
    public class ValidationResultDTO<T>
    {
        public ValidationProblemDetails? ValidationProblemDetails { get; set; }

        public T? Response { get; set; }

        public T[]? ListResponse { get; set; }
    }
}
