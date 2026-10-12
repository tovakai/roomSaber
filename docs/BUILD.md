# Local development and diagnostic build

**Status:** untested scaffold. It does not yet enable passthrough.

1. On the Steam Frame, use an **owned** Beat Saber 1.44.1 instance configured with DaVarga's bs-arm64 runtime and working BSIPA 4.3.7. Keep a separate backup/copy of the working instance.
2. Install a suitable .NET SDK on your build host. The test harness requires .NET 8; the mod target is .NET Framework 4.8.
3. Point the build at **the modded game instance**, not the Steam installation root. `IPA.Loader.dll` must be in `Beat Saber_Data/Managed`.
4. Build the library:

```bash
dotnet build src/RoomSaber/RoomSaber.csproj -c Release \
  -p:BeatSaberDir="/absolute/path/to/your/Beat Saber instance"
```

5. The output `RoomSaber.dll` is intended for the instance's `Plugins/` folder after local verification. Check the BSIPA log for `roomSaber 0.0.1 loaded`. Do **not** copy test harness binaries.
6. Remove only `Plugins/RoomSaber.dll` to revert this diagnostic plugin. It currently writes no game files or runtime patches.

The path is passed at build time because the proprietary game DLLs are not distributed with this project.

**Independent mask tests** (no Beat Saber needed):

```bash
dotnet run --project tests/RoomSaber.MaskTests/RoomSaber.MaskTests.csproj
```

No CI is configured for the plugin; a public runner cannot legally assume access to an owned game installation. Builds on the Frame itself may require extra SDK packages. The reference assembly NuGet dependency is build-time only.

If the plugin fails to build or load, capture the exact compiler/BSIPA log and system versions. Do not modify wineopenxr/UnityOpenXR just to force it.
