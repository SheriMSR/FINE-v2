using AppCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppCore.Models;

public class ValidationError
{
    public ValidationError(string field, string message)
    {
        Field = field != string.Empty ? field : null;
        Message = message;
    }

    public string Field { get; }

    public string Message { get; }
}

public class ValidationResultModel
{
    public ValidationResultModel(ModelStateDictionary modelState)
    {
        Message = StatusCode.UNPROCESSABLE_ENTITY.ToString();
        Data = modelState.Keys
            .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key.ToSnake(), x.ErrorMessage)))
            .ToList();
    }

    public string Message { get; }
    public List<ValidationError> Data { get; }
}

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
    {
        StatusCode = (int)Models.StatusCode.UNPROCESSABLE_ENTITY;
    }
}