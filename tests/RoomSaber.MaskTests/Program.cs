using RoomSaber;

int passed = 0;
void Check(string label, float actual, float expected, float tolerance = 0.0001f)
{
    if (float.IsNaN(actual) || Math.Abs(actual - expected) > tolerance)
        throw new Exception($"{label}: expected {expected} but got {actual}");
    Console.WriteLine($"PASS {label}");
    passed++;
}

void CheckThrows(string label, Action action)
{
    try { action(); }
    catch (ArgumentException) { Console.WriteLine($"PASS {label}"); passed++; return; }
    throw new Exception($"{label}: expected exception");
}

const float w = 2f, h = 1f;
float Mask(PortalMode m, float x, float y = 0f, float feather = 0f, PortalShape s = PortalShape.Ellipse)
    => PortalMask.PassthroughWeight(m, s, x, y, w, h, feather);

Check("Original always opaque", Mask(PortalMode.Original, 500f), 0f);
Check("Full passthrough always reveals room", Mask(PortalMode.FullPassthrough, 500f), 1f);
Check("Virtual center retains game", Mask(PortalMode.VirtualPortal, 0f), 0f);
Check("Virtual exterior shows room", Mask(PortalMode.VirtualPortal, 1.5f), 1f);
Check("Reality center shows room", Mask(PortalMode.RealityPortal, 0f), 1f);
Check("Reality exterior retains game", Mask(PortalMode.RealityPortal, 1.5f), 0f);
Check("Ellipse near center remains inside", Mask(PortalMode.RealityPortal, .5f, .1f), 1f);
Check("Ellipse boundary included", Mask(PortalMode.RealityPortal, 1f), 1f);
Check("Rectangle corner is inside", Mask(PortalMode.RealityPortal, .9f, .4f, 0f, PortalShape.Rectangle), 1f);
Check("Ellipse corner is outside", Mask(PortalMode.RealityPortal, .9f, .4f), 0f);
Check("Circle uses shorter diameter", Mask(PortalMode.RealityPortal, .6f, 0f, 0f, PortalShape.Circle), 0f);
Check("Feather midpoint is half room", Mask(PortalMode.RealityPortal, 1.1f, 0f, .2f), .5f);
Check("Polarity midpoint complements", Mask(PortalMode.VirtualPortal, 1.1f, 0f, .2f), .5f);
CheckThrows("Reject zero width", () => PortalMask.PassthroughWeight(PortalMode.VirtualPortal, PortalShape.Ellipse, 0f, 0f, 0f, 1f));
CheckThrows("Reject negative feather", () => Mask(PortalMode.RealityPortal, 0f, 0f, -.2f));
Console.WriteLine($"All {passed} portal mask tests passed.");
