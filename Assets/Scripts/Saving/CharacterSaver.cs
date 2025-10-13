using System.IO;
using UnityEngine;

public static class CharacterSaver
{
    //private static string FilePath => Path.Combine(Application.persistentDataPath, "character.json");
    //private static string FilePathSave => Path.Combine(Application.persistentDataPath, "character.json");

    private const string CharacterKey = "character";

    //public static Character Load()
    //{
    //    if (File.Exists(FilePath))
    //    {
    //        string json = File.ReadAllText(FilePath);
    //        return JsonUtility.FromJson<Character>(json);
    //    }
    //
    //    return new Character();
    //}

    public static Character Load()
    {
        if (PlayerPrefs.HasKey(CharacterKey))
        {
            string json = PlayerPrefs.GetString(CharacterKey);
            return JsonUtility.FromJson<Character>(json);
        }

        return new Character();
    }

    //public static string Save(Character character)
    //{
    //    string json = JsonUtility.ToJson(character, true);
    //    File.WriteAllText(FilePathSave, json);
    //    
    //    return json;
    //}

    public static string Save(Character character)
    {
        string json = JsonUtility.ToJson(character, true);
        PlayerPrefs.SetString(CharacterKey, json);
        PlayerPrefs.Save();

        return json;
    }
}