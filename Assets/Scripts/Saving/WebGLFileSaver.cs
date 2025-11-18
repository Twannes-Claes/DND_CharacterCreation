using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class WebGLFileSaver
{
    [DllImport("__Internal")]
    private static extern void SaveFileJS(string fileNamePtr, string jsonPtr);

    public static void SaveJson(string fileName, string json)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        SaveFileJS(fileName, json);
        #else
        SaveJsonToDisk(fileName, json);
        #endif
    }

    private static void SaveJsonToDisk(string fileName, string json)
    {
        #if UNITY_EDITOR
        string path = EditorUtility.SaveFilePanel
        (
            "Save JSON File",
            Application.dataPath,
            fileName,
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            Debug.Log($"Saved JSON to: {path}");
        }
        #endif
    }
}
