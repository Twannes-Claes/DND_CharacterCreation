using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_InputField))]
public class AbilityScoreInputField : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private AbilityScores _abilityScore;
    #endregion

    #region Fields
    private TMP_InputField _inputField;
    #endregion

    #region Properties
    public Action<int> OnAbilityScoreChanged;

    public AbilityScores AbilityScore => _abilityScore;

    public int AbilityModifier => Utils.ToModifier(_inputField.text);
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        _inputField.onEndEdit.AddListener((text) =>
        {
            OnAbilityScoreChanged?.Invoke(AbilityModifier);

            Save(GameManager.Instance.CharacterSheet);
        });
    }

    private void OnDisable()
    {
        _inputField.onEndEdit.RemoveAllListeners();
    }

    #endregion

    #region GameLoop
    #endregion

    #region Functions
    public void SetText(string text)
    {
        _inputField.text = text;
    }

    public void Save(Character sheet)
    {
        sheet.AbilityScores[(int)_abilityScore] = int.Parse(_inputField.text);
    }

    public void Load(Character sheet)
    {
        SetText(sheet.AbilityScores[(int)_abilityScore].ToString());
        OnAbilityScoreChanged?.Invoke(AbilityModifier);
    }
    #endregion
}
