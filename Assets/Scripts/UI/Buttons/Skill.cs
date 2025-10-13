using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;

[RequireComponent(typeof(TMP_Text))]
public class Skill : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private AbilityScores _source;
    [SerializeField] private SkillTypes _skill;
    [SerializeField] private Toggle _proficiencyToggle;
    #endregion

    #region Fields
    private TMP_Text _textComp = null;
    private GameObject _expertiseImage = null;

    private AbilityScoreInputField _abilityScore = null;
    private LongPressEvent _longPress = null;

    private bool _hasProficiency = false;
    private bool _hasExpertise = false;

    private bool _queueExpertise = false;
    #endregion

    #region Properties
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _textComp = GetComponent<TMP_Text>();

        if (_proficiencyToggle != null)
        {
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
                _longPress.OnLongPress += QueueExpertise; 
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
                _longPress.OnLongPress -= QueueExpertise;
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
        if (_proficiencyToggle == null || _hasProficiency == isProficient) 
            return;

        if (_queueExpertise)
        {
            isExpertised = true;
            _queueExpertise = false;
        }

        _hasProficiency = isProficient;
        _hasExpertise = isExpertised && _hasProficiency;

        _proficiencyToggle.isOn = isProficient;
        _expertiseImage.SetActive(_hasExpertise);

        CalculateModifier(_abilityScore.AbilityModifier);
        Save(GameManager.Instance.CharacterSheet);
    }

    public void QueueExpertise()
    {
        Debug.Log("Exppertise queued");
        if (_hasExpertise == false)
            _queueExpertise = true;
    }

    private void CalculateModifier(int abilityModifier)
    {
        if (_hasProficiency)
        {
            int expertiseBonus = _hasExpertise ? 2 : 1;
            int proficiencyBonus = int.Parse(GameManager.Instance.GetCharacterInfo(GeneralInputType.ProfiencyBonus));

            abilityModifier += proficiencyBonus * expertiseBonus;
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
        _textComp.SetText(signedNumber ? Utils.ToSignedNumber(modifier) : modifier.ToString());
    }

    public void Save(Character sheet)
    {
        Proficiency proficiency = sheet.SkillProficiencies.FirstOrDefault(p => p.skill == (int)_skill);

        if (proficiency.skill == 0 /*default*/ && _hasProficiency)
        {
            sheet.SkillProficiencies.Add(new Proficiency((int)_skill, _hasExpertise));
        }
        else if (proficiency.skill > 0 && _hasProficiency == false)
        {
            sheet.SkillProficiencies.Remove(proficiency);
        }
    }

    public void Load(Character sheet)
    {
        if (_proficiencyToggle == null)
            return;

        Proficiency proficiency = sheet.SkillProficiencies.FirstOrDefault(p => p.skill == (int)_skill);

        if (proficiency.skill == (int)_skill)
        {
            SetProficiency(true, proficiency.expertised);
        }
    }
    #endregion
}
