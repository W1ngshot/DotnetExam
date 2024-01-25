namespace DotnetExam.Models.Responses;

public class SuccessResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public SuccessResponse()
    {
        IsSuccess = true;
        Message = "Success";
    }

    public SuccessResponse(string message)
    {
        IsSuccess = true;
        Message = message;
    }
}