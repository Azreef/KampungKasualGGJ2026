using UnityEngine;
using System;

public class CursorScannerController : MonoBehaviour
{
    [Header("Scanner")]
    public SpriteRenderer cursorVisual;
    public SpriteMask scannerMask;
    public Collider2D scannerTrigger;

    [Header("Player Check")]
    public LayerMask playerLayer;

    public bool isEnabled = true;
    bool playerInside = false;

    public bool IsEnabled => isEnabled;

    public static Action<bool> OnPlayerScannerStateChanged;

    void Awake()
    {
        scannerTrigger = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isEnabled) return;

        bool insideNow = scannerTrigger.OverlapPoint(
            GameObject.FindGameObjectWithTag("Player").transform.position
        );

        if (insideNow != playerInside)
        {
            playerInside = insideNow;
            OnPlayerScannerStateChanged?.Invoke(playerInside);
        }
    }

    public void SetScanner(bool enabled)
    {
        isEnabled = enabled;

        if (cursorVisual != null)
            cursorVisual.enabled = enabled;

        if (scannerMask != null)
            scannerMask.enabled = enabled;

        if (!enabled && playerInside)
        {
            playerInside = false;
            OnPlayerScannerStateChanged?.Invoke(false);
        }
    }
}

