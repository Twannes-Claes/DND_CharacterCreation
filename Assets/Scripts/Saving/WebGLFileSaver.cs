using UnityEngine;
using System.Runtime.InteropServices;

public static class WebGLFileSaver
{
    [DllImport("__Internal")]
    private static extern void SaveFileJS(string fileName, string json);

    public static void SaveJson(string fileName, string json)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveFileJS(fileName, json);
#else
        Debug.Log($"Would save JSON:\n{json}");
#endif
    }
}
