using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System;

[RequireComponent(typeof(TMP_InputField))]
public class GeneralInputField : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private GeneralInputType _inputType;
    #endregion

    #region Fields
    private TMP_InputField _inputField;
    private RectTransform _textAreaTransform;

    private Action<int> OnAbilityScoreChangedLogic;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _textAreaTransform = (RectTransform)_inputField.textComponent.rectTransform.parent;
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

            Save(GameManager.Instance.CharacterSheet);
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
        foreach (RectTransform rect in _textAreaTransform)
        {
            if (rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero)
                return;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }

    private void ApplyPreEditLogic()
    {
        switch (_inputType)
        {
            case GeneralInputType.SpellcastingAttackBonus:
            case GeneralInputType.ProfiencyBonus:
            case GeneralInputType.Speed:
            {
                _inputField.text = Regex.Replace(_inputField.text, @"\D", "");
            }
            break;

            case GeneralInputType.SpellcastingSave:
            {
                _inputField.text = _inputField.text.Replace("DC ", "");
            }
            break;

            case GeneralInputType.Proficiencies:
            case GeneralInputType.Languages:
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
        if (_inputField.characterValidation == TMP_InputField.CharacterValidation.Integer)
        {
            if (int.TryParse(_inputField.text, out int amount))
            {
                amount = Mathf.Max(0, amount);
                _inputField.text = amount.ToString();
            }
        }

        switch (_inputType)
        {
            case GeneralInputType.ProfiencyBonus:
            {
                if (int.TryParse(_inputField.text, out int modifier))
                {
                    _inputField.text = Utils.ToSignedNumber(modifier);
                    Save(GameManager.Instance.CharacterSheet);
                    GameManager.Instance.RefreshAbilityScores();
                }
            }
            break;

            case GeneralInputType.SpellcastingAttackBonus:
            {
                if (int.TryParse(_inputField.text, out int number))
                {
                    _inputField.text = Utils.ToSignedNumber(number);
                }
            }
            break;

            case GeneralInputType.Speed:
            {
                _inputField.text += " ft.";
            }
            break;

            case GeneralInputType.SpellcastingSave:
            {
                _inputField.text = $"DC {_inputField.text}";
            }
            break;

            case GeneralInputType.Proficiencies:
            case GeneralInputType.Languages:
            case GeneralInputType.Feats:
            {
                _inputField.richText = true;
            }
            break;

            case GeneralInputType.MaxHitDice:
            {
                _inputField.text = Utils.UpdateDiceModifier(GameManager.Instance.GetAbilityScore(AbilityScores.Constitution).AbilityModifier, _inputField.text);
            }
            break;

            case GeneralInputType.CurrentHitDice:
            {
                if (int.TryParse(_inputField.text, out int currentHitDice))
                {
                    int maxDiceAmount = Utils.GetDiceCount(GameManager.Instance.CharacterSheet.CharacterInfo[(int)GeneralInputType.MaxHitDice]);

                    currentHitDice = Mathf.Clamp(currentHitDice, 0, maxDiceAmount);

                    _inputField.text = currentHitDice.ToString();
                }
            }
            break;

            case GeneralInputType.CurrentHitPoints:
            {
                if (int.TryParse(_inputField.text, out int currentHp))
                {
                    if (int.TryParse(GameManager.Instance.CharacterSheet.CharacterInfo[(int)GeneralInputType.MaxHitPoints], out int maxHp) )
                    {
                        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
                    }

                    _inputField.text = currentHp.ToString();
                }
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
                    _inputField.text = Utils.UpdateDiceModifier(score, _inputField.text);
                };

                GameManager.Instance.GetAbilityScore(AbilityScores.Constitution).OnAbilityScoreChanged += OnAbilityScoreChangedLogic;
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
                    GameManager.Instance.GetAbilityScore(AbilityScores.Constitution).OnAbilityScoreChanged -= OnAbilityScoreChangedLogic;

                    OnAbilityScoreChangedLogic = null;
                }
            }
            break;

            default:
                break;
        }
    }

    public void Save(Character sheet)
    {
        sheet.CharacterInfo[(int)_inputType] = _inputField.text;
    }

    public void Load(Character sheet)
    {
        if (_inputField == null || _textAreaTransform == null)
        {
            Awake();
        }

        _inputField.text = sheet.CharacterInfo[(int)_inputType];

        ApplyPreEditLogic();
        ApplyPostEditLogic();
    }
    #endregion
}
