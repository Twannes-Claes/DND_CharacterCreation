using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AttackField : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _bonusInput;
    [SerializeField] private TMP_InputField _damageInput;

    [SerializeField] private Button _removeButton;
    #endregion

    #region Fields
    public static AttacksManager Manager;
    private Attack _attack;

    private bool _isFirst = false;
    #endregion

    #region LifeCycle
    private void OnEnable()
    {
        _removeButton.onClick.AddListener(OnClick);
        GameManager.Instance.EditModeToggled += OnEditMode;
    }

    private void OnDisable()
    {
        _removeButton.onClick.RemoveListener(OnClick);
        GameManager.Instance.EditModeToggled -= OnEditMode;
    }
    #endregion

    #region Functions

    public void Initialize(Attack attack, bool isFirst)
    {
        _isFirst = isFirst;
        _attack = attack;

        Image imageComp = _removeButton.gameObject.GetComponent<Image>();

        if (attack.isSpell)
        {
            _removeButton.gameObject.SetActive(false);
        }
        else
        {
            _removeButton.gameObject.SetActive(GameManager.Instance.EditMode);
        }

        if (isFirst)
        {
            imageComp.sprite = Settings.Instance.Checkmark;
            gameObject.SetActive(GameManager.Instance.EditMode);
            return;
        }

        _nameInput.text = attack.name;
        _bonusInput.text = attack.bonus;
        _damageInput.text = attack.damage;

        if (attack.isSpell)
        {
            _nameInput.interactable = false;
            _bonusInput.interactable = false;
            _damageInput.interactable = false;

            return;
        }

        imageComp.sprite = Settings.Instance.Cross;
    }

    public Attack GetAttack()
    {
        return new Attack(_nameInput.text, _bonusInput.text, _damageInput.text, _attack.isSpell);
    }

    public void OnClick()
    {
        if (_isFirst)
        {
            if (string.IsNullOrEmpty(_nameInput.text))
                return;
        
            Manager.AddField(GetAttack());
            _nameInput.text = string.Empty;
            _bonusInput.text = string.Empty;
            _damageInput.text = string.Empty;
        
            return;
        }
        
        Manager.RemoveField(this);
    }

    public void OnEditMode(bool editMode)
    {
        if (_attack.isSpell)
            return;

        _removeButton.gameObject.SetActive(editMode);
    }
    #endregion
}
