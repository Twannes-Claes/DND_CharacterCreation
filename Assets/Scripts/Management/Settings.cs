using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion
}
