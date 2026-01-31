using UnityEngine;
using System;

public class CursorScannerController : MonoBehaviour
{
    [Header("Cursor Parts")]
    public SpriteRenderer cursorVisual;
    public SpriteMask scannerMask;

    bool isEnabled = true;
    bool playerInside = false;

    public bool IsEnabled => isEnabled;
    public bool IsPlayerInside => playerInside;

    public static Action<bool> OnPlayerScannerStateChanged;

    private void Start()
    {
        scannerMask = GetComponentInChildren<SpriteMask>();
        cursorVisual = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEnabled) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            OnPlayerScannerStateChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            OnPlayerScannerStateChanged?.Invoke(false);
        }
    }

    public void SetScanner(bool enabled)
    {
        isEnabled = enabled;

        if (cursorVisual != null)
            cursorVisual.enabled = enabled;

        if (scannerMask != null)
            scannerMask.enabled = enabled;

        if (!enabled)
        {
            playerInside = false;
            OnPlayerScannerStateChanged?.Invoke(false);
        }
    }
}
