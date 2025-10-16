using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EquipmentField : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _amountInput;

    [SerializeField] private Button _removeButton;
    #endregion

    #region Fields
    public static EquipmentManager Manager;

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

    public void Initialize(Equipment equipment, bool isFirst)
    {
        _isFirst = isFirst;


        Image imageComp = _removeButton.gameObject.GetComponent<Image>();
        _removeButton.gameObject.SetActive(GameManager.Instance.EditMode);

        if (isFirst)
        {
            imageComp.sprite = Settings.Instance.Checkmark;
            gameObject.SetActive(GameManager.Instance.EditMode);
            return;
        }

        _nameInput.text = equipment.name;
        _amountInput.text = equipment.amount.ToString();

        imageComp.sprite = Settings.Instance.Cross;
    }

    public Equipment GetEquipment()
    {
        if (int.TryParse(_amountInput.text, out int amount) == false)
        {
            amount = 1;
        }

        return new Equipment(_nameInput.text, amount);
    }

    public void OnClick()
    {
        if (_isFirst)
        {
            if (string.IsNullOrEmpty(_nameInput.text))
                return;

            Manager.AddField(GetEquipment());
            _nameInput.text = string.Empty;
            _amountInput.text = string.Empty;

            return;
        }

        Manager.RemoveField(this);
    }

    public void OnEditMode(bool editMode)
    {
        _removeButton.gameObject.SetActive(editMode);

        //if (editMode)
        //{
        //    //RectTransform rect = (RectTransform)_nameInput.transform;
        //
        //    //rect.anchorMin
        //    //rect.anchorMax
        //}
        //else
        //{
        //
        //}
    }
    #endregion
}
