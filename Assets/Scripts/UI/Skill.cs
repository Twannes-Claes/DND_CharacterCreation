using UnityEngine.UI;
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

    [SerializeField]
    private Toggle _proficiencyToggle;
    #endregion

    #region Fields
    private TMP_Text _textComp = null;
    private GameObject _expertiseImage = null;
    private AbilityScoreInputField _abilityScore = null;

    private bool _hasProficiency = false;
    private bool _hasExpertise = false;

    private int _proficiencyBonus = 2;
    #endregion

    #region Properties
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _textComp = GetComponent<TMP_Text>();

        if (_proficiencyToggle != null)
        {
            SetProficiency(_proficiencyToggle.isOn);
        }
    }

    private void OnEnable()
    {
        _abilityScore = Gamemanager.Instance.GetAbilityScore(_source);
        _abilityScore.OnAbilityScoreChanged += CalculateModifier;

        if (_proficiencyToggle != null)
        {
            _proficiencyToggle.onValueChanged.AddListener(SetProficiency);
            _expertiseImage = _proficiencyToggle.transform.GetChild(0).GetChild(1).gameObject;
        }
    }

    private void OnDisable()
    {
        if (_abilityScore != null)
        {
            _abilityScore.OnAbilityScoreChanged -= CalculateModifier;
        }

        if (_proficiencyToggle != null)
        {
            _proficiencyToggle.onValueChanged.RemoveAllListeners();
        }
    }

    #endregion

    #region GameLoop
    #endregion

    #region Functions
    public void SetProficiency(bool isProficient)
    {
        if (_proficiencyToggle == null) 
            return;

        if (_hasProficiency == isProficient) 
            return;

        _hasProficiency = isProficient;
        _hasExpertise = false;

        if (Gamemanager.Instance.ExpertiseInput && _hasProficiency)
        {
            _hasExpertise = true;
        }

        _expertiseImage.SetActive(_hasExpertise);

        CalculateModifier(_abilityScore.AbilityModifier);
    }

    private void CalculateModifier(int abilityModifier)
    {
        if (_hasProficiency)
        {
            int expertiseBonus = _hasExpertise ? 2 : 1;
            abilityModifier += _proficiencyBonus * expertiseBonus;
        }

        SetText(abilityModifier);
    }

    private void SetText(int modifier)
    {
        string sign = modifier >= 0 ? "+" : "";

        _textComp.SetText($"{sign}{modifier}");
    }
    #endregion
}
