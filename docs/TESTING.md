# Device test protocol

**Baseline first.** Confirm unmodified 1.44.1 with bs-arm64 + BSIPA runs, enters a song, has tracking, and can return to menu. Record the versions of bs-arm64, Proton, SteamVR and Arcturus.

| ID | Milestone | Pass condition |
| --- | --- | --- |
| T00 | Diagnostic-only plugin | Game boots and roomSaber reports load; normal songs still work |
| T01 | Native alpha feasibility | Runtime reports supported blend mode and alpha-capable swapchain |
| T02 | Cube passthrough | One opaque object shows with physical room visible around it |
| T03 | Environment separation | Hide scenery, retain notes, sabers, walls, bombs and HUD |
| T04 | Virtual portal | Beat Saber inside world-fixed oval, room outside |
| T05 | Reality portal | Room inside oval, Beat Saber outside |
| T06 | Edge crossing | Notes stay visible as they cross either portal boundary |
| T07 | Stereo / movement | No double vision, incorrect depth, jitter or eye mismatch |
| T08 | Restore | Original mode works; exiting/restarting leaves no global modifications |
| T09 | Perf | Measure frametime/reprojection at a fixed resolution and refresh rate |

### Report template

```text
Date:
Steam Frame OS:
Beat Saber:
bs-arm64 commit/release:
Proton version:
SteamVR version:
Arcturus firmware/app version:
BSIPA version:
roomSaber commit:
Test ID:
Result: PASS / FAIL / BLOCKED
Logs (redact personal paths):
Steps to reproduce:
Regression versus no-mod baseline:
```

Avoid recording or uploading real-world camera imagery without permission. Focus on runtime logs and repeatable render observations.
