using System;
using System.Collections.Generic;

namespace Checkers.Core;

public static class MoveGenerator
{
    public static IReadOnlyList<Move> GenerateLegalMoves(Board board, PlayerColor player)
    {
        var captures = new List<Move>();
        foreach (var (position, piece) in board.EnumeratePieces(player))
        {
            captures.AddRange(GenerateCaptures(board, position, piece));
        }

        if (captures.Count > 0)
        {
            return captures;
        }

        var moves = new List<Move>();
        foreach (var (position, piece) in board.EnumeratePieces(player))
        {
            moves.AddRange(GenerateSimpleMoves(board, position, piece));
        }

        return moves;
    }

    private static IEnumerable<Move> GenerateSimpleMoves(Board board, Position position, Piece piece)
    {
        foreach (var (rowDelta, colDelta) in GetMoveDirections(piece))
        {
            var target = new Position(position.Row + rowDelta, position.Col + colDelta);
            if (!target.IsValid || !target.IsDarkSquare)
            {
                continue;
            }

            if (board.GetPiece(target) == null)
            {
                yield return new Move(position, target, new[] { position, target }, Array.Empty<Position>());
            }
        }
    }

    private static IEnumerable<Move> GenerateCaptures(Board board, Position position, Piece piece)
    {
        var results = new List<Move>();
        var path = new List<Position> { position };
        var captured = new List<Position>();
        GenerateCapturesRecursive(board, position, piece, path, captured, results);
        return results;
    }

    private static void GenerateCapturesRecursive(
        Board board,
        Position position,
        Piece piece,
        List<Position> path,
        List<Position> captured,
        List<Move> results)
    {
        bool foundCapture = false;
        foreach (var (rowDelta, colDelta) in GetCaptureDirections(piece))
        {
            var mid = new Position(position.Row + rowDelta, position.Col + colDelta);
            var landing = new Position(position.Row + rowDelta * 2, position.Col + colDelta * 2);

            if (!mid.IsValid || !landing.IsValid || !landing.IsDarkSquare)
            {
                continue;
            }

            var midPiece = board.GetPiece(mid);
            if (midPiece == null || midPiece.Color == piece.Color)
            {
                continue;
            }

            if (board.GetPiece(landing) != null)
            {
                continue;
            }

            foundCapture = true;
            var nextBoard = board.Clone();
            nextBoard.SetPiece(position, null);
            nextBoard.SetPiece(mid, null);
            nextBoard.SetPiece(landing, new Piece(piece.Color, piece.IsKing));

            path.Add(landing);
            captured.Add(mid);

            bool reachedBackRow = !piece.IsKing && IsBackRow(piece.Color, landing.Row);
            if (reachedBackRow)
            {
                results.Add(new Move(path[0], landing, new List<Position>(path), new List<Position>(captured)));
            }
            else
            {
                GenerateCapturesRecursive(nextBoard, landing, piece, path, captured, results);
            }

            path.RemoveAt(path.Count - 1);
            captured.RemoveAt(captured.Count - 1);
        }

        if (!foundCapture && captured.Count > 0)
        {
            var end = path[path.Count - 1];
            results.Add(new Move(path[0], end, new List<Position>(path), new List<Position>(captured)));
        }
    }

    private static IEnumerable<(int RowDelta, int ColDelta)> GetMoveDirections(Piece piece)
    {
        if (piece.IsKing)
        {
            yield return (-1, -1);
            yield return (-1, 1);
            yield return (1, -1);
            yield return (1, 1);
            yield break;
        }

        int rowDelta = piece.Color == PlayerColor.Red ? -1 : 1;
        yield return (rowDelta, -1);
        yield return (rowDelta, 1);
    }

    private static IEnumerable<(int RowDelta, int ColDelta)> GetCaptureDirections(Piece piece)
    {
        if (piece.IsKing)
        {
            yield return (-1, -1);
            yield return (-1, 1);
            yield return (1, -1);
            yield return (1, 1);
            yield break;
        }

        int rowDelta = piece.Color == PlayerColor.Red ? -1 : 1;
        yield return (rowDelta, -1);
        yield return (rowDelta, 1);
    }

    private static bool IsBackRow(PlayerColor color, int row)
    {
        return color == PlayerColor.Red ? row == 0 : row == 7;
    }
}
