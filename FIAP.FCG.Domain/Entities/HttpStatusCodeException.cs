﻿namespace FIAP.FCG.Domain.Entities
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; }

        public HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
