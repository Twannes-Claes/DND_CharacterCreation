using System;
using System.Collections.Generic;

[Serializable]
public class Character
{
    public void AsDefault()
    {
        CharacterInfo = new List<string>(16)
        {
        /*CharacterName*/           "",
        /*ClassLevel*/              "",
        /*Background*/              "",
        /*PlayerName*/              "",
        /*Race*/                    "",
        /*Alignment*/               "", 
        /*ExperiencePoints*/        "0",   
        /*ProfiencyBonus*/          "+2",
        /*ArmorClass*/              "10",
        /*Speed*/                   "25",
        /*MaxHitPoints*/            "10",
        /*CurrentHitPoints*/        "10",
        /*MaxHitDice*/              "1d8", 
        /*CurrentHitDice*/          "1",         
        /*Personality*/             "",         
        /*Ideals*/                  "",
        /*Bonds*/                   "",
        /*Flaws*/                   "",
        /*Feats*/                   "",
        /*Proficiencies*/           "<b>Proficiencies:</b>",
        /*TemporaryHitPoints*/      "0",
        /*Copper*/                  "0",
        /*Silver*/                  "0",
        /*Electrum*/                "0",
        /*Gold*/                    "0",
        /*Platinum*/                "0",
        /*Inspiration*/             "0",
        /*Languages*/               "<b>Languages:</b> Common",
        /*SpellcastingClass*/       "",
        /*SpellcastingAbility*/     "-",
        /*SpellcastingSave*/        "DC 0",
        /*SpellcastingAttackBonus*/ "+0",
        /*SpellsPrepared*/          "0"
        };

        AbilityScores = new List<int> { 10, 10, 10, 10, 10, 10 };

        SkillProficiencies = new List<Proficiency>();

        Attacks = new List<Attack>();

        Equipments = new List<Equipment>();

        Spells = new List<Spell>();

        SpellSlots = new List<SpellSlot>
        {
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0),
            new SpellSlot(0, 0)
        };
    }

    public List<string> CharacterInfo;
    public List<int> AbilityScores;
    public List<Proficiency> SkillProficiencies;
    public List<Attack> Attacks;
    public List<Equipment> Equipments;
    public List<Spell> Spells;
    public List<SpellSlot> SpellSlots;
}

[Serializable]
public struct Proficiency
{
    public int skill;
    public bool expertised;

    public Proficiency(int skill, bool expertised)
    {
        this.skill = skill;
        this.expertised = expertised;
    }
}

[Serializable]
public struct Attack
{
    public string name;
    public string bonus;
    public string damage;

    public bool isSpell;

    public Attack(string name, string bonus, string damage, bool isSpell)
    {
        this.name = name;
        this.bonus = bonus;
        this.damage = damage;
        this.isSpell = isSpell;
    }
}

[Serializable]
public struct Equipment
{
    public string name;
    public int amount;

    public Equipment(string name, int amount)
    {
        this.name = name;
        this.amount = amount;
    }
}

[Serializable]
public struct Spell
{
    public int index;
    public string name;
    public string info;

    public bool isPrepared;
    public bool isSavingThrow;
    public bool isCantrip;

    public Spell(int index, string name, string info, bool isPrepared, bool isSavingThrow, bool isCantrip)
    {
        this.index = index;
        this.name = name;
        this.info = info;
        this.isPrepared = isPrepared;
        this.isSavingThrow = isSavingThrow;
        this.isCantrip = isCantrip;
    }
}

[Serializable]
public struct SpellSlot
{
    public int total;
    public int used;

    public SpellSlot(int total, int used)
    {
        this.total = total;
        this.used = used;
    }
}