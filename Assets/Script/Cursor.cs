using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);
        
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }
}
