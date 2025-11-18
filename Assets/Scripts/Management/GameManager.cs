using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Newtonsoft.Json;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private List<AbilityScoreInputField> _abilityScores;

    [SerializeField] private List<GameObject> _pages;
    [SerializeField] private List<CanvasGroup> _canvasGroups;
    #endregion

    #region Fields
    private readonly Dictionary<AbilityScores, AbilityScoreInputField> _abilityScoreDict = new Dictionary<AbilityScores, AbilityScoreInputField>();

    private List<ISaveable> _saveables = null;

    private int _activePage = 0;
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

            foreach (CanvasGroup group in _canvasGroups)
            {
                group.interactable = false;
            }

            foreach (GameObject page in _pages)
            {
                page.SetActive(false);
            }

            _pages[0].SetActive(true);
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

    private void MovePage(int direction)
    {
        _pages[_activePage].SetActive(false);
        _activePage = (_activePage + direction + _pages.Count) % _pages.Count;
        _pages[_activePage].SetActive(true);
    }

    public void MoveLeft()
    {
        MovePage(-1);
    }

    public void MoveRight()
    {
        MovePage(1);
    }

    public void SaveCharacter()
    {
        foreach (ISaveable saveable in _saveables)
        {
            saveable.Save(CharacterSheet);
        }

        Debug.Log($"Saving JSON:\n {CharacterSaver.SavePersistent(CharacterSheet)}");
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
        LoadCharacter(JsonConvert.DeserializeObject<Character>(json));
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