# Specs Grid

| ID | Type | Item | Prereq Task | Complete |
|---|---|---|---|---|
| T1 | Task | Initialize solution and projects (`Checkers.sln`, Core/Cli/Tests) | - | [x] |
| T2 | Task | Add project references (Cli -> Core, Tests -> Core) | T1 | [x] |
| T3 | Task | Define core domain models (Position, Piece, Move, Board, GameState, enums) | T2 | [x] |
| T4 | Task | Implement board initialization and piece placement | T3 | [x] |
| T5 | Task | Implement standard move generation (non-capture) | T4 | [x] |
| T6 | Task | Implement capture detection and generation (single jump) | T5 | [x] |
| T7 | Task | Enforce mandatory captures | T6 | [x] |
| T8 | Task | Implement multi-jump sequences | T7 | [x] |
| T9 | Task | Implement kinging rules and king moves/captures | T8 | [x] |
| T10 | Task | Implement win/draw detection | T9 | [x] |
| T11 | Task | Implement CLI rendering (ASCII board + coordinates) | T10 | [x] |
| T12 | Task | Implement CLI input parsing/validation | T11 | [x] |
| T13 | Task | Integrate CLI with core for moves/turns | T12 | [x] |
| T14 | Task | Create unit test fixtures/helpers | T3 | [x] |
| T15 | Task | Write unit tests for core rules | T14 | [x] |
| T16 | Task | Add `build.ps1` (restore/build/test) | T1 | [x] |
| T17 | Task | Verify build script and tests pass | T16, T15 | [x] |
| TP1 | Test | Board initializes to standard starting layout | T4 | [x] |
| TP2 | Test | Standard piece move generation (non-capture) | T5 | [x] |
| TP3 | Test | Capture move generation (single jump) | T6 | [x] |
| TP4 | Test | Mandatory capture enforcement | T7 | [x] |
| TP5 | Test | Multi-jump sequence generation | T8 | [x] |
| TP6 | Test | Kinging on reaching back rank | T9 | [x] |
| TP7 | Test | King movement and capture | T9 | [x] |
| TP8 | Test | Win detection by no pieces | T10 | [x] |
| TP9 | Test | Win detection by no legal moves | T10 | [x] |
| TP10 | Test | Draw detection (if implemented) | T10 | [ ] |
| TP11 | Test | CLI renders board with coordinates | T11 | [ ] |
| TP12 | Test | CLI move input parsing | T12 | [ ] |
| TP13 | Test | CLI applies legal move and advances turn | T13 | [ ] |
| TP14 | Test | `build.ps1` restores/builds/tests successfully | T16 | [x] |
| TP15 | Test | End-to-end build verification | T17 | [x] |
