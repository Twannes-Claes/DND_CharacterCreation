using UnityEngine;
using UnityEngine.EventSystems;

public class StopScrollOnEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields
    private bool _isMobile;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _isMobile = Application.isMobilePlatform;
    }
    #endregion

    #region Functions
    public void OnPointerEnter(PointerEventData _)
    {
        if (!_isMobile)
        {
            GameManager.Instance.StopScrolling = true;
        }
    }

    public void OnPointerExit(PointerEventData _)
    {
        if (!_isMobile)
        {
            GameManager.Instance.StopScrolling = false;
        }
    }
    #endregion
}
