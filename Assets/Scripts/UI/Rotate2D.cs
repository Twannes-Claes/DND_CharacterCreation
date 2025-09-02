using UnityEngine;

public class Rotate2D : MonoBehaviour
{
    #region Editor Fields
    [SerializeField]
    private float rotationSpeed = 90f;
    #endregion

    #region GameLoop
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
    #endregion
}
