using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanZoomController : MonoBehaviour
{
    #region Editor Fields
    [Header("Camera")]
    [SerializeField] private Camera zoomCamera;

    [Header("Content Area (Bounds)")]
    [SerializeField] private RectTransform contentArea;

    [Header("Max ZoomIn")]
    [SerializeField] private float minOrthoSize = 1f;

    [Header("Zoom Speed Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomSmoothTime = 0.1f;

    [Header("Pan Speed Settings")]
    [SerializeField] private float panSpeed = 5f;
    [SerializeField] private float panSmoothTime = 0.1f;
    [SerializeField] private bool panOnlyWhenZoomed = true;
    #endregion

    #region Fields
    private float maxOrthoSize;
    private float targetZoom;
    private float zoomVelocity;

    private Vector3 targetPan;
    private Vector3 panVelocity;
    #endregion

    #region LifeCycle
    private void Start()
    {
        maxOrthoSize = zoomCamera.orthographicSize;

        targetZoom = zoomCamera.orthographicSize;
        targetPan = zoomCamera.transform.position;
    }
    #endregion

    #region GameLoop
    private void Update()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            return;

        HandleZoom();
        HandlePan();

        SmoothZoom();
        SmoothPan();
    }
    #endregion

    #region Functions
    private void HandleZoom()
    {
        if (Gamemanager.Instance.StopScrolling)
            return;
        
        float zoomDelta = 0f;

        #if UNITY_EDITOR
        if (Input.mouseScrollDelta.y != 0f)
        {
            zoomDelta = Input.mouseScrollDelta.y * zoomSpeed;
        }
        #endif

        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevT0 = t0.position - t0.deltaPosition;
            Vector2 prevT1 = t1.position - t1.deltaPosition;

            float prevDist = (prevT0 - prevT1).magnitude;
            float currDist = (t0.position - t1.position).magnitude;

            zoomDelta = (currDist - prevDist) * zoomSpeed * 0.01f;
        }

        if (zoomDelta != 0f)
        {
            ZoomAt(zoomDelta);
        }
    }

    private void ZoomAt(float delta)
    {
        float oldZoom = zoomCamera.orthographicSize;
        float newZoom = Mathf.Clamp(oldZoom - delta, minOrthoSize, maxOrthoSize);
    
        if (Mathf.Approximately(newZoom, oldZoom)) return;
    
        targetZoom = newZoom;
    }

    private void HandlePan()
    {
       if (Gamemanager.Instance.StopScrolling)
           return;

        if (panOnlyWhenZoomed && zoomCamera.orthographicSize >= maxOrthoSize - float.Epsilon) return;

        #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            float dx = -Input.GetAxis("Mouse X");
            float dy = -Input.GetAxis("Mouse Y");

            Vector3 delta = 5 * panSpeed * new Vector3(dx, dy, 0f);

            targetPan += 60f * Time.deltaTime * delta;
        }
        #endif

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                Vector3 delta = new Vector3(-t.deltaPosition.x, -t.deltaPosition.y, 0f) * (panSpeed * 0.02f);
                targetPan += delta;
            }
        }

        ClampPan();
    }

    private void ClampPan()
    {
        if (contentArea == null) return;

        Vector3[] corners = new Vector3[4];
        contentArea.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        float vertExtent = targetZoom;
        float horExtent = vertExtent * zoomCamera.aspect;

        float clampMinX = minX + horExtent;
        float clampMaxX = maxX - horExtent;
        float clampMinY = minY + vertExtent;
        float clampMaxY = maxY - vertExtent;

        if (clampMinX > clampMaxX)
        {
            clampMinX = clampMaxX = (minX + maxX) / 2f;
        }
        if (clampMinY > clampMaxY)
        {
            clampMinY = clampMaxY = (minY + maxY) / 2f;
        }

        targetPan = new Vector3
        (
            Mathf.Clamp(targetPan.x, clampMinX, clampMaxX),
            Mathf.Clamp(targetPan.y, clampMinY, clampMaxY),
            targetPan.z
        );
    }


    private void SmoothZoom()
    {
        zoomCamera.orthographicSize = Mathf.SmoothDamp
        (
            zoomCamera.orthographicSize,
            targetZoom,
            ref zoomVelocity,
            zoomSmoothTime
        );
    }

    private void SmoothPan()
    {
        zoomCamera.transform.position = Vector3.SmoothDamp
        (
            zoomCamera.transform.position,
            targetPan,
            ref panVelocity,
            panSmoothTime
        );
    }
    #endregion
}
