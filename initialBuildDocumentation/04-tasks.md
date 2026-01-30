# Tasks

1. Initialize the C# solution structure (`Checkers.sln`) with projects:
   - `src/Checkers.Core` (class library)
   - `src/Checkers.Cli` (console app)
   - `tests/Checkers.Tests` (test project)

2. Add project references:
   - `Checkers.Cli` references `Checkers.Core`
   - `Checkers.Tests` references `Checkers.Core`

3. Define core domain models in `Checkers.Core`:
   - `Position`, `Piece`, `Move`, `Board`, `GameState`, enums for color/status

4. Implement board initialization and basic piece placement rules.

5. Implement move generation for standard pieces (non-capture moves).

6. Implement capture detection and generation (single jumps).

7. Enforce mandatory captures over non-captures.

8. Implement multi-jump sequences in a single turn.

9. Implement kinging rules and king movement/captures.

10. Implement win/draw detection based on pieces remaining and legal moves.

11. Implement CLI rendering (ASCII board with coordinates).

12. Implement CLI input parsing and validation for moves.

13. Integrate CLI with `Checkers.Core` for move execution and turn progression.

14. Create unit test fixtures for board setup and helper scenarios.

15. Write unit tests for:
    - Move generation (normal + capture)
    - Mandatory capture enforcement
    - Multi-jump sequences
    - Kinging
    - Win detection

16. Add `build.ps1` to run `dotnet restore`, `dotnet build`, and `dotnet test`.

17. Verify build script and tests pass locally.
