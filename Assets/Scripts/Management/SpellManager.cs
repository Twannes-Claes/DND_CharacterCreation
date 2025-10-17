using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellManager : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private List<Toggle> _spellToggles = new List<Toggle>();

    [SerializeField] private TMP_InputField _limitInput;
    #endregion

    #region Fields
    private int _prepareLimit = 0;
    private int _currentPrepared = 0;

    private List<int> _activeToggleIndexes = new List<int>();
    #endregion

    #region LifeCycle
    private void OnEnable()
    {
        for (int i = 0; i < _spellToggles.Count; i++)
        {
            //i is a reference, so won't pass the correct one to the lambda
            //make a value instead
            int index = i;
            Toggle toggle = _spellToggles[i];
            toggle.onValueChanged.AddListener((bool value) => OnToggleChanged(value, index));
        }

        _limitInput.onEndEdit.AddListener(OnLimitChanged);
    }

    private void OnDisable()
    {
        _limitInput.onEndEdit.RemoveAllListeners();

        foreach (Toggle toggle in _spellToggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }

    #endregion

    #region Functions
    private void OnToggleChanged(bool isOn, int index)
    {
        int modifier = isOn ? +1 : -1;

        _currentPrepared = Mathf.Max(0, _currentPrepared + modifier);

        if (isOn)
        {
            _activeToggleIndexes.Add(index);
        }
        else
        {
            _activeToggleIndexes.Remove(index);
        }

        UpdateToggleInteractability();
    }

    private void OnLimitChanged(string limit)
    {
        if (int.TryParse(limit, out int val))
        {
            _prepareLimit = Mathf.Max(0, val);
            UpdateToggleInteractability();
        }
    }

    private void UpdateToggleInteractability()
    {
        bool atLimit = _currentPrepared >= _prepareLimit;

        foreach (Toggle toggle in _spellToggles)
        {
            toggle.interactable = toggle.isOn || !atLimit;
        }
    }

    public void Load(Character sheet)
    {
        _currentPrepared = sheet.PreparedSpells.Count;

        foreach(Spell p in sheet.PreparedSpells)
        {
            _spellToggles[p.index].isOn = true;
        }

        OnLimitChanged(sheet.CharacterInfo[(int)GeneralInputType.SpellsPrepared]);
    }

    public void Save(Character sheet)
    {
        sheet.PreparedSpells.Clear();

        foreach(int index in _activeToggleIndexes)
        {
            sheet.PreparedSpells.Add(new Spell(index, "", false));
        }
    }
    #endregion
}
