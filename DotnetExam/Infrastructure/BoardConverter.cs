using System.Diagnostics;
using System.Text;
using DotnetExam.Models.Enums;
using DotnetExam.Models.Main;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotnetExam.Infrastructure;

public sealed class BoardConverter()
    : ValueConverter<Board, string>(board => BoardToString(board), str => StringToBoard(str))
{
    private static string BoardToString(Board board)
    {
        var builder = new StringBuilder();

        for (var x = 0; x < board.Size; x++)
        {
            for (var y = 0; y < board.Size; y++)
            {
                var mark = board.GetMark(y, x);
                
                var character = mark switch
                {
                    Mark.Cross => "X",
                    Mark.Nought => "O",
                    null => "_",
                    _ => throw new UnreachableException()
                };
                
                builder.Append(character);
            }
        }
        
        return builder.ToString();
    }

    private static Board StringToBoard(string str)
    {
        var board = new Board();
        var arr = str.Chunk(board.Size).ToArray();

        for (var x = 0; x < board.Size; x++)
        {
            for (var y = 0; y < board.Size; y++)
            {
                var mark = arr[y][x] switch
                {
                    'X' => Mark.Cross,
                    'O' => Mark.Nought,
                    '_' => null as Mark?,
                    _ => throw new UnreachableException()
                };

                board.SetMark(x, y, mark);
            }
        }

        return board;
    }
}