using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SpellField : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _infoInput;

    [SerializeField] private Button _typeButton;
    [SerializeField] private Image _typeImage;

    [SerializeField] private Toggle _spellToggle;

    [SerializeField] private bool _isCantrip;
    #endregion

    #region  Fields
    private bool _isSavingThrow = false;
    public static ISaveable FieldSaveable;
    #endregion

    #region Properties
    public Toggle SpellToggle => _spellToggle;

    public bool IsValid => !string.IsNullOrEmpty(_nameInput.text);
    #endregion  

    #region LifeCycle
    private void OnEnable()
    {
        _nameInput.onEndEdit.AddListener((text) =>
        {
            _nameInput.text = text.TrimEnd();

            bool isValid = IsValid;

            _infoInput.gameObject.SetActive(isValid);
            _typeButton.gameObject.SetActive(isValid);

            if (isValid == false)
            {
                _spellToggle.isOn = false;
            }

            _spellToggle.interactable = isValid;

            FieldSaveable.Save(GameManager.Instance.CharacterSheet);
        });

        _infoInput.onSelect.AddListener((text) =>
        {
            _infoInput.textComponent.alignment = TextAlignmentOptions.Bottom;
            _infoInput.placeholder.gameObject.SetActive(false);
        });

        _infoInput.onEndEdit.AddListener((text) =>
        {
            _infoInput.textComponent.alignment = TextAlignmentOptions.BottomRight;
            _infoInput.placeholder.gameObject.SetActive(true);

            FieldSaveable.Save(GameManager.Instance.CharacterSheet);
        });

        _typeButton.onClick.AddListener(() =>
        {
            _isSavingThrow = !_isSavingThrow;
            _typeImage.sprite = _isSavingThrow ? Settings.Instance.SpellSaveSprite : Settings.Instance.AttackBonusSprite;

            FieldSaveable.Save(GameManager.Instance.CharacterSheet);
        });
    }

    private void OnDisable()
    {
        _nameInput.onEndEdit.RemoveAllListeners();
        _infoInput.onEndEdit.RemoveAllListeners();

        _infoInput.onSelect.RemoveAllListeners();

        _typeButton.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions
    public void Initialize(Spell spell)
    {
        _nameInput.text = spell.name;
        _infoInput.text = spell.info;

        _spellToggle.isOn = spell.isPrepared;

        _isSavingThrow = spell.isSavingThrow;
        _typeImage.sprite = _isSavingThrow ? Settings.Instance.SpellSaveSprite : Settings.Instance.AttackBonusSprite;

        _infoInput.gameObject.SetActive(IsValid);
        _typeButton.gameObject.SetActive(IsValid);
    }

    public void Reset()
    {
        _nameInput.text = string.Empty;
        _infoInput.text = string.Empty;

        _spellToggle.isOn = false;

        _isSavingThrow = false;
        _typeImage.sprite = Settings.Instance.AttackBonusSprite;

        _infoInput.gameObject.SetActive(false);
        _typeButton.gameObject.SetActive(false);
    }

    public Spell ToSpell(int index)
    {
        return new Spell(index, _nameInput.text, _infoInput.text, _spellToggle.isOn, _isSavingThrow, _isCantrip);
    }
    #endregion
}
