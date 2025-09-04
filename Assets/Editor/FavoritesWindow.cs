using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

public class QuickAccessWindow : EditorWindow
{
    [System.Serializable]
    private class QuickItem
    {
        public UnityEngine.Object asset;
    }

    private List<QuickItem> items = new();
    private ReorderableList reorderableList;

    [MenuItem("Twanny/Favorites Window")]
    public static void ShowWindow()
    {
        GetWindow<QuickAccessWindow>("Favorites");
    }

    private void OnEnable()
    {
        LoadData();
        SetupReorderableList();
    }

    private void SetupReorderableList()
    {
        reorderableList = new ReorderableList(items, typeof(QuickItem), true, true, true, true);

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Drag your favorites in here");
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = items[index];
            rect.y += 2;
            rect.height = EditorGUIUtility.singleLineHeight;

            float buttonWidth = 50;
            float removeWidth = 20;
            float fieldWidth = rect.width - buttonWidth - removeWidth - 10;

            // Open button on the left
            if (GUI.Button(new Rect(rect.x, rect.y, buttonWidth, rect.height), "Open"))
            {
                if (element.asset != null)
                    AssetDatabase.OpenAsset(element.asset);
            }

            // Object field next
            EditorGUI.BeginChangeCheck();
            element.asset = EditorGUI.ObjectField(new Rect(rect.x + buttonWidth + 5, rect.y, fieldWidth, rect.height), element.asset, typeof(UnityEngine.Object), false);
            if (EditorGUI.EndChangeCheck())
            {
                SaveData();
            }

            // Remove button on the right
            if (GUI.Button(new Rect(rect.x + buttonWidth + 5 + fieldWidth + 5, rect.y, removeWidth, rect.height), "X"))
            {
                items.RemoveAt(index);
                SaveData();
            }
        };

        reorderableList.onAddCallback = (ReorderableList list) =>
        {
            items.Add(new QuickItem());
            SaveData();
        };

        reorderableList.onReorderCallback = (ReorderableList list) =>
        {
            SaveData();
        };
    }

    private void OnGUI()
    {
        if (reorderableList != null)
            reorderableList.DoLayoutList();
    }

    private void SaveData()
    {
        EditorPrefs.SetInt("QuickAccessCount", items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            string path = items[i].asset != null ? AssetDatabase.GetAssetPath(items[i].asset) : "";
            EditorPrefs.SetString("QuickAccessPath" + i, path);
        }
    }

    private void LoadData()
    {
        items.Clear();
        int count = EditorPrefs.GetInt("QuickAccessCount", 0);

        for (int i = 0; i < count; i++)
        {
            string path = EditorPrefs.GetString("QuickAccessPath" + i, "");
            UnityEngine.Object asset = !string.IsNullOrEmpty(path) ? AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path) : null;
            items.Add(new QuickItem { asset = asset });
        }
    }
}
