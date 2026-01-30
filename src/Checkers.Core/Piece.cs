namespace Checkers.Core;

public sealed class Piece
{
    public Piece(PlayerColor color, bool isKing = false)
    {
        Color = color;
        IsKing = isKing;
    }

    public PlayerColor Color { get; }
    public bool IsKing { get; private set; }

    public void MakeKing() => IsKing = true;
}
