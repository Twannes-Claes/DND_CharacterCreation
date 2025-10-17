using System;
using System.Collections.Generic;

[Serializable]
public class Character
{
    public List<string> CharacterInfo = new List<string>(16)
    {
        /*CharacterName*/           "",
        /*ClassLevel*/              "",
        /*Background*/              "",
        /*PlayerName*/              "",
        /*Race*/                    "",
        /*Alignment*/               "", 
        /*ExperiencePoints*/        "0",   
        /*ProfiencyBonus*/          "+2",
        /*ArmorClass*/              "11",
        /*Speed*/                   "25",
        /*MaxHitPoints*/            "10",
        /*CurrentHitPoints*/        "10",
        /*MaxHitDice*/              "1d8", 
        /*CurrentHitDice*/          "1",         
        /*Personality*/             "",         
        /*Ideals*/                  "",
        /*Bonds*/                   "",
        /*Flaws*/                   "",
        /*Feats*/                   "  <b>Disciple Of life:</b>​ Your healing spells are particulary effective. Whenever you restore hit points to a creature with a spell of 1st level or higher, the creature regains additional hit points equal to 2+ the spell's level.​",
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

    public List<int> AbilityScores = new List<int> { 10, 10, 10, 10, 10, 10 };

    public List<Proficiency> SkillProficiencies = new List<Proficiency>
    {
        new Proficiency(1, false),
        new Proficiency(4, false),
        new Proficiency(10, true)
    };

    public List<Equipment> Equipments = new List<Equipment>
    {
        new Equipment("Greatsword", 1),
        new Equipment("Arrows", 25),
        new Equipment("Flask of oil", 2)
    };

    public List<Spell> PreparedSpells = new List<Spell>();
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
    public bool isSavingThrow;

    public Spell(int index, string name, bool isSavingThrow)
    {
        this.index = index;
        this.name = name;
        this.isSavingThrow = isSavingThrow;
    }
}


