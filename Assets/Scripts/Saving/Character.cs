using System;
using System.Collections.Generic;

[Serializable]
public class Character
{
    public List<string> CharacterInfo = new List<string>(16)
    {
        "",                      //CharacterName = 0,
        "",                      //ClassLevel,
        "",                      //Background,
        "",                      //PlayerName,
        "",                      //Race,
        "",                      //Alignment,
        "0",                     //ExperiencePoints,
        "+2",                    //ProfiencyBonus,
        "11",                    //ArmorClass,
        "25",                    //Speed,
        "10",                    //MaxHitPoints,
        "10",                    //CurrentHitPoints,
        "1d8",                   //MaxHitDice,
        "1",                     //CurrentHitDice,
        "",                      //Personality,
        "",                      //Ideals,
        "",                      //Bonds,
        "",                      //Flaws,
        "  <b>Disciple Of life:</b>​ Your healing spells are particulary effective. Whenever you restore hit points to a creature with a spell of 1st level or higher, the creature regains additional hit points equal to 2+ the spell's level.​",                      //Feats,
        "<b>Proficiencies:</b>", //Proficiencies,
        "0",                     //TemporaryHitPoints
        "0",                     //Copper,
        "0",                     //Silver,
        "0",                     //Electrum,
        "0",                     //Gold,
        "0",                     //Platinum,
        "0",                     //Inspiration,
        "<b>Languages:</b>"      //Languages
    };

    public List<int> AbilityScores = new List<int> { 12, 14, 15, 8, 9, 10 };

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


