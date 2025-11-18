using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EditModeButton : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private Image _colorImage;
    #endregion

    #region Fields
    private Button _button = null;

    #endregion

    #region LifeCycle
    private void OnEnable()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(OnClick);

        _colorImage.color = GameManager.Instance.EditMode ? Settings.Instance.Green : Settings.Instance.Red;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    #endregion

    #region Functions
    private void OnClick()
    {
        GameManager.Instance.ToggleEditMode();

        _colorImage.color = GameManager.Instance.EditMode ? Settings.Instance.Green : Settings.Instance.Red;
    }
    #endregion
}