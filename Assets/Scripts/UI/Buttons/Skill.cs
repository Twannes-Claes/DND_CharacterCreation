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
    private LongPressEvent _longPress = null;

    private bool _hasProficiency = false;
    private bool _hasExpertise = false;

    private readonly int _proficiencyBonus = 2;
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
            _longPress = _proficiencyToggle.gameObject.GetComponent<LongPressEvent>();
        }
    }

    private void OnEnable()
    {
        _abilityScore = GameManager.Instance.GetAbilityScore(_source);
        _abilityScore.OnAbilityScoreChanged += CalculateModifier;

        if (_proficiencyToggle != null)
        {
            _proficiencyToggle.onValueChanged.AddListener(SetProficiency);
            _expertiseImage = _proficiencyToggle.transform.GetChild(0).GetChild(1).gameObject;

            if (_longPress != null)
            {
                _longPress.OnLongPress += MakeExpertised; 
            }
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

            if (_longPress != null)
            {
                _longPress.OnLongPress -= MakeExpertised;
            }
        }
    }

    #endregion

    #region GameLoop
    #endregion

    #region Functions
    public void SetProficiency(bool isProficient)
    {
        SetProficiency(isProficient, false);
    }

    public void SetProficiency(bool isProficient, bool isExpertised)
    {
        if (_proficiencyToggle == null) 
            return;

        if (_hasProficiency == isProficient)
            return;

        _hasProficiency = isProficient;
        _hasExpertise = isExpertised && _hasProficiency;

        _expertiseImage.SetActive(_hasExpertise);

        CalculateModifier(_abilityScore.AbilityModifier);
    }

    public void MakeExpertised()
    {
        SetProficiency(true, true);
    }

    private void CalculateModifier(int abilityModifier)
    {
        if (_hasProficiency)
        {
            int expertiseBonus = _hasExpertise ? 2 : 1;
            abilityModifier += _proficiencyBonus * expertiseBonus;
        }

        if (_skill == SkillTypes.PassivePerception)
        {
            abilityModifier += 10;
            SetText(abilityModifier, false);
            return;
        }

        SetText(abilityModifier);
    }

    private void SetText(int modifier, bool signedNumber = true)
    {
        if (signedNumber)
        {
            _textComp.SetText(GameManager.SignedNumberToString(modifier));
        }
        else
        {
            _textComp.SetText(modifier.ToString());
        }
    }
    #endregion
}
