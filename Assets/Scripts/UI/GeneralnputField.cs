using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System;

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
        ProfiencyBonus,
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
        Equipment,
        TemporaryHitPoints,
        Copper,
        Silver,
        Electrum,
        Gold,
        Platinum,
        Inspiration
    };

    #region Editor Fields
    [SerializeField]
    private GeneralInputType _inputType;
    #endregion

    #region Fields
    private TMP_InputField _inputField;
    private RectTransform _transform;

    private Action<int> OnAbilityScoreChangedLogic;
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

            //Save
            //Save[_inputType] = _inputField.text;
        });

        ApplyEnableLogic();
    }

    private void OnDisable()
    {
        _inputField.onSelect.RemoveAllListeners();
        _inputField.onEndEdit.RemoveAllListeners();

        ApplyDisableLogic();
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
            case GeneralInputType.ProfiencyBonus:
            case GeneralInputType.Speed:
            {
                _inputField.text = Regex.Replace(_inputField.text, @"\D", "");
            }
            break;

            case GeneralInputType.Feats:
            {
                _inputField.richText = false;
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
            case GeneralInputType.ProfiencyBonus:
            {
                if (int.TryParse(_inputField.text, out int modifier))
                {
                    _inputField.text = Gamemanager.SignedNumberToString(modifier);
                }
            }
            break;

            case GeneralInputType.Speed:
            {
                _inputField.text += " ft.";
            }
            break;

            case GeneralInputType.Feats:
            {
                _inputField.richText = true;
            }
            break;

            case GeneralInputType.MaxHitDice:
            {
                _inputField.text = Gamemanager.UpdateDiceModifier(Gamemanager.Instance.GetAbilityScore(AbilityScores.Constitution).AbilityModifier, _inputField.text);
            }
            break;

            default:
            break;
        }
    }

    private void ApplyEnableLogic()
    {
        switch (_inputType)
        {
            case GeneralInputType.MaxHitDice:
            {
                OnAbilityScoreChangedLogic = (score) =>
                {
                    _inputField.text = Gamemanager.UpdateDiceModifier(score, _inputField.text);
                };

                Gamemanager.Instance.GetAbilityScore(AbilityScores.Constitution).OnAbilityScoreChanged += OnAbilityScoreChangedLogic;
            }
            break;

            default:
                break;
        }
    }

    private void ApplyDisableLogic()
    {
        switch (_inputType)
        {
            case GeneralInputType.MaxHitDice:
            {
                if (OnAbilityScoreChangedLogic != null)
                {
                    Gamemanager.Instance.GetAbilityScore(AbilityScores.Constitution).OnAbilityScoreChanged -= OnAbilityScoreChangedLogic;

                    OnAbilityScoreChangedLogic = null;
                }
            }
            break;

            default:
                break;
        }
    }
    #endregion
}
