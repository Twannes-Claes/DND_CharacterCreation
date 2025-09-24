using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private GameObject _canvasAPI;

    [SerializeField]
    private Button _buttonSwitcher;
    #endregion

    #region LifeCycle
    private void OnEnable()
    {
        _buttonSwitcher.onClick.AddListener(ToggleAPIUI);
    }

    private void OnDisable()
    {
        _buttonSwitcher.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions
    private void ToggleAPIUI()
    {
        _canvasAPI.SetActive(!_canvasAPI.activeSelf);
    }
    #endregion
}
