# roomSaber

**Experimental mixed-reality portal mod for Beat Saber on Steam Frame.**

> **Status: research + scaffold (0.0.1). No working passthrough mod is released yet.**

roomSaber aims to let you play the **on-device ARM64 Beat Saber port** with your room and the Beat Saber world connected through a configurable portal.

## Intended modes

| Mode | Inside the portal | Outside the portal |
| --- | --- | --- |
| **Virtual Portal** | Beat Saber environment | Arcturus room passthrough |
| **Reality Portal** | Arcturus room passthrough | Beat Saber environment |
| **Full Passthrough** | Room passthrough | Room passthrough |
| **Original** | Unmodified Beat Saber | Unmodified Beat Saber |

The oval is initially fixed in playspace in front of the player (not glued to the headset). Notes, bombs, sabers, obstacles, HUD and scoring should stay visible and behave normally across its boundary. Mode, width, height, distance, vertical offset, edge softness and a possible portal border are planned settings. Other shapes can follow after the oval works.

## Exact target

- **Hardware:** Valve Steam Frame, with Arcturus Vision camera.
- **Game:** [DaVarga/bs-arm64](https://github.com/DaVarga/bs-arm64) installation of owned Beat Saber, initially **1.44.1** (Unity 6000.0.40f1).
- **Execution:** Windows **ARM64** game and Mono code under **ARM64 Proton** on the headset. This is *not* a native Linux executable, PCVR streaming mod, or Quest APK.
- **Mod loader:** BSIPA 4.3.7 / Harmony on bs-arm64, subject to verification on the user's install.
- **Passthrough:** headset-side OpenXR alpha blending, if the Proton / Unity / SteamVR path can preserve and submit it. **Not established yet.**

## What is and is not solved

**Known upstream:** bs-arm64 runs Beat Saber with BSIPA/Harmony on Steam Frame. Arcturus documents compositor alpha blending for local applications, and AutoBS has useful portal concepts.

**Unproven for roomSaber:** enabling OpenXR alpha blend through the Windows ARM64 UnityOpenXR → Wine OpenXR bridge; preserving swapchain alpha through Unity, D3D11/DXVK, and the compositor; compositing a selective oval *without* masking the game objects; runtime comfort/performance.

**Do not confuse AutoBS with this rendering route:** AutoBS's mixed reality portals use green chroma-key geometry for ALVR / Virtual Desktop. roomSaber wants on-device compositor alpha, not chroma key and not direct camera frames.

## First deliverables

1. Instrument the OpenXR chain, enumerate supported environment blend modes, and identify whether the swapchain format preserves alpha.
2. On the device, make a **single opaque cube over real-room passthrough**. If Proton cannot do this, isolate the narrowest necessary bridge patch.
3. Implement a conservative scene/render classification that can hide scenery **without hiding notes or HUD**.
4. Introduce an elliptical aperture mask with **both polarity modes**. Check per-eye stereo alignment and clipping when gameplay objects cross the edge.
5. Add gameplay toggles, fail-safe normal mode, configuration and a reproducible install/uninstall workflow.

See [architecture](docs/ARCHITECTURE.md), [roadmap](docs/ROADMAP.md) and [testing](docs/TESTING.md).

## Repository layout

- `src/RoomSaber/`: minimal BSIPA load/diagnostic scaffold (no rendering hooks yet).
- `docs/`: technical constraints, milestones and Frame test protocol.

## Development

The plugin scaffold intentionally requires the user's own game installation and local SDK/build tools. **No Beat Saber assets, Unity player, Proton binaries or camera footage are included.**

The first significant implementation is an OpenXR feasibility probe, not a promised working gameplay build. Please report the game version, bs-arm64 version, Proton version, SteamVR runtime and camera configuration when testing.

## Acknowledgments and licensing

- [DaVarga/bs-arm64](https://github.com/DaVarga/bs-arm64), native ARM64 Beat Saber execution and mod-loader support.
- [procedure1/AutoBS](https://github.com/procedure1/AutoBS), mixed-reality portal design reference (MIT-licensed upstream). No AutoBS source has been copied into this initial scaffold.
- [Arcturus Vision developer resources](https://arcturus.vision/developers), local OpenXR alpha blending examples.

Unofficial community project. Not affiliated with Beat Games, Valve, Arcturus Vision or the upstream mod authors. Game ownership required.
