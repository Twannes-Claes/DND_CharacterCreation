using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Skill : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private AbilityScores _source;

    [SerializeField]
    private SkillTypes _skill;
    #endregion

    #region Fields
    private TMP_Text _textComp = null;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _textComp = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        Gamemanager.Instance.GetAbilityScore(_source).OnAbilityScoreChanged += SetText;
    }

    private void OnDisable()
    {
        Gamemanager.Instance.GetAbilityScore(_source).OnAbilityScoreChanged -= SetText;
    }

    #endregion

    #region GameLoop
    #endregion

    #region Functions
    private void SetText(int abilityModifier)
    {
        //TODO if proficient add profiencybonus

        string sign = abilityModifier >= 0 ? "+" : "";

        _textComp.SetText($"{sign}{abilityModifier}");
    }
    #endregion
}
