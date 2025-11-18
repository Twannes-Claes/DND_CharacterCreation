using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WebGLFileUploader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void UploadFileJS(string gameObjectNamePtr, string callbackMethodPtr);

    public void UploadJson()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        UploadFileJS(gameObject.name, "OnFileUploaded");
        #else
        UploadJsonFromDisk();
        #endif
    }

    private void UploadJsonFromDisk()
    {
        #if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel
        (
            "Select JSON File",
            Application.dataPath,
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            OnFileUploaded(json);
        }
        else
        {
            Debug.Log("Upload cancelled");
        }
        #endif
    }

    public void OnFileUploaded(string json)
    {
        GameManager.Instance.LoadCharacter(json);
    }
}
