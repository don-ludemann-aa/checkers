# Agents

## Project context
- A Windows 11 terminal checkers game in a C# solution.
- Solution shape: `Checkers.sln`, `src/Checkers.Core`, `src/Checkers.Cli`, `tests/Checkers.Tests`, and `build.ps1`.
- Rules target standard American checkers (captures mandatory, multi-jump, kinging).
- Unit tests cover move generation, captures, kinging, and win detection.

## Prompts
- All project prompts and planning artifacts live in `prompts/`.
- Order of operations: Requirements -> Design -> Tasks -> Test plan -> Specs -> Generate code.

## Notes
- Keep Visual Studio user-specific files out of version control (see `.gitignore`).
