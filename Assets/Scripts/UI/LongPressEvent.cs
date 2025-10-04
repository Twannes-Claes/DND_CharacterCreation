using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class LongPressEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    #region Fields
    private const float _holdTime = 0.5f;

    private bool _isPointerDown = false;
    private float _timer = 0f;

    private bool _canInvoke = false;
    #endregion

    #region Events
    public Action OnLongPress;
    #endregion

    #region GameLoop
    private void Update()
    {
        if (_isPointerDown)
        {
            _timer += Time.deltaTime;

            if (_timer >= _holdTime)
            {
                _canInvoke = true;
            }
        }
    }
    #endregion

    #region Interface
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _timer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_canInvoke)
        {
            OnLongPress?.Invoke();
        }

        Reset();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Reset();
    }
    #endregion

    #region Functions
    private void Reset()
    {
        _canInvoke = false;
        _isPointerDown = false;
        _timer = 0f;
    }
    #endregion
}