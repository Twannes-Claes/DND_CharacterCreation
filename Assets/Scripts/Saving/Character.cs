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
        "10",                    //ArmorClass,
        "25",                    //Speed,
        "10",                    //MaxHitPoints,
        "10",                    //CurrentHitPoints,
        "1d8",                   //MaxHitDice,
        "1",                     //CurrentHitDice,
        "",                      //Personality,
        "",                      //Ideals,
        "",                      //Bonds,
        "",                      //Flaws,
        "",                      //Feats,
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

    //public int Inspiration = 0;
    //public int ProfiencyBonus = 0;
    //public int ArmorClass = 0;
    //public int Speed = 0;
    //
    //public int CurrentHitPoints = 0;
    //public int MaxHitPoints = 0;
    //public int TemporaryHitPoints = 0;
    //
    //public string TotalHitDice = "1d8";
    //public int CurrentHitDice = 0;

    public List<int> AbilityScores = new List<int> { 12, 14, 15, 8, 9, 10 };
}
