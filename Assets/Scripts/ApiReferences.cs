using System;
using Newtonsoft.Json;

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


[Serializable]
public class EquipmentReference
{
    public override string ToString()
    {
        return $"{name} Weight: {weight}, {equipment_category.name}";
    }

    public string index;
    public string name;

    public ApiReference equipment_category;

    public string weapon_category;
    public string weapon_range;
    public string category_range;

    public CostReference cost;
    public DamageReference damage;
    public RangeReference range;

    public DamageReference two_handed_damage;

    public double weight;
    public ApiReference[] properties;
    public string url;

    public ThrowRangeReference throw_range;

    public string[] special;
    public string image;

    public string armor_category;

    public ArmorClassReference armor_class;

    public int str_minimum;
    public bool stealth_disadvantage;

    public ApiReference gear_category;

    public string[] desc;
    public int quantity;

    public ContentReference[] contents;

    public string tool_category;
    public string vehicle_category;

    public SpeedReference speed;
    public string capacity;
}

[Serializable]
public class DamageReference
{
    public string damage_dice;
    public ApiReference damage_type;
}

[Serializable]
public class RangeReference
{
    [JsonProperty("normal")]
    public int normalRange;

    [JsonProperty("long")]
    public int longRange;
}

[Serializable]
public class ThrowRangeReference
{
    [JsonProperty("normal")]
    public int normalRange;

    [JsonProperty("long")]
    public int longRange;
}

[Serializable]
public class ArmorClassReference
{
    [JsonProperty("base")]
    public int base_armor;
    public bool dex_bonus;
    public int max_bonus;
}

[Serializable]
public class ContentReference
{
    public int quantity;

    public ApiReference item;
}

[Serializable]
public class SpeedReference
{
    public double quantity;
    public string unit;
}

[Serializable]
public class CostReference
{
    public int quantity;
    public string unit;
}
