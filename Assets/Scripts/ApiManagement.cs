using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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

public static class ApiHelper
{
    private static readonly string baseUrl = "https://www.dnd5eapi.co";
    private static readonly string apiUrl = "/api/2014/";
    public static string CategoryToUrl(ApiCategory category)
    {
        string enumName = category.ToString();

        // Insert dash before each capital letter (except first)
        //AbilityScores -> ability-scores
        string path = Regex.Replace(enumName, "(?<!^)([A-Z])", "-$1").ToLower();

        return baseUrl + apiUrl + path;
    }

    public static System.Collections.IEnumerator FetchDetail<T>(string url, Action<T> callback)
    {
        string apiUrl = baseUrl + url;

        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching category: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                T detail = JsonConvert.DeserializeObject<T>(json);
                callback?.Invoke(detail);
            }
        }
    }

    public static IEnumerator FetchCategory(ApiCategory category, Action<ApiResult> callback)
    {
        string url = CategoryToUrl(category);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching category: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;

                ApiResult result = JsonConvert.DeserializeObject<ApiResult>(json);
                result.category = category;

                callback?.Invoke(result);
            }
        }
    }
}
