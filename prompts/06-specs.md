# Specs Grid

| ID | Type | Item | Prereq Task | Complete |
|---|---|---|---|---|
| T1 | Task | Initialize solution and projects (`Checkers.sln`, Core/Cli/Tests) | - | [ ] |
| T2 | Task | Add project references (Cli -> Core, Tests -> Core) | T1 | [ ] |
| T3 | Task | Define core domain models (Position, Piece, Move, Board, GameState, enums) | T2 | [ ] |
| T4 | Task | Implement board initialization and piece placement | T3 | [ ] |
| T5 | Task | Implement standard move generation (non-capture) | T4 | [ ] |
| T6 | Task | Implement capture detection and generation (single jump) | T5 | [ ] |
| T7 | Task | Enforce mandatory captures | T6 | [ ] |
| T8 | Task | Implement multi-jump sequences | T7 | [ ] |
| T9 | Task | Implement kinging rules and king moves/captures | T8 | [ ] |
| T10 | Task | Implement win/draw detection | T9 | [ ] |
| T11 | Task | Implement CLI rendering (ASCII board + coordinates) | T10 | [ ] |
| T12 | Task | Implement CLI input parsing/validation | T11 | [ ] |
| T13 | Task | Integrate CLI with core for moves/turns | T12 | [ ] |
| T14 | Task | Create unit test fixtures/helpers | T3 | [ ] |
| T15 | Task | Write unit tests for core rules | T14 | [ ] |
| T16 | Task | Add `build.ps1` (restore/build/test) | T1 | [ ] |
| T17 | Task | Verify build script and tests pass | T16, T15 | [ ] |
| TP1 | Test | Board initializes to standard starting layout | T4 | [ ] |
| TP2 | Test | Standard piece move generation (non-capture) | T5 | [ ] |
| TP3 | Test | Capture move generation (single jump) | T6 | [ ] |
| TP4 | Test | Mandatory capture enforcement | T7 | [ ] |
| TP5 | Test | Multi-jump sequence generation | T8 | [ ] |
| TP6 | Test | Kinging on reaching back rank | T9 | [ ] |
| TP7 | Test | King movement and capture | T9 | [ ] |
| TP8 | Test | Win detection by no pieces | T10 | [ ] |
| TP9 | Test | Win detection by no legal moves | T10 | [ ] |
| TP10 | Test | Draw detection (if implemented) | T10 | [ ] |
| TP11 | Test | CLI renders board with coordinates | T11 | [ ] |
| TP12 | Test | CLI move input parsing | T12 | [ ] |
| TP13 | Test | CLI applies legal move and advances turn | T13 | [ ] |
| TP14 | Test | `build.ps1` restores/builds/tests successfully | T16 | [ ] |
| TP15 | Test | End-to-end build verification | T17 | [ ] |
