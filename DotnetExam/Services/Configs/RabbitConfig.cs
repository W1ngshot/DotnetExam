namespace DotnetExam.Services.Configs;

public class RabbitConfig
{
    public const string Rabbit = "RabbitConfig";
    
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}