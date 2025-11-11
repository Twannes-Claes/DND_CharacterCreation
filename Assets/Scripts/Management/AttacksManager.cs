using System.Collections.Generic;
using UnityEngine;

public class AttacksManager : MonoBehaviour, ISaveable
{
    #region Editor Fields
    [SerializeField] private GameObject _attackFieldPrefab = null;
    #endregion

    #region Fields
    private List<AttackField> _attacks = new List<AttackField>();
    #endregion

    #region Statics
    public static AttacksManager Instance { get; private set; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.EditModeToggled += OnEditMode;
        SpellManager.OnDataChanged -= Load;
        SpellManager.OnDataChanged += Load;
        OnEditMode(GameManager.Instance.EditMode);
    }

    private void OnDisable()
    {
        GameManager.Instance.EditModeToggled -= OnEditMode;
    }
    #endregion

    #region Functions
    public void AddField(Attack attack, bool asFirst = false)
    {
        if (Instantiate(_attackFieldPrefab, this.transform).TryGetComponent(out AttackField attackField))
        {
            _attacks.Add(attackField);
            attackField.Initialize(attack, asFirst);
        }
    }

    public void AddField(Spell spell)
    {
        string bonus = spell.isSavingThrow ? GameManager.Instance.CharacterSheet.CharacterInfo[(int)GeneralInputType.SpellcastingSave] : GameManager.Instance.CharacterSheet.CharacterInfo[(int)GeneralInputType.SpellcastingAttackBonus];

        Attack attack = new Attack(spell.name, bonus, spell.info, true);

        AddField(attack);
    }

    public void RemoveField(AttackField field)
    {
        _attacks.Remove(field);

        Destroy(field.gameObject);
    }

    public void Load(Character sheet)
    {
        Debug.Log("Attacks load");

        AttackField.Manager = this;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _attacks.Clear();

        //Spawn first field as empty
        AddField(new Attack(), true);

        for (int i = 0; i < sheet.Attacks.Count; i++)
        {
            if (sheet.Attacks[i].isSpell)
                continue;

            AddField(sheet.Attacks[i]);
        }

        for (int i = 0; i < sheet.Spells.Count; i++)
        {
            if (sheet.Spells[i].isCantrip || sheet.Spells[i].isPrepared)
            {
                AddField(sheet.Spells[i]);
            }
        }
    }

    public void Load()
    {
        Debug.Log("YES LOADING CHANGE");
        Load(GameManager.Instance.CharacterSheet);
    }

    public void Save(Character sheet)
    {
        sheet.Attacks.Clear();
        
        for (int i = 1; i < _attacks.Count; i++)
        {
            AttackField field = _attacks[i];
            Attack attack = field.GetAttack();

            if (!string.IsNullOrWhiteSpace(field.name) || !attack.isSpell)
            {
                sheet.Attacks.Add(field.GetAttack());
            }
        }
    }

    private void OnEditMode(bool editMode)
    {
        if (_attacks.Count == 0)
            return;

        _attacks[0].gameObject.SetActive(editMode);
        _attacks[0].OnEditMode(editMode);
    }
    #endregion
}
