using System;
using System.Collections.Generic;
using System.Linq;
using Checkers.Core;

Cli.Run();

static class Cli
{
    private static readonly char[] RowLabels = { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k' };

    private enum SelectionStep
    {
        Column,
        Row,
        Direction,
        Destination,
        Confirm
    }

    public static void Run()
    {
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

            if (!TrySelectMove(state, out var move, out var quit))
            {
                continue;
            }

            if (quit)
            {
                break;
            }

            if (move != null)
            {
                state.ApplyMove(move);
            }
        }
    }

    private static void RenderBoard(Board board)
    {
        Console.WriteLine("      1 2 3 4 5 6 7 8");
        Console.WriteLine("    +-----------------+");
        for (int row = 0; row < 8; row++)
        {
            Console.Write($"  {RowLabels[row]} | ");
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
        Console.WriteLine("    +-----------------+");
        Console.WriteLine("      1 2 3 4 5 6 7 8");
        Console.WriteLine("    b=black man  B=black king  r=red man  R=red king  .=empty  ==unplayable");
    }

    private static bool TrySelectMove(GameState state, out Move? move, out bool quit)
    {
        move = null;
        quit = false;

        int? selectedColumn = null;
        int? selectedRow = null;
        int? selectedDirection = null;

        var selectionHistory = new Stack<SelectionStep>();
        var step = SelectionStep.Column;

        while (true)
        {
            var legalMoves = state.GetLegalMoves();
            if (legalMoves.Count == 0)
            {
                return false;
            }

            switch (step)
            {
                case SelectionStep.Column:
                    {
                        var columns = GetAvailableColumns(legalMoves);
                        if (columns.Count == 1)
                        {
                            selectedColumn = columns[0];
                            step = SelectionStep.Row;
                            break;
                        }

                        Console.Write($"Choose a column: [{string.Join(", ", columns)}] ");
                        var input = ReadInput();
                        if (input == null)
                        {
                            WriteError("Enter a column number.");
                            break;
                        }

                        if (IsQuit(input))
                        {
                            quit = true;
                            return true;
                        }

                        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        if (int.TryParse(input, out var column) && columns.Contains(column))
                        {
                            selectedColumn = column;
                            selectionHistory.Push(SelectionStep.Column);
                            step = SelectionStep.Row;
                            break;
                        }

                        WriteError("Not an available column.");
                        break;
                    }
                case SelectionStep.Row:
                    {
                        if (selectedColumn == null)
                        {
                            step = SelectionStep.Column;
                            break;
                        }

                        var rows = GetAvailableRowsForColumn(legalMoves, selectedColumn.Value);
                        if (rows.Count == 0)
                        {
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, SelectionStep.Column);
                            step = SelectionStep.Column;
                            break;
                        }

                        if (rows.Count == 1)
                        {
                            selectedRow = rows[0];
                            step = SelectionStep.Direction;
                            break;
                        }

                        var rowOptions = rows.Select(r => RowLabels[r]).ToArray();
                        Console.Write($"Choose a row: [{string.Join(", ", rowOptions)}] ");
                        var input = ReadInput();
                        if (input == null)
                        {
                            WriteError("Enter a row letter.");
                            break;
                        }

                        if (IsQuit(input))
                        {
                            quit = true;
                            return true;
                        }

                        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
                        {
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, SelectionStep.Column);
                            step = SelectionStep.Column;
                            break;
                        }

                        var rowLabel = char.ToLowerInvariant(input[0]);
                        var rowIndex = Array.IndexOf(RowLabels, rowLabel);
                        if (rowIndex >= 0 && rows.Contains(rowIndex))
                        {
                            selectedRow = rowIndex;
                            selectionHistory.Push(SelectionStep.Row);
                            step = SelectionStep.Direction;
                            break;
                        }

                        WriteError("Not an available row.");
                        break;
                    }
                case SelectionStep.Direction:
                    {
                        if (selectedColumn == null || selectedRow == null)
                        {
                            step = SelectionStep.Row;
                            break;
                        }

                        var from = new Position(selectedRow.Value, selectedColumn.Value - 1);
                        var directions = GetAvailableDirections(legalMoves, from);
                        if (directions.Count == 0)
                        {
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, SelectionStep.Row);
                            step = SelectionStep.Row;
                            break;
                        }

                        if (directions.Count == 1)
                        {
                            selectedDirection = directions[0];
                            step = SelectionStep.Destination;
                            break;
                        }

                        var directionOptions = directions
                            .Select(direction => FormatDirectionOption(from, GetMovesForDirection(legalMoves, from, direction).First(), direction))
                            .ToArray();
                        Console.Write($"Choose direction: [{string.Join(", ", directionOptions)}] ");
                        var input = ReadInput();
                        if (input == null)
                        {
                            WriteError("Enter a direction number.");
                            break;
                        }

                        if (IsQuit(input))
                        {
                            quit = true;
                            return true;
                        }

                        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
                        {
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, SelectionStep.Row);
                            step = SelectionStep.Row;
                            break;
                        }

                        if (int.TryParse(input, out var direction) && directions.Contains(direction))
                        {
                            selectedDirection = direction;
                            selectionHistory.Push(SelectionStep.Direction);
                            step = SelectionStep.Destination;
                            break;
                        }

                        WriteError("Not an available direction.");
                        break;
                    }
                case SelectionStep.Destination:
                    {
                        if (selectedColumn == null || selectedRow == null || selectedDirection == null)
                        {
                            step = SelectionStep.Direction;
                            break;
                        }

                        var from = new Position(selectedRow.Value, selectedColumn.Value - 1);
                        var moves = GetMovesForDirection(legalMoves, from, selectedDirection.Value);
                        if (moves.Count == 0)
                        {
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, SelectionStep.Direction);
                            step = SelectionStep.Direction;
                            break;
                        }

