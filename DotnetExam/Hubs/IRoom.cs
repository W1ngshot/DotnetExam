namespace DotnetExam.Hubs;

public interface IRoom
{ 
    Task Move(int x, int y, string mark, string state); 
    Task PlayerDisconnected(string state, string? winnerId);
    Task SendMessage(string message, string username);
    Task SendGameInfo(string hostName, string? opponentName, string state);
}