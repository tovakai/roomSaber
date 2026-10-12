using IPA;
using IPALogger = IPA.Logging.Logger;

namespace RoomSaber
{
    /// <summary>
    /// BSIPA entry point. Deliberately diagnostic-only in v0.0.1:
    /// no scene changes, OpenXR calls, camera access or render hooks.
    /// </summary>
    [Plugin(RuntimeOptions.SingleStartInit)]
    public sealed class Plugin
    {
        private IPALogger _log;

        [Init]
        public void Init(IPALogger logger)
        {
            _log = logger;
            _log.Info("roomSaber 0.0.1 loaded (diagnostics-only scaffold).");
        }

        [OnStart]
        public void OnApplicationStart()
        {
            _log.Info("roomSaber: no scene modification or passthrough is enabled.");
            _log.Info("Target: Steam Frame / owned Beat Saber 1.44.1 / bs-arm64 / BSIPA.");
        }
    }
}
