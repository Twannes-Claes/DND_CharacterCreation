using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class EditModeButton : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] TextMeshProUGUI _text;
    #endregion

    #region Fields
    private Button _button = null;

    private const string TAG_ON = "Edit-ON";
    private const string TAG_OFF= "Edit-OFF";
    #endregion

    #region LifeCycle
    private void OnEnable()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);

        _text.text = GameManager.Instance.EditMode ? TAG_ON : TAG_OFF;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions
    private void OnClick()
    {
        GameManager.Instance.ToggleEditMode();

        _text.text = GameManager.Instance.EditMode ? TAG_ON : TAG_OFF;
    }
    #endregion
}