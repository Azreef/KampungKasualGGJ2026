using UnityEngine;

public class CursorScannerController : MonoBehaviour
{
    [Header("Cursor Parts")]
    public SpriteRenderer cursorVisual;
    public SpriteMask scannerMask;

    bool isEnabled = true;

    public bool IsEnabled => isEnabled;

    private void Start()
    {
        scannerMask = GetComponentInChildren<SpriteMask>();
        cursorVisual = GetComponent<SpriteRenderer>();
    }

    public void SetScanner(bool enabled)
    {
        isEnabled = enabled;

        if (cursorVisual != null)
            cursorVisual.enabled = enabled;

        if (scannerMask != null)
            scannerMask.enabled = enabled;
    }

    public void ToggleScanner()
    {
        SetScanner(!isEnabled);
    }
}
