using System;
using System.Collections.Generic;
using System.Linq;
using Checkers.Core;

var state = new GameState(Board.CreateStandard(), PlayerColor.Red);

while (true)
{
    Console.Clear();
    RenderBoard(state.Board);
    Console.WriteLine();
    Console.WriteLine($"Turn: {state.CurrentPlayer}   Status: {state.Status}");

    if (state.Status != GameStatus.InProgress)
    {
        Console.WriteLine("Game over. Press Enter to exit.");
        Console.ReadLine();
        break;
    }

    Console.Write("Move (e.g., b6-a5 or b6-d4-f2), or q to quit: ");
    var input = Console.ReadLine();
    if (input == null)
    {
        continue;
    }

    input = input.Trim();
    if (input.Equals("q", StringComparison.OrdinalIgnoreCase) ||
        input.Equals("quit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (!TryParsePath(input, out var path, out var error))
    {
        Console.WriteLine(error);
        Pause();
        continue;
    }

    if (!state.TryApplyMove(path, out error))
    {
        Console.WriteLine(error);
        Pause();
    }
}

static void RenderBoard(Board board)
{
    Console.WriteLine("    a b c d e f g h");
    Console.WriteLine("  +-----------------+");
    for (int row = 0; row < 8; row++)
    {
        int rank = 8 - row;
        Console.Write(rank + " | ");
        for (int col = 0; col < 8; col++)
        {
            var pos = new Position(row, col);
            var piece = board.GetPiece(pos);
            char symbol;
            if (!pos.IsDarkSquare)
            {
                symbol = '=';
            }
            else if (piece == null)
            {
                symbol = '.';
            }
            else
            {
                symbol = piece.Color == PlayerColor.Red
                    ? (piece.IsKing ? 'R' : 'r')
                    : (piece.IsKing ? 'B' : 'b');
            }

            Console.Write(symbol);
            Console.Write(' ');
        }
        Console.WriteLine("|");
    }
    Console.WriteLine("  +-----------------+");
    Console.WriteLine("    a b c d e f g h");
    Console.WriteLine("    b=black man  B=black king  r=red man  R=red king  .=empty  ==unplayable");
}

static bool TryParsePath(string input, out List<Position> path, out string error)
{
    path = new List<Position>();
    error = string.Empty;

    var parts = input.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (parts.Length < 2)
    {
        error = "Enter at least a start and end square.";
        return false;
    }

    foreach (var part in parts)
    {
        if (!Position.TryParse(part, out var position))
        {
            error = $"Invalid square: {part}";
            return false;
        }
        path.Add(position);
    }

    return true;
}

static void Pause()
{
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
}
