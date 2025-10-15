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
    private void Awake()
    {
        Image imageComp = _removeButton.gameObject.GetComponent<Image>();
        imageComp.sprite = Assets.Instance.Cross;
        imageComp.color = Assets.Instance.Red;
    }

    private void OnEnable()
    {
        _removeButton.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _removeButton.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions

    public void Initialize(Equipment equipment)
    {
        _nameInput.text = equipment.name;
        _amountInput.text = equipment.amount.ToString();
    }

    public void SetFirst()
    {
        _isFirst = true;

        Image imageComp = _removeButton.gameObject.GetComponent<Image>();
        imageComp.sprite = Assets.Instance.Checkmark;
        imageComp.color = Assets.Instance.Green;
    }

    public Equipment GetEquipment()
    {
        int amount = 0;

        if (int.TryParse(_amountInput.text, out int result))
        {
            amount = result;
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
    #endregion
}
