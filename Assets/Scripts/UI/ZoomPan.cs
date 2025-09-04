using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomPan : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera zoomCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 5f;
    [SerializeField] private float zoomSmoothTime = 0.1f;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 5f;
    [SerializeField] private float panSmoothTime = 0.1f;


    [SerializeField] private bool panOnlyWhenZoomed = true;

    private float targetZoom;
    private float zoomVelocity;
    private Vector3 targetPosition;
    private Vector3 panVelocity;

    private void Start()
    {
        targetZoom = zoomCamera.orthographicSize;
        zoomCamera.farClipPlane = 999f;

        targetPosition = zoomCamera.transform.position;
    }

    private void Update()
    {
        zoomCamera.orthographicSize = targetZoom;
        targetZoom -= 10f * Time.deltaTime;
        //HandleZoom();
        //HandlePan();
        //SmoothZoom();
        //SmoothPan();
    }

    private void LateUpdate()
    {
        zoomCamera.orthographicSize = targetZoom;
        zoomCamera.farClipPlane = 999f;
        zoomCamera.orthographic = false;
        zoomCamera.orthographic = true;
    }

    private void HandleZoom()
    {
        float zoomDelta = 0f;
        Vector2 zoomCenter = Vector2.zero;

        // Mouse scroll
        if (Input.mouseScrollDelta.y != 0)
        {
            zoomDelta = Input.mouseScrollDelta.y * zoomSpeed * 0.5f;
            zoomCenter = Input.mousePosition;
        }

        // Touch pinch
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevT0 = t0.position - t0.deltaPosition;
            Vector2 prevT1 = t1.position - t1.deltaPosition;

            float prevDistance = (prevT0 - prevT1).magnitude;
            float currentDistance = (t0.position - t1.position).magnitude;

            zoomDelta = (currentDistance - prevDistance) * zoomSpeed * 0.01f;
            zoomCenter = (t0.position + t1.position) / 2;
        }

        if (zoomDelta != 0f)
            ZoomAt(zoomCenter, zoomDelta);
    }

    private void ZoomAt(Vector2 screenPoint, float delta)
    {
        float oldZoom = zoomCamera.orthographicSize;
        targetZoom = Mathf.Clamp(oldZoom - delta, minZoom, maxZoom);

        Vector3 worldPointBefore = zoomCamera.ScreenToWorldPoint(screenPoint);
        zoomCamera.orthographicSize = targetZoom;
        Vector3 worldPointAfter = zoomCamera.ScreenToWorldPoint(screenPoint);

        targetPosition += worldPointBefore - worldPointAfter;
    }

    private void HandlePan()
    {
        if (panOnlyWhenZoomed && zoomCamera.orthographicSize <= minZoom) return;

        // Mouse drag
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0f) * panSpeed * 10;
            targetPosition += delta * Time.deltaTime;
        }

        // Touch drag
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                Vector3 delta = new Vector3(t.deltaPosition.x, t.deltaPosition.y, 0f) * panSpeed * 0.01f;
                targetPosition += delta;
            }
        }
    }

    private void SmoothZoom()
    {
        zoomCamera.orthographicSize = Mathf.SmoothDamp(
            zoomCamera.orthographicSize,
            targetZoom,
            ref zoomVelocity,
            zoomSmoothTime
        );
    }

    private void SmoothPan()
    {
        zoomCamera.transform.position = Vector3.SmoothDamp(
            zoomCamera.transform.position,
            targetPosition,
            ref panVelocity,
            panSmoothTime
        );
    }
}
