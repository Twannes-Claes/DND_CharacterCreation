using System.IO;
using UnityEngine;

public static class CharacterSaver
{
    private static string FilePath => Path.Combine(Application.persistentDataPath, "character1.json");
    private static string FilePathSave => Path.Combine(Application.persistentDataPath, "character.json");

    public static Character Load()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<Character>(json);
        }
        return new Character();
    }

    public static string Save(Character character)
    {
        string json = JsonUtility.ToJson(character, true);
        File.WriteAllText(FilePathSave, json);

        return json;
    }
}
