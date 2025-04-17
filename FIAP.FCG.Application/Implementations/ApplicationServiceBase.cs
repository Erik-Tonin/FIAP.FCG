using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.FCG.Application.Implementations
{
    public abstract class ApplicationServiceBase
    {
        public ValidationResult ValidationResult { get; protected set; } = new ValidationResult();

        public void AddValidationError(string error, string description)
        {
            ValidationResult.Errors.Add(new ValidationFailure(error, description));
        }

        protected ValidationResultDTO<T> CustomValidationDataResponse<T>(object response)
        {
            ValidationResultDTO<T> validationResultDTO = new ValidationResultDTO<T>();
            T val2 = (validationResultDTO.Response = (T)Convert.ChangeType(response, typeof(T)));
            if (ValidationResult.Errors.Any())
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Status = 400,
                    Title = "Validation Failed"
                };

                problemDetails.Errors.Add("Messages", ValidationResult.Errors.Select(x => x.ErrorMessage).ToArray());

                validationResultDTO.ValidationProblemDetails = problemDetails;
            }


            return validationResultDTO;
        }

        protected ValidationResultDTO<T> CustomValidationDataResponse<T>(params object[] response)
        {
            ValidationResultDTO<T> validationResultDTO = new ValidationResultDTO<T>();
            T[] array2 = (validationResultDTO.ListResponse = (T[])Convert.ChangeType(response, typeof(T[])));
            if (ValidationResult.Errors.Any())
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Status = 400,
                    Title = "Validation Failed"
                };

                problemDetails.Errors.Add("Messages", ValidationResult.Errors.Select(x => x.ErrorMessage).ToArray());

                validationResultDTO.ValidationProblemDetails = problemDetails;
            }


            return validationResultDTO;
        }

    }
}
