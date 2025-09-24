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
    private List<TMPro.TMP_InputField> _inputFields = new List<TMPro.TMP_InputField>();
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
            foreach (var field in _inputFields)
            {
                field.interactable = !field.IsInteractable();
            }
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
