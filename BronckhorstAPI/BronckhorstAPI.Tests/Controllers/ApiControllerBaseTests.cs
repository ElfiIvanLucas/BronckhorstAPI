using BronckhorstAPI.Controller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BronckhorstAPI.Tests.Controllers;

public class ApiControllerBaseTests
{
    [Fact]
    public void TestHandleException_UnexpectedException_ReturnsInternalServerError()
    {
        // Arrange
        var controller = new TestApiController();

        // Act
        var result = controller.InvokeHandleException(new Exception());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.IsType<ProblemDetails>(objectResult.Value);
    }

    [Fact]
    public void TestHandleException_ArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var controller = new TestApiController();
        const string expectedMessage = "Invalid argument";

        // Act
        var result = controller.InvokeHandleException(new ArgumentException(expectedMessage));

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(expectedMessage, badRequestResult.Value);
    }

    [Fact]
    public void TestValidateId_PositiveId_DoesNotThrow()
    {
        // Arrange
        var controller = new TestApiController();

        // Act
        var exception = Record.Exception(() => controller.InvokeValidateId(1, "id"));

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(InvalidIds))]
    public void TestValidateId_InvalidId_ThrowsArgumentOutOfRangeException(int id)
    {
        // Arrange
        var controller = new TestApiController();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => controller.InvokeValidateId(id, "id"));
    }

    public static TheoryData<int> InvalidIds =>
    [
        0,
        -1
    ];

    private sealed class TestApiController : ApiControllerBase
    {
        public ActionResult InvokeHandleException(Exception exception) => HandleException(exception);

        public void InvokeValidateId(int id, string parameterName) => ValidateId(id, parameterName);
    }
}