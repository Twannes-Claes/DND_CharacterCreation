using System.Collections;
using UnityEngine;

using UnityEngine.Networking;

public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    private ApiCategory categoryToFetch;

    private void Start()
    {
        StartCoroutine(FetchCategory(categoryToFetch));
    }
    private IEnumerator FetchCategory(ApiCategory category)
    {
        string url = ApiHelper.GetURL(category);

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

                ApiResult result = JsonUtility.FromJson<ApiResult>(json);
                result.category = category;

                Debug.Log(json);
            }
        }
    }
}

