using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private GameObject _canvasAPI;

    [SerializeField]
    private Button _buttonSwitcher;

    [SerializeField]
    private CanvasGroup _inputFieldGroup;

    [SerializeField]
    private TMPro.TMP_InputField _proficiencyBonus;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _inputFieldGroup.interactable = !_inputFieldGroup.interactable;
        }
    }
    #endregion

    #region Functions
    private void ToggleAPIUI()
    {
        _canvasAPI.SetActive(!_canvasAPI.activeSelf);
    }
    #endregion
}
