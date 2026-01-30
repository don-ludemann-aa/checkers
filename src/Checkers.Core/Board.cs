using System;
using System.Collections.Generic;

namespace Checkers.Core;

public sealed class Board
{
    private readonly Piece?[,] _squares = new Piece?[8, 8];

    public Piece? GetPiece(Position position) => position.IsValid ? _squares[position.Row, position.Col] : null;

    public void SetPiece(Position position, Piece? piece)
    {
        if (!position.IsValid)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        _squares[position.Row, position.Col] = piece;
    }

    public IEnumerable<(Position Position, Piece Piece)> EnumeratePieces(PlayerColor color)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var piece = _squares[row, col];
                if (piece != null && piece.Color == color)
                {
                    yield return (new Position(row, col), piece);
                }
            }
        }
    }

    public int CountPieces(PlayerColor color)
    {
        int count = 0;
        foreach (var _ in EnumeratePieces(color))
        {
            count++;
        }
        return count;
    }

    public static Board CreateStandard()
    {
        var board = new Board();
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if ((row + col) % 2 == 1)
                {
                    board._squares[row, col] = new Piece(PlayerColor.Black);
                }
            }
        }

        for (int row = 5; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if ((row + col) % 2 == 1)
                {
                    board._squares[row, col] = new Piece(PlayerColor.Red);
                }
            }
        }

        return board;
    }

    public static Board CreateEmpty() => new Board();

    public Board Clone()
    {
        var board = new Board();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var piece = _squares[row, col];
                if (piece != null)
                {
                    board._squares[row, col] = new Piece(piece.Color, piece.IsKing);
                }
            }
        }
        return board;
    }
}
