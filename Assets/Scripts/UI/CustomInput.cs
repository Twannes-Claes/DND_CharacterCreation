using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(TMP_InputField))]
public class CustomInput : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private List<TMP_Text> _linkedText = new List<TMP_Text>();

    [SerializeField]
    private bool IsAbilityScore = false;
    #endregion

    #region Fields
    private TMP_InputField _inputField;
    private RectTransform _transform;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _transform = _inputField.textComponent.rectTransform;
    }

    private void OnEnable()
    {
        _inputField.onEndEdit.AddListener((text) =>
        {
            _inputField.text = text.TrimEnd();

            ResetTransform();
            ApplyInputToLinkedText();
        });

        Gamemanager.Instance.CacheInputField(this);
    }

    private void OnDisable()
    {
        _inputField.onEndEdit.RemoveAllListeners();
    }

    #endregion

    #region GameLoop
    #endregion

    #region Functions
    public void ApplyInputToLinkedText()
    {
        if (!IsAbilityScore)
            return;

        if (int.TryParse(_inputField.text, out int abilityScore))
        {
            int modifier = Mathf.FloorToInt((abilityScore - 10) / 2f);

            string sign = modifier >= 0 ? "+" : "";

            foreach (TMP_Text linked in _linkedText)
            {
                linked.text = sign + modifier.ToString();
            }
        }
    }

    private void ResetTransform()
    {
        if (_transform.offsetMin == Vector2.zero && _transform.offsetMax == Vector2.zero)
            return;

        _transform.offsetMin = Vector2.zero;
        _transform.offsetMax = Vector2.zero;
    }
    #endregion
}
