using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ColorVisionLayer
{
    public string name;
    public LayerMask layer;
}

public enum VisionColor
{
    None = -1,
    Red = 0,
    Blue = 1,
    Green = 2
}

public class ColorVisionController : MonoBehaviour
{
    public static System.Action<VisionColor> OnColorVisionChanged;

    [Header("Always Visible Layers")]
    public LayerMask alwaysVisibleLayers;

    [Header("Color Vision Layers")]
    public List<ColorVisionLayer> colorLayers;

    public CursorScannerController scanner;

    int baseMask;
    int currentColorIndex = -1;

    [Header("Cursor Sprites")]
    public Sprite redCursorSprite;
    public Sprite blueCursorSprite;
    public Sprite greenCursorSprite;


    Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main ?? FindObjectOfType<Camera>();
        if (mainCamera == null)
            Debug.LogError("No Camera found in the scene!");

        if (scanner == null)
        {
            scanner = FindObjectOfType<CursorScannerController>();
            if (scanner == null)
                Debug.LogError("No CursorScannerController found in the scene!");
        }

        baseMask = alwaysVisibleLayers.value;

        mainCamera.cullingMask = baseMask;
        if (scanner != null)
            scanner.SetScanner(false);
    }

    void Update()   
    {
        if (Input.GetMouseButtonDown(0))
            HandleColorInput(0);

        if (Input.GetMouseButtonDown(1))
            HandleColorInput(1);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            HandleColorInput(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            HandleColorInput(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            HandleColorInput(2);
    }

    void HandleColorInput(int index)
    {
        if (index < 0 || index >= colorLayers.Count)
            return;

        if (currentColorIndex == index)
            return;

        currentColorIndex = index;

        // Update camera culling
        mainCamera.cullingMask = baseMask | colorLayers[index].layer.value;

        // Enable scanner
        scanner.SetScanner(true);

        // Change cursor sprite
        if (scanner.cursorVisual != null)
        {
            switch (index)
            {
                case 0: scanner.cursorVisual.sprite = redCursorSprite; break;
                case 1: scanner.cursorVisual.sprite = blueCursorSprite; break;
                case 2: scanner.cursorVisual.sprite = greenCursorSprite; break;
            }
        }

        // Notify any listeners
        OnColorVisionChanged?.Invoke((VisionColor)index);
    }

}
