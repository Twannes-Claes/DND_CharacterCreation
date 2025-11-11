using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellSlotField : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private int _level;

    [SerializeField] private TMP_InputField _totalInput;

    [SerializeField] private Transform _layoutGroup;
    [SerializeField] private GameObject _togglePrefab;
    #endregion

    #region  Fields
    private SpellSlot _spellSlot = new SpellSlot(0,0);

    private int _previousTotal = 0;

    private readonly List<Toggle> _toggles = new List<Toggle>();
    #endregion

    #region Properties
    #endregion  

    #region LifeCycle
    private void OnEnable()
    {
        _totalInput.onEndEdit.AddListener(OnTotalChanged);
        BuildToggles();

        GameManager.Instance.EditModeToggled += EditModeChanged;
        EditModeChanged(GameManager.Instance.EditMode);
    }

    private void OnDisable()
    {
        _totalInput.onEndEdit.RemoveAllListeners();
        ClearToggles();

        GameManager.Instance.EditModeToggled -= EditModeChanged;
    }
    #endregion

    #region Functions

    private void SpellSlotToggled(bool used)
    {
        int modifier = used ? 1 : -1;

        _spellSlot.used += modifier;
        _spellSlot.used = Mathf.Clamp(_spellSlot.used, 0, Settings.Instance.MaxSpellSlotsPerLevel);
    }

    private void OnTotalChanged(string text)
    {
        if (!int.TryParse(text, out int total))
        {
            _totalInput.text = _previousTotal.ToString();
            return;
        }

        total = Mathf.Clamp(total, 0, Settings.Instance.MaxSpellSlotsPerLevel);

        if (total == _previousTotal)
        {
            _totalInput.text = total.ToString();
            return;
        }

        _previousTotal = total;
        _spellSlot.total = total;
        _totalInput.text = total.ToString();

        _spellSlot.used = Mathf.Clamp(_spellSlot.used, 0, _spellSlot.total);

        BuildToggles();
    }

    private void BuildToggles()
    {
        ClearToggles();

        for (int i = 0; i < _spellSlot.total; i++)
        {
            if (Instantiate(_togglePrefab, _layoutGroup).TryGetComponent(out Toggle toggle))
            {
                toggle.isOn = i < _spellSlot.used;
                toggle.onValueChanged.AddListener(SpellSlotToggled);
                _toggles.Add(toggle);
            }
        }
    }

    private void ClearToggles()
    {
        foreach (Toggle toggle in _toggles)
        {
            if (toggle)
            {
                toggle.onValueChanged.RemoveAllListeners();
                Destroy(toggle.gameObject);
            }
        }
        _toggles.Clear();
    }

    private void EditModeChanged(bool editMode)
    {
        _totalInput.interactable = editMode;
    }

    public void Save(Character sheet)
    {
        sheet.SpellSlots[_level-1] = _spellSlot;
    }

    public void Load(Character sheet)
    {
        ClearToggles();

        SpellSlot slot = sheet.SpellSlots[_level-1];

        _spellSlot = slot;
        _previousTotal = slot.total;
        _totalInput.text = slot.total.ToString();

        BuildToggles();
    }
    #endregion
}
