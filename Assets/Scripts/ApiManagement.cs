using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

public enum ApiCategoryType
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

public enum ApiCategoryTypeUsable
{
    Equipment,
    Spells
}

public static class ApiHelper
{
    private static readonly string baseURL = "https://www.dnd5eapi.co";
    private static readonly string apiURL = "/api/2014/";

    public static async Task<T> FetchDetailAsync<T>(string url)
    {
        string apiUrl = baseURL + url;
        string json = await FetchJsonAsync(apiUrl);

        return JsonConvert.DeserializeObject<T>(json);
    }

    public static async Task<ApiCategoryResource> FetchCategoryAsync(ApiCategoryType category)
    {
        string url = CategoryToUrl(category);
        string json = await FetchJsonAsync(url);
    
        ApiCategoryResource result = JsonConvert.DeserializeObject<ApiCategoryResource>(json);
        result.category = category;

        return result;
    }

    private static Task<string> FetchJsonAsync(string url)
    {
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Accept", "application/json");

        var operation = request.SendWebRequest();

        operation.completed += _ =>
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                tcs.SetException(new Exception("Error fetching category: " + request.error));
            }
            else
            {
                tcs.SetResult(request.downloadHandler.text);
            }

            request.Dispose();
        };

        return tcs.Task;
    }

    private static string CategoryToUrl(ApiCategoryType category)
    {
        string enumName = category.ToString();

        // Insert dash before each capital letter (except first)
        //AbilityScores -> ability-scores
        string path = Regex.Replace(enumName, "(?<!^)([A-Z])", "-$1").ToLower();

        return baseURL + apiURL + path;
    }

    #region PrintUniqueAPI_Keys
    //private void PrintApiUniqueKeys()
    //{
    //    string jsonText = jsonFile.text;
    //    var keys = new HashSet<string>();
    //
    //    // Check if JSON starts with [ or {
    //    jsonText = jsonText.Trim();
    //    if (jsonText.StartsWith("["))
    //    {
    //        JArray array = JArray.Parse(jsonText);
    //        foreach (var item in array)
    //            ExtractKeys(item, keys);
    //    }
    //    else
    //    {
    //        JObject obj = JObject.Parse(jsonText);
    //        ExtractKeys(obj, keys);
    //    }
    //
    //    // Join all keys into a single string
    //    string allKeys = string.Join(", ", keys);
    //    Debug.Log("Unique keys: " + allKeys);
    //}
    //
    //private void ExtractKeys(JToken token, HashSet<string> keys, string prefix = "")
    //{
    //    if (token is JObject jObject)
    //    {
    //        foreach (var prop in jObject.Properties())
    //        {
    //            string keyName = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
    //            keys.Add(keyName);
    //            ExtractKeys(prop.Value, keys, keyName);
    //        }
    //    }
    //    else if (token is JArray jArray)
    //    {
    //        foreach (var item in jArray)
    //        {
    //            ExtractKeys(item, keys, prefix);
    //        }
    //    }
    //}
    #endregion
}
