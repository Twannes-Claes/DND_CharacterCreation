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
    #endregion

    #region  Fields
    private bool _isSavingThrow = false;
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

            ResetTransform();

            bool isValid = IsValid;

            _infoInput.gameObject.SetActive(isValid);
            _typeButton.gameObject.SetActive(isValid);

            if (isValid == false)
            {
                _spellToggle.isOn = false;
            }

            _spellToggle.interactable = isValid;
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
        });

        _typeButton.onClick.AddListener(() =>
        {
            _isSavingThrow = !_isSavingThrow;
            _typeImage.sprite = _isSavingThrow ? Settings.Instance.SpellSaveSprite : Settings.Instance.AttackBonusSprite;
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

    public Spell ToSpell()
    {
        return new Spell(-1, _nameInput.text, _infoInput.text, _spellToggle.isOn, _isSavingThrow);
    }

    private void ResetTransform()
    {
        foreach (RectTransform rect in _nameInput.transform.GetChild(0))
        {
            if (rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero)
                return;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        foreach (RectTransform rect in _infoInput.transform.GetChild(0))
        {
            if (rect.offsetMin == Vector2.zero && rect.offsetMax == Vector2.zero)
                return;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
    #endregion
}
