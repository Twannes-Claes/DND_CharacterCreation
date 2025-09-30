using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_InputField))]
public class AbilityScoreInputField : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private AbilityScores _abilityScore;
    #endregion

    #region Fields
    private TMP_InputField _inputField;
    #endregion

    #region Properties
    public Action<int> OnAbilityScoreChanged;

    public AbilityScores AbilityScore => _abilityScore;

    public int AbilityModifier => AbilityScoreToModifier(_inputField.text);
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
    private static int AbilityScoreToModifier(string text)
    {
        if (int.TryParse(text, out int abilityScore))
        {
            return Mathf.FloorToInt((abilityScore - 10) / 2f);
        }

        return -9;
    }
    #endregion
}
