using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        CharacterSheet = CharacterSaver.Load();

        foreach (ISaveable saveable in FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToList())
        {
            saveable.Load(CharacterSheet);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            string json = CharacterSaver.Save(CharacterSheet);

            WebGLFileSaver.SaveJson("CharacterSheet.json", json);
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log(CharacterSaver.Save(CharacterSheet));
    }
    #endregion

    #region Functions
    public void Save()
    {
        CharacterSaver.Save(CharacterSheet);
    }

    public string GetCharacterInfo(GeneralInputType type)
    {
        return CharacterSheet.CharacterInfo[(int)type];
    }

    public void RefreshAbilityScores()
    {
        foreach(AbilityScoreInputField field in _abilityScoreDict.Values)
        {
            field.OnAbilityScoreChanged?.Invoke(field.AbilityModifier);
        }
    }

    public AbilityScoreInputField GetAbilityScore(AbilityScores abilityScore)
    {
        _abilityScoreDict.TryGetValue(abilityScore, out var field);
        return field;
    }
    #endregion  
}