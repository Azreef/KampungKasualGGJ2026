using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    internal static CursorLockMode lockState;
    private Camera mainCamera;

    public static bool visible { get; internal set; }

    void Start()
    {
        mainCamera = Camera.main;
    }
        
    void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);
        
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }
}
