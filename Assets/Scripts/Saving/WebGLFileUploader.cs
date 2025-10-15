using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLFileUploader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void UploadFileJS(string gameObjectNamePtr, string callbackMethodPtr);

    public void UploadJson()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        UploadFileJS(gameObject.name, "OnFileUploaded");
        #else
        Debug.LogWarning("Upload not supported in Editor");
        #endif
    }

    public void OnFileUploaded(string json)
    {
        GameManager.Instance.LoadSheet(CharacterSaver.LoadJson(json));
    }
}
