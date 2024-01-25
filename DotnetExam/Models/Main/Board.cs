using System.Collections;
using DotnetExam.Models.Enums;

namespace DotnetExam.Models.Main;

public class Board : IEnumerable<Cell>
{
    public readonly int Size = 3;
    private Cell[,] Cells { get; }

    public Board()
    {
        Cells = new Cell[Size, Size];
        for (var x = 0; x < Size; x++)
        {
            for (var y = 0; y < Size; y++)
            {
                Cells[x, y] = new Cell();
            }
        }
    }

    public bool IsFilled() => this.All(c => c.Mark is not null);

    public IEnumerable<Cell[]> GetLines()
    {
        for (var row = 0; row < Size; row++)
        {
            var line = new Cell[Size];
            for (var col = 0; col < Size; col++)
            {
                line[col] = Cells[col, row];
            }
            yield return line;
        }

        for (var col = 0; col < Size; col++)
        {
            var line = new Cell[Size];
            for (var row = 0; row < Size; row++)
            {
                line[row] = Cells[col, row];
            }
            yield return line;
        }

        var diagonal1 = new Cell[Size];
        for (var i = 0; i < Size; i++)
        {
            diagonal1[i] = Cells[i, i];
        }
        yield return diagonal1;

        var diagonal2 = new Cell[Size];
        for (var i = 0; i < Size; i++)
        {
            diagonal2[i] = Cells[i, Size - 1 - i];
        }
        yield return diagonal2;
    }

    public bool IsInBound(int x, int y) => x >= 0 && x < Size && y >= 0 && y < Size;

    public Mark? GetMark(int x, int y) => Cells[x, y].Mark;

    public void SetMark(int x, int y, Mark? mark)
    {
        Cells[x, y].Mark = mark;
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public IEnumerator<Cell> GetEnumerator()
    {
        return Cells.OfType<Cell>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}