using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForwardScrollToScrollRect : MonoBehaviour, IScrollHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Editor Fields
    [SerializeField] 
    private ScrollRect _scrollRect;
    #endregion

    #region Fields
    private bool _isMobile;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _isMobile = Application.isMobilePlatform;
        _scrollRect.horizontal = false;
    }
    #endregion

    #region Interface
    public void OnScroll(PointerEventData eventData)
    {
        if (!_isMobile && _scrollRect != null)
        {
            _scrollRect.OnScroll(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isMobile && _scrollRect != null)
        {
            _scrollRect.OnBeginDrag(eventData);
            Gamemanager.Instance.StopPanning = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isMobile && _scrollRect != null)
        {
            _scrollRect.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isMobile && _scrollRect != null)
        {
            _scrollRect.OnEndDrag(eventData);
            Gamemanager.Instance.StopPanning = false;
        }
    }
    #endregion
}
