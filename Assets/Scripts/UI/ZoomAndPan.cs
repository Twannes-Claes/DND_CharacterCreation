using UnityEngine;

public class ZoomAndPan : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.01f;
    public float minScale = 1f;
    public float maxScale = 4f;
    public float zoomSmoothTime = 0.1f;

    [Header("Pan Settings")]
    public bool allowPan = true;
    public float panSpeed = 0.5f;
    public float panSmoothTime = 0.1f;

    private RectTransform rectTransform;

    private float targetScale;
    private float zoomVelocity;

    private Vector2 targetPosition;
    private Vector2 panVelocity;

    private Vector2 originalSize; // Store the original size of the RectTransform

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (!rectTransform)
            Debug.LogError("ZoomAndPan requires a RectTransform!");

        targetScale = rectTransform.localScale.x;
        targetPosition = rectTransform.anchoredPosition;

        originalSize = rectTransform.rect.size;
    }

    private void Update()
    {
        HandleZoomInput();
        HandlePanInput();

        SmoothZoom();
        SmoothPan();
    }

    private void HandleZoomInput()
    {
        Vector2 zoomCenter = Vector2.zero;
        float zoomDelta = 0f;

        // Touch pinch
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevT0 = t0.position - t0.deltaPosition;
            Vector2 prevT1 = t1.position - t1.deltaPosition;

            float prevDistance = (prevT0 - prevT1).magnitude;
            float currentDistance = (t0.position - t1.position).magnitude;

            zoomDelta = (currentDistance - prevDistance) * zoomSpeed;
            zoomCenter = (t0.position + t1.position) / 2;
        }
        // Mouse scroll
        else if (Input.mouseScrollDelta.y != 0)
        {
            zoomDelta = Input.mouseScrollDelta.y * zoomSpeed * 50f; // scale for sensitivity
            zoomCenter = Input.mousePosition;
        }

        if (zoomDelta != 0f)
            ZoomAt(zoomCenter, zoomDelta);
    }

    private void ZoomAt(Vector2 screenPoint, float delta)
    {
        // Convert screen point to local point relative to parent
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out Vector2 localPoint);

        float oldScale = rectTransform.localScale.x;
        targetScale = Mathf.Clamp(oldScale + delta, minScale, maxScale);

        // Only adjust position if actually zooming
        if (!Mathf.Approximately(oldScale, targetScale))
        {
            Vector2 pivotOffset = (localPoint - rectTransform.anchoredPosition) / oldScale;
            targetPosition += pivotOffset * (oldScale - targetScale);
            ClampPan();
        }
    }

    private void HandlePanInput()
    {
        if (!allowPan) return;

        // Only allow pan if zoomed in
        if (targetScale <= 1f) return;

        Vector2 delta = Vector2.zero;

        // Touch pan
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
                delta = t.deltaPosition * panSpeed;
        }
        // Mouse drag pan
        else if (Input.GetMouseButton(0))
        {
            delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * panSpeed * 10f;
        }

        targetPosition += delta;
        ClampPan();
    }

    private void ClampPan()
    {
        // Calculate half size of scaled rect
        Vector2 halfSize = originalSize * targetScale * 0.5f;
        Vector2 parentSize = ((RectTransform)rectTransform.parent).rect.size * 0.5f;

        // Clamp x
        float clampedX = Mathf.Clamp(targetPosition.x, -halfSize.x + parentSize.x, halfSize.x - parentSize.x);
        float clampedY = Mathf.Clamp(targetPosition.y, -halfSize.y + parentSize.y, halfSize.y - parentSize.y);

        targetPosition = new Vector2(clampedX, clampedY);
    }

    private void SmoothZoom()
    {
        float scale = Mathf.SmoothDamp(rectTransform.localScale.x, targetScale, ref zoomVelocity, zoomSmoothTime);
        rectTransform.localScale = new Vector3(scale, scale, 1f);
    }

    private void SmoothPan()
    {
        rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, targetPosition, ref panVelocity, panSmoothTime);
    }
}
