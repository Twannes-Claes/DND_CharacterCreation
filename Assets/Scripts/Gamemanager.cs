using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq; // Make sure you have Newtonsoft.Json installed

public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    //private ApiCategory categoryToFetch;

    public TextAsset jsonFile; // Drag your .txt JSON file here in the Inspector
    private void Start()
    {
        StartCoroutine(ApiHelper.FetchCategory(ApiCategory.Equipment, (result) =>
        {
            if (result != null && result.results != null)
            {
                Debug.Log($"Fetched {result.count} items from {result.category}");

                StartCoroutine(FetchAllDetails(result));
            }
        }));
    }

    private System.Collections.IEnumerator FetchAllDetails(ApiResult result)
    {
        foreach (var item in result.results)
        {
            //Debug.Log($"{item.index} ({item.url})");

            EquipmentDetail detail = null;

            System.Collections.IEnumerator fetchCoroutine = ApiHelper.FetchDetail<EquipmentDetail>(item.url, d => detail = d);
            yield return fetchCoroutine;

            if (detail != null)
            {
                //Debug.Log($"Detail for {detail}");
            }
        }

        Debug.Log("done");
    }

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
}

