using System;

public enum ApiCategory
{
    AbilityScores,
    Alignments,
    Backgrounds,
    Classes,
    Conditions,
    DamageTypes,
    Equipment,
    EquipmentCategories,
    Feats,
    Features,
    Languages,
    MagicItems,
    MagicSchools,
    Monsters,
    Proficiencies,
    Races,
    RuleSections,
    Rules,
    Skills,
    Spells,
    Subclasses,
    Subraces,
    Traits,
    WeaponProperties
}

[Serializable] 
public class ApiResult 
{
    public ApiCategory category;
    public int count; 
    public ApiReference[] results; 
}

[Serializable] 
public class ApiReference 
{ 
    public string index; 
    public string name; 
    public string url; 
}

public static class ApiHelper
{
    private static readonly string baseUrl = "https://www.dnd5eapi.co/api/2014/";
    public static string GetURL(ApiCategory category)
    {
        return category switch
        {
            ApiCategory.Alignments => baseUrl + "alignments",
            ApiCategory.AbilityScores => baseUrl + "ability-scores",
            ApiCategory.Backgrounds => baseUrl + "backgrounds",
            ApiCategory.Classes => baseUrl + "classes",
            ApiCategory.Conditions => baseUrl + "conditions",
            ApiCategory.DamageTypes => baseUrl + "damage-types",
            ApiCategory.Equipment => baseUrl + "equipment",
            ApiCategory.EquipmentCategories => baseUrl + "equipment-categories",
            ApiCategory.Feats => baseUrl + "feats",
            ApiCategory.Features => baseUrl + "features",
            ApiCategory.Languages => baseUrl + "languages",
            ApiCategory.MagicItems => baseUrl + "magic-items",
            ApiCategory.MagicSchools => baseUrl + "magic-schools",
            ApiCategory.Monsters => baseUrl + "monsters",
            ApiCategory.Proficiencies => baseUrl + "proficiencies",
            ApiCategory.Races => baseUrl + "races",
            ApiCategory.RuleSections => baseUrl + "rule-sections",
            ApiCategory.Rules => baseUrl + "rules",
            ApiCategory.Skills => baseUrl + "skills",
            ApiCategory.Spells => baseUrl + "spells",
            ApiCategory.Subclasses => baseUrl + "subclasses",
            ApiCategory.Subraces => baseUrl + "subraces",
            ApiCategory.Traits => baseUrl + "traits",
            ApiCategory.WeaponProperties => baseUrl + "weapon-properties",
            _ => baseUrl
        };
    }
}
