using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private List<AbilityScoreInputField> _abilityScores;

    [SerializeField] private List<CanvasGroup> _canvasGroups;
    #endregion

    #region Fields
    private readonly Dictionary<AbilityScores, AbilityScoreInputField> _abilityScoreDict = new Dictionary<AbilityScores, AbilityScoreInputField>();

    private List<ISaveable> _saveables = null;
    #endregion

    #region Properties
    public Character CharacterSheet { get; set; } = null;
    public bool EditMode { get; set; } = false;

    public bool FreshCharacter = true;
    #endregion

    #region Events

    public Action<bool> EditModeToggled;

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
        _saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToList();
        LoadCharacter(CharacterSaver.LoadPersistent());
    }
    #endregion

    #region Functions
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F2))
        {
            ToggleEditMode();
        }
    }

    public void ToggleEditMode()
    {
        EditMode = !EditMode;

        EditModeToggled?.Invoke(EditMode);

        SaveCharacter();

        foreach (CanvasGroup group in _canvasGroups)
        {
            group.interactable = EditMode;
        }
    }

    public void SaveCharacter()
    {
        foreach (ISaveable saveable in _saveables)
        {
            saveable.Save(CharacterSheet);
        }

        CharacterSaver.SavePersistent(CharacterSheet);
    }

    public void BackUpCharacter()
    {
        foreach (ISaveable saveable in _saveables)
        {
            saveable.Save(CharacterSheet);
        }

        WebGLFileSaver.SaveJson("CharacterSheet.json", CharacterSaver.SavePersistent(CharacterSheet));
    }

    public void LoadCharacter(string json)
    {
        LoadCharacter(JsonUtility.FromJson<Character>(json));
    }

    public void LoadCharacter(Character save)
    {
        if (save == null)
            return;

        CharacterSheet = save;

        foreach (ISaveable saveable in _saveables)
        {
            saveable.Load(CharacterSheet);
        }
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