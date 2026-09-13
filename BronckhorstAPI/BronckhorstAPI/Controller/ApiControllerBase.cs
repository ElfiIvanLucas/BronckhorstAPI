using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Controller;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult HandleException(Exception exception) =>
        exception switch
        {
            ArgumentException argumentException =>
                BadRequest(argumentException.Message),
            _ =>
                Problem(statusCode: StatusCodes.Status500InternalServerError)
        };

    protected static void ValidateId(int id, string parameterName)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, id, $"{parameterName} must be greater than zero.");
        }
    }
}
