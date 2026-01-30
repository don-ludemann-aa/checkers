using System;

namespace Checkers.Core;

public readonly struct Position : IEquatable<Position>
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public bool IsValid => Row >= 0 && Row < 8 && Col >= 0 && Col < 8;
    public bool IsDarkSquare => (Row + Col) % 2 == 1;

    public string ToNotation()
    {
        if (!IsValid)
        {
            return "?";
        }

        char file = (char)('a' + Col);
        int rank = 8 - Row;
        return $"{file}{rank}";
    }

    public static bool TryParse(string text, out Position position)
    {
        position = default;
        if (string.IsNullOrWhiteSpace(text) || text.Length < 2)
        {
            return false;
        }

        char file = char.ToLowerInvariant(text[0]);
        if (file < 'a' || file > 'h')
        {
            return false;
        }

        if (!int.TryParse(text.Substring(1), out int rank))
        {
            return false;
        }

        if (rank < 1 || rank > 8)
        {
            return false;
        }

        int col = file - 'a';
        int row = 8 - rank;
        position = new Position(row, col);
        return position.IsValid;
    }

    public bool Equals(Position other) => Row == other.Row && Col == other.Col;
    public override bool Equals(object? obj) => obj is Position other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Row, Col);

    public static bool operator ==(Position left, Position right) => left.Equals(right);
    public static bool operator !=(Position left, Position right) => !left.Equals(right);
}
