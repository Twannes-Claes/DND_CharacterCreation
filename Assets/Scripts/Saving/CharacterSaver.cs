using UnityEngine;

public static class CharacterSaver
{
    private const string CharacterKey = "character";

    public static Character LoadPersistent()
    {
        if (GameManager.Instance.FreshCharacter)
        {
            return GetDefaultCharacter();
        }

        if (PlayerPrefs.HasKey(CharacterKey))
        {
            string json = PlayerPrefs.GetString(CharacterKey);
            Debug.Log($"Loading {json}");
            Character save = JsonUtility.FromJson<Character>(json);

            if (save != null)
            {
                return save; 
            }
        }

        return GetDefaultCharacter();
    }

    public static string SavePersistent(Character character)
    {
        string json = JsonUtility.ToJson(character, true);
        PlayerPrefs.SetString(CharacterKey, json);
        PlayerPrefs.Save();

        return json;
    }

    public static Character GetDefaultCharacter()
    {
        Character freshCharacter = new Character();
        freshCharacter.AsDefault();

        return freshCharacter;
    }
}