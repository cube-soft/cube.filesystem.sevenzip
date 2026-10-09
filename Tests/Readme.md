# Test organization

Keep general API and format coverage in the existing test classes. Put regression
tests for a specific issue in `Core/Sources/Issues` or `Ice/Sources/Issues`, with a
short descriptive class name and the issue ID in the class summary. Keep the
project's existing namespace so assembly setup and shared fixtures still apply.

- `Core/Sources/Issues/SignatureTest.cs`: CICE-20260908-01, ambiguous signatures,
  DMG trailer boundaries, extension fallback, and LZH extraction.
- `Ice/Sources/Issues/ZoneIdLongPathTest.cs`: CICE-20260908-02, ZoneID on long paths.

Keep small synthetic inputs and expected values in the issue class. Reuse the
existing `Examples` and expected-data paths for shared fixtures; do not duplicate
them merely to mirror the source folders. Generate temporary files through the
existing test fixture helpers.

When moving tests, compare discovered cases and results before and after the
move, allowing only the intended fixture-name change. Preserve test arguments,
assertions, and generated-data content.
