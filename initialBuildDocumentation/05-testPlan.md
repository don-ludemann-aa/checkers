# Test Plan

## Unit tests (xUnit)

1. Board initializes to standard starting layout
   - Expect 12 red and 12 black pieces on dark squares only.
   - Task that enables: 4

2. Standard piece move generation (non-capture)
   - Given a single piece with open diagonals, expect two forward moves (one for edge cases).
   - Task that enables: 5

3. Capture move generation (single jump)
   - Given opposing piece on diagonal with empty landing, expect capture move returned.
   - Task that enables: 6

4. Mandatory capture enforcement
   - Given at least one capture available, expect non-capture moves to be excluded.
   - Task that enables: 7

5. Multi-jump sequence generation
   - Given a position with chained captures, expect generated move includes multiple captured positions.
   - Task that enables: 8

6. Kinging on reaching back rank
   - Given a piece moves to opponent back row, expect `isKing = true`.
   - Task that enables: 9

7. King movement and capture
   - Given a king, expect backward diagonal moves and captures to be allowed.
   - Task that enables: 9

8. Win detection by no pieces
   - Given opponent has zero pieces, expect status = Win.
   - Task that enables: 10

9. Win detection by no legal moves
   - Given opponent has pieces but no legal moves, expect status = Win.
   - Task that enables: 10

10. Draw detection (if implemented)
   - If draw rules are implemented, verify status = Draw in a defined stalemate scenario.
   - Task that enables: 10

## Integration/CLI checks (non-unit)

11. CLI renders board with coordinates
   - Verify output contains coordinate labels and correct initial layout.
   - Task that enables: 11

12. CLI move input parsing
   - Parse valid input formats like `b6-a5` and reject invalid strings.
   - Task that enables: 12

13. CLI applies a legal move
   - Given a valid input, board updates and player turn advances.
   - Task that enables: 13

## Build script

14. `build.ps1` restores, builds, and tests
   - Running script succeeds and returns exit code 0 on green tests.
   - Task that enables: 16

15. End-to-end build verification
   - Running `build.ps1` after tasks 1-16 completes without errors.
   - Task that enables: 17
