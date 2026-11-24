using UnityEngine;

/// <summary>
/// Keeps a fixed "design rectangle" of world space fully visible for an
/// orthographic camera, regardless of the screen's aspect ratio.
///
/// The whole game is built in world space (board, score boxes, reset button,
/// block tray), so instead of anchoring each element to the screen we simply
/// fit the camera to the designed content. Everything then scales together and
/// stays correctly placed on any portrait screen.
///
/// referenceWidth / referenceHeight describe the area (in world units) that must
/// always be visible. The camera grows its orthographic size to fit whichever of
/// the two is the binding constraint for the current aspect (letterboxing the
/// other axis with a margin), so nothing is ever cropped.
/// </summary>
[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class CameraFit : MonoBehaviour
{
    [Tooltip("World-space width of the background to fill the screen with.")]
    [SerializeField] private float referenceWidth = 10.8f;

    [Tooltip("World-space height of the background to fill the screen with.")]
    [SerializeField] private float referenceHeight = 19.2f;

    private Camera cam;
    private int lastWidth;
    private int lastHeight;
    private float lastRefWidth;
    private float lastRefHeight;

    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        Apply();
    }

    private void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight ||
            !Mathf.Approximately(referenceWidth, lastRefWidth) ||
            !Mathf.Approximately(referenceHeight, lastRefHeight))
        {
            Apply();
        }
    }

    private void Apply()
    {
        if (cam == null) cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic) return;

        lastWidth = Screen.width;
        lastHeight = Screen.height;
        lastRefWidth = referenceWidth;
        lastRefHeight = referenceHeight;

        float aspect = (float)Screen.width / Mathf.Max(1, Screen.height);

        float sizeForHeight = referenceHeight * 0.5f;
        float sizeForWidth = (referenceWidth * 0.5f) / Mathf.Max(0.0001f, aspect);

        // Fill (cover): take the smaller size so the background always covers the
        // whole screen with no gaps, cropping the overflowing axis if needed.
        cam.orthographicSize = Mathf.Min(sizeForHeight, sizeForWidth);
    }
}
