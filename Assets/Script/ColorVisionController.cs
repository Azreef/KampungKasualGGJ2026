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
        {
            bool wasEnabled = scanner.IsEnabled;
            scanner.SetScanner(!wasEnabled);

            if (!scanner.IsEnabled)
            {
                currentColorIndex = -1;
                mainCamera.cullingMask = baseMask;
                OnColorVisionChanged?.Invoke(VisionColor.None);
            }
            else
            {
                mainCamera.cullingMask =
                    baseMask | colorLayers[index].layer.value;
                OnColorVisionChanged?.Invoke((VisionColor)index);
            }

            return;
        }

        // Switch to new color
        currentColorIndex = index;

        mainCamera.cullingMask =
            baseMask | colorLayers[index].layer.value;

        scanner.SetScanner(true);
        OnColorVisionChanged?.Invoke((VisionColor)index);
    }

}
