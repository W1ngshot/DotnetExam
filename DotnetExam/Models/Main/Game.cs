using DotnetExam.Models.Enums;
using DotnetExam.Models.Main.Abstractions;

namespace DotnetExam.Models.Main;

public class Game : BaseEntity
{
    public required Player Host { get; set; }
    public Guid? OpponentId { get; set; }
    public Player? Opponent { get; set; }
    public required GameState State { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required Board Board { get; set; }
    
    public void Move(int x, int y, Mark mark)
    {
        if (State is not GameState.Started)
        {
            throw new InvalidOperationException();
        }

        if (!Board.IsInBound(x, y))
        {
            throw new InvalidOperationException();
        }

        if (!IsPlayerTurn(mark))
        {
            throw new InvalidOperationException();
        }

        if (Board.GetMark(x, y) is not null)
        {
            throw new InvalidOperationException();
        }

        Board.SetMark(x, y, mark);
        UpdateState();
    }

    public Mark NextTurn()
    {
        var crossesCount = Board.Count(cell => cell.Mark is Mark.Cross);
        var noughtsCount = Board.Count(cell => cell.Mark is Mark.Nought);

        return crossesCount == noughtsCount ? Mark.Cross : Mark.Nought;
    }
    
    private void UpdateState()
    {
        if (CheckWinner(out var mark))
        {
            State = mark is Mark.Cross ? GameState.CrossesWon : GameState.NoughtsWon;
            return;
        }
        
        if (Board.IsFilled())
        {
            State = GameState.Draw;
            return;
        }

        State = GameState.Started;
    }

    public bool IsPlayerTurn(Mark mark)
    {
        return NextTurn() == mark;
    }

    private bool CheckWinner(out Mark mark)
    {
        var lines = Board.GetLines();

        foreach (var line in lines)
        {
            var t = line[0].Mark;

            if (!t.HasValue || line.Any(c => c.Mark != t))
            {
                continue;
            }

            mark = t.Value;
            return true;
        }

        mark = default;
        return false;
    }
}