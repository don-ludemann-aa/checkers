using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Core;

public sealed class GameState
{
    public GameState(Board board, PlayerColor currentPlayer)
    {
        Board = board ?? throw new ArgumentNullException(nameof(board));
        CurrentPlayer = currentPlayer;
        Status = GameStatus.InProgress;
    }

    public Board Board { get; }
    public PlayerColor CurrentPlayer { get; private set; }
    public GameStatus Status { get; private set; }

    public IReadOnlyList<Move> GetLegalMoves() => MoveGenerator.GenerateLegalMoves(Board, CurrentPlayer);

    public bool TryApplyMove(IReadOnlyList<Position> path, out string error)
    {
        error = string.Empty;
        if (Status != GameStatus.InProgress)
        {
            error = "Game is over.";
            return false;
        }

        var legalMoves = GetLegalMoves();
        var move = legalMoves.FirstOrDefault(m => m.MatchesPath(path));
        if (move == null)
        {
            error = "Illegal move.";
            return false;
        }

        ApplyMove(move);
        return true;
    }

    public void ApplyMove(Move move)
    {
        var piece = Board.GetPiece(move.From);
        if (piece == null)
        {
            throw new InvalidOperationException("No piece at move start.");
        }

        Board.SetPiece(move.From, null);
        foreach (var captured in move.Captured)
        {
            Board.SetPiece(captured, null);
        }

        if (!piece.IsKing && IsBackRow(piece.Color, move.To.Row))
        {
            piece.MakeKing();
        }

        Board.SetPiece(move.To, piece);

        CurrentPlayer = CurrentPlayer == PlayerColor.Red ? PlayerColor.Black : PlayerColor.Red;
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        if (Board.CountPieces(PlayerColor.Red) == 0)
        {
            Status = GameStatus.BlackWins;
            return;
        }

        if (Board.CountPieces(PlayerColor.Black) == 0)
        {
            Status = GameStatus.RedWins;
            return;
        }

        var legalMoves = MoveGenerator.GenerateLegalMoves(Board, CurrentPlayer);
        if (legalMoves.Count == 0)
        {
            Status = CurrentPlayer == PlayerColor.Red ? GameStatus.BlackWins : GameStatus.RedWins;
            return;
        }

        Status = GameStatus.InProgress;
    }

    private static bool IsBackRow(PlayerColor color, int row)
    {
        return color == PlayerColor.Red ? row == 0 : row == 7;
    }
}
