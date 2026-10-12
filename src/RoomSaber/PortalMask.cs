using System;

namespace RoomSaber
{
    /// <summary>
    /// Logical portal modes. This is NOT OpenXR, Unity or camera integration.
    /// </summary>
    public enum PortalMode
    {
        Original,
        VirtualPortal,
        RealityPortal,
        FullPassthrough
    }

    public enum PortalShape
    {
        Ellipse,
        Circle,
        Rectangle
    }

    /// <summary>
    /// CPU reference function for a portal-local 2D mask.
    /// Input X/Y are coordinates on the virtual portal plane in metres,
    /// with (0,0) at its center; NOT screen coordinates.
    /// Result is the desired passthrough fraction for *environment* pixels.
    /// Notes, sabers, HUD and obstacles need an independent opaque path.
    ///
    /// This mathematical prototype does not affect game rendering.
    /// </summary>
    public static class PortalMask
    {
        public static float PassthroughWeight(
            PortalMode mode, PortalShape shape,
            float localX, float localY,
            float widthMeters, float heightMeters,
            float feather = 0f)
        {
            if (mode == PortalMode.Original) return 0f;
            if (mode == PortalMode.FullPassthrough) return 1f;

            if (widthMeters <= 0f) throw new ArgumentOutOfRangeException(nameof(widthMeters));
            if (heightMeters <= 0f) throw new ArgumentOutOfRangeException(nameof(heightMeters));
            if (feather < 0f) throw new ArgumentOutOfRangeException(nameof(feather));
            if (float.IsNaN(localX) || float.IsNaN(localY) ||
                float.IsNaN(widthMeters) || float.IsNaN(heightMeters) ||
                float.IsNaN(feather))
                throw new ArgumentException("Mask coordinates and parameters must be finite.");

            float nx, ny;
            if (shape == PortalShape.Circle)
            {
                float radius = Math.Min(widthMeters, heightMeters) * 0.5f;
                nx = localX / radius;
                ny = localY / radius;
            }
            else
            {
                nx = localX / (widthMeters * 0.5f);
                ny = localY / (heightMeters * 0.5f);
            }

            float distance;
            switch (shape)
            {
                case PortalShape.Ellipse:
                case PortalShape.Circle:
                    distance = (float)Math.Sqrt(nx * nx + ny * ny);
                    break;
                case PortalShape.Rectangle:
                    distance = Math.Max(Math.Abs(nx), Math.Abs(ny));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape));
            }

            float inside = feather == 0f
                ? (distance <= 1f ? 1f : 0f)
                : 1f - SmoothStep(1f, 1f + feather, distance);

            switch (mode)
            {
                case PortalMode.VirtualPortal:
                    return 1f - inside;
                case PortalMode.RealityPortal:
                    return inside;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        private static float SmoothStep(float start, float end, float value)
        {
            float t = Math.Max(0f, Math.Min(1f, (value - start) / (end - start)));
            return t * t * (3f - 2f * t);
        }
    }
}
