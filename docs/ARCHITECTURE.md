# Technical architecture (research, 0.0.1)

## The runtime boundary matters

This project targets **on-device Beat Saber Windows ARM64 under ARM64 Proton**, as implemented by [bs-arm64](https://github.com/DaVarga/bs-arm64). It is not a native Linux executable, and is not PC streaming.

Likely frame path, based on bs-arm64's documented architecture:

```text
Beat Saber / Unity 6000.0.40f1 (Windows ARM64)
  ├── BSIPA + managed roomSaber plugin
  ├── Unity camera / rendered objects / D3D11 render targets
  └── UnityOpenXR.dll (ARM64)
         -> OpenXR loader (ARM64)
         -> Proton wineopenxr_a64.dll / wineopenxr.so
         -> SteamVR OpenXR runtime on Steam Frame
         -> Arcturus composited camera passthrough
```

**Critical unknown:** Whether this entire chain can negotiate `XR_ENVIRONMENT_BLEND_MODE_ALPHA_BLEND`, preserve target alpha, and submit the requested blend mode to `xrEndFrame`. A BSIPA scene plugin alone cannot guarantee any of these.

The camera texture does **not** need to be exposed to roomSaber. Arcturus currently documents local compositor blending, not a direct camera-frame API.

## Reference sources

- bs-arm64 architecture: https://github.com/DaVarga/bs-arm64/blob/master/docs/ARCHITECTURE.md
- Arcturus blending: https://arcturus.vision/developers
- AutoBS geometry/chroma-key reference: https://github.com/procedure1/AutoBS/blob/bs-1.42/AutoBS/Modules/EnvironmentMarkersAndGreenScreen.cs

AutoBS uses solid green geometry for ALVR / Virtual Desktop chroma-key passthrough. **Do not copy that strategy unchanged:** OpenXR alpha compositing needs real transparency and the appropriate compositor mode. AutoBS is MIT-licensed; if code is adapted, preserve upstream attribution and the full license notice.

## Rendering contract

Treat **environment** independently from **gameplay objects**:

| Layer | Virtual Portal | Reality Portal |
| --- | --- | --- |
| Environment inside oval | opaque game scene | passthrough |
| Environment outside oval | passthrough | opaque game scene |
| Notes, walls, bombs, sabers, HUD | rendered normally everywhere | rendered normally everywhere |

The overlay note/saber pass must be on top of passthrough as appropriate, and maintain depth and stereo correctness. Never mask notes along with scenery. The CPU reference in `PortalMask.cs` documents portal polarity and edge feathering but does **not** implement per-eye geometry, occlusion or GPU compositing.

A world-anchored portal plane (not head-locked) will initially be centred in front of the play area. Distance, dimensions, offset, rotation and floor handling need experimentation. Portal masking must be applied per eye in a consistent world-space coordinate system.

## Alpha approach candidates (unproven)

1. **Unity-side**: transparent background, segregated environment renderer passes, shader/compositing pass that writes selective output alpha. Ensure bloom, MSAA resolve, post-effects and UI don't make the whole frame opaque.
2. **OpenXR-side**: inspect `xrEnumerateEnvironmentBlendModes` / swapchain formats and how UnityOpenXR + wineopenxr submit `XrFrameEndInfo.environmentBlendMode`. An isolated test hook may be needed.
3. **Bridge patch only if necessary**: minimize changes to bs-arm64/Proton and upstream a reproducible patch rather than distributing replacement proprietary/game DLLs.

No patch is assumed until runtime evidence establishes the failure point. Prefer simple logging before shader surgery.

## Diagnostics required before graphics work

- Game/Unity, bs-arm64, Proton, SteamVR and Arcturus versions
- Enumerated OpenXR blend modes and actual submitted `environmentBlendMode`
- Swapchain format and alpha availability
- An obvious opaque object over real-room passthrough
- Head motions to check stereo alignment and positional tracking
- Cleanup/reversion on menu, song start/end and mod disable

## Safety and ownership

- Build/test against a user-owned Beat Saber installation.
- Never commit game assemblies, runtime DLLs, proprietary assets or passthrough imagery.
- Avoid modifying SteamVR globally or accessing camera frames.
- Any native bridge hook must be reversible and confined to the target instance.
- Preserve a one-click/one-command recovery path to the untouched Beat Saber launch.
