using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SpellManager : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private List<SpellField> _spellFields = new List<SpellField>();

    [SerializeField] private TMP_InputField _limitInput;
    #endregion

    #region Fields
    private int _prepareLimit = 0;
    private int _currentPrepared = 0;
    #endregion

    #region Events
    public static Action OnDataChanged;
    #endregion


    #region LifeCycle
    private void OnEnable()
    {
        for (int i = 0; i < _spellFields.Count; i++)
        {
            ////i is a reference, so won't pass the correct one to the lambda
            ////make a value instead
            //int index = i;
            _spellFields[i].SpellToggle.onValueChanged.AddListener((bool value) => OnToggleChanged(value));
        }

        _limitInput.onEndEdit.AddListener(OnLimitChanged);
    }

    private void OnDisable()
    {
        _limitInput.onEndEdit.RemoveAllListeners();

        foreach (SpellField spell in _spellFields)
        {
            spell.SpellToggle.onValueChanged.RemoveAllListeners();
        }
    }

    #endregion

    #region Functions
    private void OnToggleChanged(bool isOn)
    {
        int modifier = isOn ? +1 : -1;

        _currentPrepared = Mathf.Max(0, _currentPrepared + modifier);

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
        bool atLimit = _currentPrepared == _prepareLimit;

        foreach (SpellField spell in _spellFields)
        {
            if (spell.IsValid == false)
                continue;

            Toggle spellToggle = spell.SpellToggle;

            spellToggle.interactable = spell.SpellToggle.isOn || !atLimit;
        }

        Save(GameManager.Instance.CharacterSheet);
    }

    public void Load(Character sheet)
    {
        _currentPrepared = 0;

        SpellField.FieldSaveable = this;

        foreach(SpellField field in _spellFields)
        {
            field.Reset();
        }

        for (int i = 0; i < sheet.Spells.Count; i++)
        {
            Spell spell = sheet.Spells[i];

            _spellFields[spell.index].Initialize(spell);

            if (spell.isPrepared)
            {
                _currentPrepared++;
            }
        }

        OnLimitChanged(sheet.CharacterInfo[(int)GeneralInputType.SpellsPrepared]);
    }

    public void Save(Character sheet)
    {
        sheet.Spells.Clear();

        for (int i = 0; i < _spellFields.Count; i++)
        {
            SpellField field = _spellFields[i];

            if (field.IsValid == false)
                continue;

            Spell spell = field.ToSpell(i);

            sheet.Spells.Add(spell);
        }

        OnDataChanged?.Invoke();
    }
    #endregion
}
