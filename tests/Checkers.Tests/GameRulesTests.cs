using System.Collections.Generic;
using System.Linq;
using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class GameRulesTests
{
    [Fact]
    public void BoardInitializesWithStandardLayout()
    {
        var board = Board.CreateStandard();

        Assert.Equal(12, board.CountPieces(PlayerColor.Red));
        Assert.Equal(12, board.CountPieces(PlayerColor.Black));

        foreach (var (pos, _) in board.EnumeratePieces(PlayerColor.Red))
        {
            Assert.True(pos.IsDarkSquare);
        }

        foreach (var (pos, _) in board.EnumeratePieces(PlayerColor.Black))
        {
            Assert.True(pos.IsDarkSquare);
        }
    }

    [Fact]
    public void StandardPieceGeneratesNonCaptureMoves()
    {
        var board = Board.CreateEmpty();
        var start = new Position(5, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        var targets = moves.Select(m => m.To).ToList();
        Assert.Contains(new Position(4, 1), targets);
        Assert.Contains(new Position(4, 3), targets);
    }

    [Fact]
    public void CaptureMoveGeneratedWhenOpponentAdjacent()
    {
        var board = Board.CreateEmpty();
        var start = new Position(5, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red));
        board.SetPiece(new Position(4, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        Assert.Single(moves);
        Assert.Equal(new Position(3, 4), moves[0].To);
        Assert.True(moves[0].IsCapture);
    }

    [Fact]
    public void MandatoryCaptureExcludesNonCaptures()
    {
        var board = Board.CreateEmpty();
        var capturePiece = new Position(5, 2);
        var normalPiece = new Position(5, 4);
        board.SetPiece(capturePiece, new Piece(PlayerColor.Red));
        board.SetPiece(normalPiece, new Piece(PlayerColor.Red));
        board.SetPiece(new Position(4, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        Assert.All(moves, move => Assert.True(move.IsCapture));
    }

    [Fact]
    public void MultiJumpCaptureSequenceGenerated()
    {
        var board = Board.CreateEmpty();
        var start = new Position(5, 0);
        board.SetPiece(start, new Piece(PlayerColor.Red));
        board.SetPiece(new Position(4, 1), new Piece(PlayerColor.Black));
        board.SetPiece(new Position(2, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);
        var move = moves.Single();

        Assert.Equal(new Position(1, 4), move.To);
        Assert.Equal(2, move.Captured.Count);
        Assert.Equal(3, move.Path.Count);
    }

    [Fact]
    public void PieceBecomesKingOnBackRow()
    {
        var board = Board.CreateEmpty();
        var start = new Position(1, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red));

        var state = new GameState(board, PlayerColor.Red);
        var moves = state.GetLegalMoves();
        var move = moves.Single(m => m.To == new Position(0, 1));

        state.ApplyMove(move);

        var piece = board.GetPiece(new Position(0, 1));
        Assert.NotNull(piece);
        Assert.True(piece!.IsKing);
    }

    [Fact]
    public void KingMovesBackward()
    {
        var board = Board.CreateEmpty();
        var start = new Position(3, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red, isKing: true));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);
        var targets = moves.Select(m => m.To).ToList();

        Assert.Contains(new Position(2, 1), targets);
        Assert.Contains(new Position(2, 3), targets);
        Assert.Contains(new Position(4, 1), targets);
        Assert.Contains(new Position(4, 3), targets);
    }

    [Fact]
    public void WinDetectedWhenOpponentHasNoPieces()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        var state = new GameState(board, PlayerColor.Black);

        state.UpdateStatus();

        Assert.Equal(GameStatus.RedWins, state.Status);
    }

    [Fact]
    public void WinDetectedWhenOpponentHasNoLegalMoves()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(0, 1), new Piece(PlayerColor.Red));
        board.SetPiece(new Position(7, 0), new Piece(PlayerColor.Black));

        var state = new GameState(board, PlayerColor.Black);
        state.UpdateStatus();

        Assert.Equal(GameStatus.RedWins, state.Status);
    }
}
