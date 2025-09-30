using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TMP_InputField))]
public class GeneralInputField : MonoBehaviour
{
    public enum GeneralInputType
    {
        CharacterName = 0,
        ClassLevel,
        Background,
        PlayerName,
        Race,
        Alignment,
        ExperiencePoints,
        ArmorClass,
        Speed,
        MaxHitPoints,
        CurrentHitPoints,
        MaxHitDice,
        CurrentHitDice,
        Personality,
        Ideals,
        Bonds,
        Flaws,
        Feats,
        Profiencies,
        Equipment
    };

    #region Editor Fields
    [SerializeField]
    private GeneralInputType _inputType;
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
        _inputField.onSelect.AddListener((text) =>
        {
            ApplyPreEditLogic();
        });

        _inputField.onEndEdit.AddListener((text) =>
        {
            _inputField.text = text.TrimEnd();

            ResetTransform();
            ApplyPostEditLogic();
        });
    }

    private void OnDisable()
    {
        _inputField.onEndEdit.RemoveAllListeners();
    }
    #endregion

    #region GameLoop
    #endregion

    #region Functions
    private void ResetTransform()
    {
        if (_transform.offsetMin == Vector2.zero && _transform.offsetMax == Vector2.zero)
            return;

        _transform.offsetMin = Vector2.zero;
        _transform.offsetMax = Vector2.zero;
    }

    private void ApplyPreEditLogic()
    {
        switch (_inputType)
        {
            case GeneralInputType.Speed:
            {
                _inputField.text = Regex.Replace(_inputField.text, @"\D", "");
            }
            break;

            default:
            break;
        }
    }

    private void ApplyPostEditLogic()
    {
        switch (_inputType)
        {
            case GeneralInputType.Speed:
            {
                    _inputField.text += " ft.";
            }
            break;

            default:
            break;
        }
    }
    #endregion
}
