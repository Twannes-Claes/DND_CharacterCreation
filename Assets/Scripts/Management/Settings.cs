using UnityEngine;

[DefaultExecutionOrder(-150)]
public class Settings : MonoBehaviour
{
    #region Statics
    public static Settings Instance { get; private set; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Editor Fields

    public Color Green;
    public Color Red;

    public Sprite Checkmark;
    public Sprite Cross;

    public float DesktopStartZoom = 700;

    public Sprite SpellSaveSprite;
    public Sprite AttackBonusSprite;

    public int MaxSpellSlotsPerLevel = 4;
    #endregion
}
