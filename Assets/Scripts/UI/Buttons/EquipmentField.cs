using System.Collections;
using System.Collections.Generic;
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
    private void OnEnable() => _removeButton.onClick.AddListener(OnClick);
    private void OnDisable() => _removeButton.onClick.RemoveListener(OnClick);
    #endregion

    #region Functions

    public void Initialize(Equipment equipment, bool isFirst)
    {
        _isFirst = isFirst;

        Image imageComp = _removeButton.gameObject.GetComponent<Image>();

        //Blank text if its the first one
        if (isFirst)
        {
            imageComp.sprite = Settings.Instance.Checkmark;
            imageComp.color = Settings.Instance.Green;

            return;
        }

        _nameInput.text = equipment.name;
        _amountInput.text = equipment.amount.ToString();

        imageComp.sprite = Settings.Instance.Cross;
        imageComp.color = Settings.Instance.Red;
    }

    public Equipment GetEquipment()
    {
        int.TryParse(_amountInput.text, out int amount);

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
    #endregion
}
