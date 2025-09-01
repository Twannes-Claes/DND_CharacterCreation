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
public class CommonDetail
{
    public string index;
    public string name;
    public string url;
}


[Serializable]
public class EquipmentDetail
{
    public override string ToString()
    {
        return $"{name} Weight: {weight}, {equipment_category.name}";
    }

    public string index;
    public string name;

    public CommonDetail equipment_category;

    public string weapon_category;
    public string weapon_range;
    public string category_range;

    public CostDetail cost;
    public DamageDetail damage;
    public RangeDetail range;

    public DamageDetail two_handed_damage;

    public double weight;
    public CommonDetail[] properties;
    public string url;

    public ThrowRangeDetail throw_range;

    public string[] special;
    public string image;

    public string armor_category;

    public ArmorClass armor_class;

    public int str_minimum;
    public bool stealth_disadvantage;

    public CommonDetail gear_category;

    public string[] desc;
    public int quantity;

    public ContentDetail[] contents;

    public string tool_category;
    public string vehicle_category;

    public SpeedDetail speed;
    public string capacity;
}

[Serializable]
public class DamageDetail
{
    public string damage_dice;
    public CommonDetail damage_type;
}

[Serializable]
public class RangeDetail
{
    [JsonProperty("normal")]
    public int normalRange;

    [JsonProperty("long")]
    public int longRange;
}

[Serializable]
public class ThrowRangeDetail
{
    [JsonProperty("normal")]
    public int normalRange;

    [JsonProperty("long")]
    public int longRange;
}

[Serializable]
public class ArmorClass
{
    [JsonProperty("base")]
    public int base_armor;
    public bool dex_bonus;
    public int max_bonus;
}

[Serializable]
public class ContentDetail
{
    public int quantity;

    public CommonDetail item;
}

[Serializable]
public class SpeedDetail
{
    public double quantity;
    public string unit;
}

[Serializable]
public class CostDetail
{
    public int quantity;
    public string unit;
}
