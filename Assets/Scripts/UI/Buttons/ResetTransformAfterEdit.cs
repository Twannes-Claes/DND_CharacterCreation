using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetTransformAfterEdit : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private TMP_InputField _inputField;

    #endregion

    #region Fields
    private RectTransform _textAreaTransform = null;
    #endregion

    #region LifeCycle

    private void OnEnable()
    {
        _textAreaTransform = (RectTransform)_inputField.textComponent.rectTransform.parent;

        _inputField.onEndEdit.AddListener((text) =>
        {
            foreach (RectTransform rect in _textAreaTransform)
            {
                if (rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero)
                    return;

                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
        });
    }

    private void OnDisable()
    {
        _inputField.onEndEdit.RemoveAllListeners();
    }

    #endregion
}