                        var destinations = GetDestinations(moves);
                        if (destinations.Count == 1)
                        {
                            move = moves.First(m => m.To == destinations[0]);
                            step = SelectionStep.Confirm;
                            break;
                        }

                        var destinationOptions = destinations.Select(ToUi).ToArray();
                        Console.Write($"Choose destination: [{string.Join(", ", destinationOptions)}] ");
                        var input = ReadInput();
                        if (input == null)
                        {
                            WriteError("Enter a destination.");
                            break;
                        }

                        if (IsQuit(input))
                        {
                            quit = true;
                            return true;
                        }

                        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
                        {
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, SelectionStep.Direction);
                            step = SelectionStep.Direction;
                            break;
                        }

                        if (TryParseUiSquare(input, out var destination) && destinations.Contains(destination))
                        {
                            move = moves.First(m => m.To == destination);
                            selectionHistory.Push(SelectionStep.Destination);
                            step = SelectionStep.Confirm;
                            break;
                        }

                        WriteError("Not an available destination.");
                        break;
                    }
                case SelectionStep.Confirm:
                    {
                        if (move == null)
                        {
                            step = SelectionStep.Destination;
                            break;
                        }

                        Console.WriteLine($"Move: {ToUi(move.From)}->{ToUi(move.To)}");
                        Console.Write("Press Enter to confirm, or b to go back: ");
                        var input = ReadInput(allowEmpty: true);
                        if (input == null)
                        {
                            WriteError("Enter to confirm or b to go back.");
                            break;
                        }

                        if (input.Length == 0)
                        {
                            return true;
                        }

                        if (IsQuit(input))
                        {
                            quit = true;
                            return true;
                        }

                        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
                        {
                            var backStep = selectionHistory.Count > 0 ? selectionHistory.Pop() : SelectionStep.Column;
                            ResetSelections(ref selectedColumn, ref selectedRow, ref selectedDirection, ref move, backStep);
                            step = backStep;
                            break;
                        }

                        WriteError("Enter to confirm or b to go back.");
                        break;
                    }
            }
        }
    }

    private static void ResetSelections(ref int? selectedColumn, ref int? selectedRow, ref int? selectedDirection, ref Move? move, SelectionStep step)
    {
        switch (step)
        {
            case SelectionStep.Column:
                selectedColumn = null;
                selectedRow = null;
                selectedDirection = null;
                move = null;
                break;
            case SelectionStep.Row:
                selectedRow = null;
                selectedDirection = null;
                move = null;
                break;
            case SelectionStep.Direction:
                selectedDirection = null;
                move = null;
                break;
            case SelectionStep.Destination:
                move = null;
                break;
        }
    }

    private static List<int> GetAvailableColumns(IReadOnlyList<Move> legalMoves)
    {
        return legalMoves
            .Select(m => m.From.Col + 1)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
    }

    private static List<int> GetAvailableRowsForColumn(IReadOnlyList<Move> legalMoves, int column)
    {
        return legalMoves
            .Where(m => m.From.Col == column - 1)
            .Select(m => m.From.Row)
            .Distinct()
            .OrderBy(r => r)
            .ToList();
    }

    private static List<int> GetAvailableDirections(IReadOnlyList<Move> legalMoves, Position from)
    {
        return legalMoves
            .Where(m => m.From == from)
            .Select(m => GetDirectionCode(from, m.Path[1]))
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }

    private static List<Move> GetMovesForDirection(IReadOnlyList<Move> legalMoves, Position from, int direction)
    {
        return legalMoves
            .Where(m => m.From == from && GetDirectionCode(from, m.Path[1]) == direction)
            .ToList();
    }

    private static List<Position> GetDestinations(IReadOnlyList<Move> moves)
    {
        return moves
            .Select(m => m.To)
            .Distinct()
            .OrderBy(p => p.Col)
            .ThenBy(p => p.Row)
            .ToList();
    }

    private static string ToUi(Position position)
    {
        return $"{position.Col + 1}{RowLabels[position.Row]}";
    }

    private static bool TryParseUiSquare(string input, out Position position)
    {
        position = default;
        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
        {
            return false;
        }

        input = input.Trim().ToLowerInvariant();
        if (!char.IsDigit(input[0]))
        {
            return false;
        }

        if (!int.TryParse(input.Substring(0, 1), out var column) || column < 1 || column > 8)
        {
            return false;
        }

        var rowLabel = input[1];
        var rowIndex = Array.IndexOf(RowLabels, rowLabel);
        if (rowIndex < 0)
        {
            return false;
        }

        position = new Position(rowIndex, column - 1);
        return position.IsValid;
    }

    private static int GetDirectionCode(Position from, Position next)
    {
        int rowDelta = next.Row - from.Row;
        int colDelta = next.Col - from.Col;

        int rowSign = Math.Sign(rowDelta);
        int colSign = Math.Sign(colDelta);

        if (rowSign < 0 && colSign < 0)
        {
            return 1;
        }

        if (rowSign < 0 && colSign > 0)
        {
            return 2;
        }

        if (rowSign > 0 && colSign > 0)
        {
            return 3;
        }

        if (rowSign > 0 && colSign < 0)
        {
            return 4;
        }

        return 0;
    }

    private static string FormatDirectionOption(Position from, Move move, int direction)
    {
        var next = move.Path.Count > 1 ? move.Path[1] : move.To;
        int rowDelta = next.Row - from.Row;
        int colDelta = next.Col - from.Col;
        int rowSign = Math.Sign(rowDelta);
        int colSign = Math.Sign(colDelta);

        var adjacent = new Position(from.Row + rowSign, from.Col + colSign);
        var adjacentLabel = adjacent.IsValid ? ToUi(adjacent) : "off";

        if (!move.IsCapture)
        {
            return $"{direction}: {ToUi(from)}->{adjacentLabel}";
        }

        var landingLabel = next.IsValid ? ToUi(next) : "off";
        return $"{direction}: {ToUi(from)}->{adjacentLabel}->{landingLabel}";
    }

    private static string? ReadInput(bool allowEmpty = false)
    {
        var input = Console.ReadLine();
        if (input == null)
        {
            return null;
        }

        input = input.Trim();
        if (!allowEmpty && input.Length == 0)
        {
            return null;
        }

        return input;
    }

    private static bool IsQuit(string input)
    {
        return input.Equals("q", StringComparison.OrdinalIgnoreCase)
            || input.Equals("quit", StringComparison.OrdinalIgnoreCase);
    }

    private static void WriteError(string message)
    {
        Console.WriteLine(message);
    }
}
