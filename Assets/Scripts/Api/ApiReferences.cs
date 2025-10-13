using Newtonsoft.Json;
using System;

public enum ApiCategoryType
{
    Proficiencies = 0,
    Languages = 1,
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
    MagicItems,
    MagicSchools,
    Monsters,
    Races,
    RuleSections,
    Rules,
    Skills,
    Spells,
    Subclasses,
    Subraces,
    Traits,
    WeaponProperties,
    None,
}

[Serializable]
public class ApiCategoryResource
{
    public ApiCategoryType category;
    public int count;
    public ApiResource[] results;
}

[Serializable]
public class ApiResource
{
    public string index;
    public string name;
    public string url;
}

#region EquipmentResources
[Serializable]
public class EquipmentResource
{
    public override string ToString()
    {
        return $"{name} Weight: {weight}, {equipment_category.name}";
    }

    public string index;
    public string name;

    public ApiResource equipment_category;

    public string weapon_category;
    public string weapon_range;
    public string category_range;

    public CostResource cost;
    public DamageResource damage;

    [JsonProperty("range")]
    public RangeResource range;

    public DamageResource two_handed_damage;

    public double weight;
    public ApiResource[] properties;
    public string url;

    public ThrowRangeResource throw_range;

    public string[] special;
    public string image;

    public string armor_category;

    public ArmorClassResource armor_class;

    public int str_minimum;
    public bool stealth_disadvantage;

    public ApiResource gear_category;

    public string[] desc;
    public int quantity;

    public ContentResource[] contents;

    public string tool_category;
    public string vehicle_category;

    public SpeedResource speed;
    public string capacity;
}

[Serializable]
public class DamageResource
{
    public string damage_dice;
    public ApiResource damage_type;
}

[Serializable]
public class RangeResource
{
    [JsonProperty("normal")]
    public int normalRange;

    [JsonProperty("long")]
    public int longRange;
}

[Serializable]
public class ThrowRangeResource
{
    [JsonProperty("normal")]
    public int normalRange;

    [JsonProperty("long")]
    public int longRange;
}

[Serializable]
public class ArmorClassResource
{
    [JsonProperty("base")]
    public int base_armor;
    public bool dex_bonus;
    public int max_bonus;
}

[Serializable]
public class ContentResource
{
    public int quantity;

    public ApiResource item;
}

[Serializable]
public class SpeedResource
{
    public double quantity;
    public string unit;
}

[Serializable]
public class CostResource
{
    public int quantity;
    public string unit;
}
#endregion