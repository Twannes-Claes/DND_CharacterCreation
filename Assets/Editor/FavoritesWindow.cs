using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class QuickAccessWindow : EditorWindow
{
    private readonly List<GameObject> prefabList = new();

    private readonly List<SceneAsset> sceneList = new();

    private readonly List<MonoScript> scriptList = new();

    private Vector2 scrollPos;

    [MenuItem("Twanny/Favorites Window")]
    public static void ShowWindow()
    {
        GetWindow<QuickAccessWindow>("Favorites");
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        DrawList(prefabList, "Prefabs");

        DrawList(sceneList, "Scenes");

        DrawList(scriptList, "Scripts");

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Prefab"))
        {
            prefabList.Add(null);
        }

        if (GUILayout.Button("Add Scene"))
        {
            sceneList.Add(null);
        }

        if (GUILayout.Button("Add Script"))
        {
            scriptList.Add(null);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        LoadData();
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void LoadData()
    {
        LoadList(prefabList, "Prefab");
        LoadList(sceneList, "Scene");
        LoadList(scriptList, "Script");
    }

    private void SaveData()
    {
        SaveList(prefabList, "Prefab");
        SaveList(sceneList, "Scene");
        SaveList(scriptList, "Script");
    }

    private void DrawList<T>(List<T> list, string label) where T : UnityEngine.Object
    {
        if (list.Count == 0) return;

        GUILayout.Label(label, EditorStyles.boldLabel);

        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Open or Edit the asset
            if (GUILayout.Button("Edit", GUILayout.Width(50)))
            {
                AssetDatabase.OpenAsset(list[i]);
            }

            // Draw object field to edit the object in the list
            list[i] = (T)EditorGUILayout.ObjectField(list[i], typeof(T), false);

            // Remove button
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                list.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void LoadList<T>(List<T> list, string pathRootName) where T : UnityEngine.Object
    {
        list.Clear();
        int count = EditorPrefs.GetInt(pathRootName + "Count", 0);
        for (int i = 0; i < count; i++)
        {
            string path = EditorPrefs.GetString(pathRootName + "Path" + i, "");
            if (!string.IsNullOrEmpty(path))
            {
                T prefab = AssetDatabase.LoadAssetAtPath<T>(path);
                if (prefab != null)
                {
                    list.Add(prefab);
                }
            }
        }
    }

    private void SaveList<T>(List<T> list, string pathRootName) where T : UnityEngine.Object
    {
        EditorPrefs.SetInt(pathRootName + "Count", list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            string path = AssetDatabase.GetAssetPath(list[i]);
            EditorPrefs.SetString(pathRootName + "Path" + i, path);
        }
    }
}
