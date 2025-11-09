using Newtonsoft.Json;
using UnityEngine;

public static class CharacterSaver
{
    private const string CharacterKey = "character";

    public static Character LoadPersistent()
    {
        if (GameManager.Instance.FreshCharacter)
        {
            return new Character();
        }

        if (PlayerPrefs.HasKey(CharacterKey))
        {
            string json = PlayerPrefs.GetString(CharacterKey);
            Character save = JsonConvert.DeserializeObject<Character>(json);

            if (save != null)
            {
                return save; 
            }
        }

        return new Character();
    }

    public static string SavePersistent(Character character)
    {
        string json = JsonConvert.SerializeObject(character);
        PlayerPrefs.SetString(CharacterKey, json);
        PlayerPrefs.Save();

        return json;
    }

    #region IO
    //private static string FilePath => Path.Combine(Application.persistentDataPath, "character.json");
    //private static string FilePathSave => Path.Combine(Application.persistentDataPath, "character.json");

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

    //public static string Save(Character character)
    //{
    //    string json = JsonUtility.ToJson(character, true);
    //    File.WriteAllText(FilePathSave, json);
    //    
    //    return json;
    //}
    #endregion
}