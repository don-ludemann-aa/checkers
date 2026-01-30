using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Core;

public sealed class Move
{
    public Move(Position from, Position to, IReadOnlyList<Position> path, IReadOnlyList<Position> captured)
    {
        From = from;
        To = to;
        Path = path;
        Captured = captured;
    }

    public Position From { get; }
    public Position To { get; }
    public IReadOnlyList<Position> Path { get; }
    public IReadOnlyList<Position> Captured { get; }
    public bool IsCapture => Captured.Count > 0;

    public string PathNotation => string.Join("-", Path.Select(p => p.ToNotation()));

    public bool MatchesPath(IReadOnlyList<Position> path)
    {
        if (path.Count != Path.Count)
        {
            return false;
        }

        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] != Path[i])
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString() => PathNotation;
}
