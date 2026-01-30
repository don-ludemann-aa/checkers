# Design

## Overview
Build a Windows 11 terminal checkers game in a C# solution. The solution includes a console app for gameplay, a class library for core game logic, and a test project for unit tests. A PowerShell build script builds the solution and runs all tests.

## Solution structure
- `Checkers.sln`
- `src/Checkers.Core` (class library)
- `src/Checkers.Cli` (console app)
- `tests/Checkers.Tests` (unit tests)
- `build.ps1` (build + test)

## Core game model
- `Board` (8x8, tracks squares, pieces)
- `Piece` (color: Red/Black, isKing)
- `Position` (row/col, 0-based)
- `Move` (from, to, captured positions)
- `GameState` (current player, board, status: InProgress/Win/Draw)

## Rules and validation
- Standard American checkers rules for movement/captures.
- Diagonal moves on dark squares only.
- Single-step move if no capture available; captures are mandatory when available.
- Multiple jumps in a single turn when available.
- Kinging on reaching opposite end.
- Win when opponent has no pieces or no legal moves.

## CLI flow
- Render board with coordinates and simple ASCII symbols.
- Prompt current player for move input (e.g., `b6-a5` or `b6-d4` for jumps).
- Validate moves using `Checkers.Core`.
- Show errors for invalid input or illegal moves; reprompt.
- Support quitting with `q`.

## Testing approach
- Unit tests for:
  - Move generation (normal + capture)
  - Mandatory capture enforcement
  - Multi-jump sequences
  - Kinging
  - Win detection
- Use xUnit (or MSTest/NUnit if preferred) with deterministic scenarios.

## Build script
- `build.ps1` runs `dotnet restore`, `dotnet build`, and `dotnet test`.
- Exit non-zero on failure.

## Non-goals
- AI opponent or networking.
- GUI front end.
