# Roadmap

## 0.0.1 — Repository bootstrap (scaffold)

- [x] State precise execution target: Windows ARM64 + Proton on Steam Frame
- [x] Define both portal directions and transparency contract
- [x] Minimal BSIPA diagnostic entry point (unverified until device build/load)
- [x] Pure CPU reference mask with independent test harness
- [ ] Build and load plugin on real bs-arm64 install

## 0.0.2 — OpenXR feasibility

- [ ] Confirm blend modes reported by runtime
- [ ] Confirm alpha-capable swapchain format
- [ ] Identify where Proton/Unity chooses submitted blend mode
- [ ] Prove **one opaque cube floating over passthrough** in the target game context
- [ ] Keep original mode working even when camera unavailable

## 0.0.3 — Environment and gameplay isolation

- [ ] Identify Unity scene cameras and effects for 1.44.1
- [ ] Suppress environment without hiding notes/sabers/walls/HUD
- [ ] Verify normals, fog, post-processing, bloom and 120 Hz stability
- [ ] Add clear diagnostics and reversible toggles

## 0.1.0 — Two-way oval portal proof

- [ ] Virtual Portal: game inside oval, room outside
- [ ] Reality Portal: room inside oval, game outside
- [ ] Full passthrough and Original modes
- [ ] World-anchored oval size/distance/height control
- [ ] Objects cross edge without disappearing
- [ ] Per-eye alignment verified, head motion stable
- [ ] Repeatable install/uninstall and test report

## Later, after the underlying renderer works

- Portal border glow and subtle feather options
- Alternative shapes, settings UI, configurable anchors
- Compatibility/performance tests on other supported game versions
- Separate performance profiles for 90/120 Hz

## Non-goals

- No streamed PCVR/ALVR chroma-key backend in initial scope
- No direct camera frame capture
- No repackaged retail game data
- No promise of Quest/Android support
