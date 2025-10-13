using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.IO;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private List<AbilityScoreInputField> _abilityScores;
    #endregion

    #region Fields
    private readonly Dictionary<AbilityScores, AbilityScoreInputField> _abilityScoreDict = new Dictionary<AbilityScores, AbilityScoreInputField>();
    #endregion

    #region Properties
    public Character CharacterSheet { get; set; } = null;

    public bool StopScrolling { get; set; } = false;
    public bool StopPanning { get; set; } = false;
    #endregion 

    #region Statics
    public static GameManager Instance { get; private set; }
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
                _abilityScoreDict[field.AbilityScore] = field;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "character.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CharacterSheet = JsonUtility.FromJson<Character>(json);
            Debug.Log("Character loaded successfully!");
        }
        else
        {
            CharacterSheet = new Character();
            Debug.Log("No saved character found.");
        }

        List<ISaveable> saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToList();

        foreach (var save in saveables)
        {
            save.Load(CharacterSheet);
        }
    }

    private void OnApplicationQuit()
    {
        var filePath = Path.Combine(Application.persistentDataPath, "character.json");
        string json = JsonUtility.ToJson(CharacterSheet, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"Character saved to: {filePath}");
        Debug.Log(JsonUtility.ToJson(CharacterSheet, true));
    }
    #endregion

    #region Functions
    public AbilityScoreInputField GetAbilityScore(AbilityScores abilityScore)
    {
        if (_abilityScoreDict.ContainsKey(abilityScore) == false)
            return null;

        return _abilityScoreDict[abilityScore];
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

    //2d8+3, modifier changed to +4 for example => 2d8+4
    public static string UpdateDiceModifier(int score, string diceText)
    {
        diceText = diceText.Replace(" ", "");

        var match = Regex.Match(diceText, @"^([0-9]*d[0-9]+)([+-]\d+)?$");

        if (!match.Success)
            return string.Empty; ;

        string dicePart = match.Groups[1].Value;
        string modifier = GameManager.SignedNumberToString(score);

        if (score == 0)
        {
            modifier = string.Empty;
        }

        return dicePart + modifier;
    }

    #endregion  
}

