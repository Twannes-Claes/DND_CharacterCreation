using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

[DefaultExecutionOrder(-100)]
public class Gamemanager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private DetailPanel _detailPanel;

    [SerializeField]
    private List<AbilityScoreInputField> _abilityScores;
    #endregion

    #region Fields
    private readonly Dictionary<ApiCategoryType, ApiCategoryResource> _cachedCategories = new Dictionary<ApiCategoryType, ApiCategoryResource>();

    private readonly Dictionary<AbilityScores, AbilityScoreInputField> _cachedAbilityScores = new Dictionary<AbilityScores, AbilityScoreInputField>();
    #endregion

    #region Properties
    public DetailPanel DetailPanel => _detailPanel;

    public bool StopScrolling = false;
    public bool StopPanning = false;

    public bool ExpertiseInput
    {
        get { return Input.GetKey(KeyCode.X); }
    }
    #endregion 

    #region Statics
    public static Gamemanager Instance { get; private set; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (AbilityScoreInputField field in _abilityScores)
            {
                _cachedAbilityScores[field.AbilityScore] = field;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Simulate loading the ability scores and then setting the value

        foreach (AbilityScoreInputField field in _cachedAbilityScores.Values)
        {
            if (field != null)
            {
                field.OnAbilityScoreChanged?.Invoke(field.AbilityModifier);
            }
        }
    }
    #endregion

    #region Functions
    public AbilityScoreInputField GetAbilityScore(AbilityScores abilityScore)
    {
        if (_cachedAbilityScores.ContainsKey(abilityScore) == false)
            return null;

        return _cachedAbilityScores[abilityScore];
    }

    public async Task<ApiCategoryResource> FetchCategory(ApiCategoryType category)
    {
        if (_cachedCategories.TryGetValue(category, out var resource))
        {
            return resource;
        }

        ApiCategoryResource result = await ApiHelper.FetchCategoryAsync(category); ;
        if (result != null)
        {
            _cachedCategories[category] = result;
        }

        return result;
    }

    public void AddAbilityScore(AbilityScoreInputField inputField)
    {
        AbilityScores key = inputField.AbilityScore;

        if (!_cachedAbilityScores.ContainsKey(key))
        {
            _cachedAbilityScores[key] = inputField;
        }
    }

    public void RemoveAbilityScore(AbilityScoreInputField inputField)
    {
        AbilityScores key = inputField.AbilityScore;

        _cachedAbilityScores.Remove(key);
    }

    public static int AbilityScoreToModifier(string text)
    {
        if (int.TryParse(text, out int abilityScore))
        {
            return Mathf.FloorToInt((abilityScore - 10) / 2f);
        }

        return -99;
    }

    public static int AbilityScoreToModifier(int abilityScore)
    {
        return Mathf.FloorToInt((abilityScore - 10) / 2f);
    }

    public static string SignedNumberToString(int modifier)
    {
        return modifier >= 0 ? $"+{modifier}" : modifier.ToString();
    }

    public static string AbilityScoreToSignedModifier(string text)
    {
        return SignedNumberToString(AbilityScoreToModifier(text));
    }

    public static string AbilityScoreToSignedModifier(int score)
    {
        return SignedNumberToString(AbilityScoreToModifier(score));
    }

    public static string UpdateDiceModifier(int score, string diceText)
    {
        diceText = diceText.Replace(" ", "");

        var match = Regex.Match(diceText, @"^([0-9]*d[0-9]+)([+-]\d+)?$");

        if (!match.Success)
            return string.Empty; ;

        string dicePart = match.Groups[1].Value;
        string modifier = Gamemanager.SignedNumberToString(score);

        if (score == 0)
        {
            modifier = string.Empty;
        }

        return dicePart + modifier;
    }

    #endregion  
}

