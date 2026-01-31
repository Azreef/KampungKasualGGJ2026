using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorVisionPlatform : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;
    SpriteRenderer spriteRenderer;

    Collider2D[] colliders;

    SpriteMaskInteraction originalTilemapMask;
    SpriteMaskInteraction originalSpriteMask;

    [Header("Color Vision")]
    [SerializeField] private VisionColor platformColor;

    bool correctColorActive = false;
    bool playerInsideScanner = false;

    void Awake()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        colliders = GetComponents<Collider2D>();

        if (tilemapRenderer != null)
            originalTilemapMask = tilemapRenderer.maskInteraction;

        if (spriteRenderer != null)
            originalSpriteMask = spriteRenderer.maskInteraction;

        DisableCollision();
    }

    void OnEnable()
    {
        ColorVisionController.OnColorVisionChanged += OnColorChanged;
        CursorScannerController.OnPlayerScannerStateChanged += OnScannerStateChanged;
    }

    void OnDisable()
    {
        ColorVisionController.OnColorVisionChanged -= OnColorChanged;
        CursorScannerController.OnPlayerScannerStateChanged -= OnScannerStateChanged;
        RestoreOriginalState();
    }

    void OnColorChanged(VisionColor activeColor)
    {
        correctColorActive = (activeColor == platformColor);

        if (tilemapRenderer != null)
            tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        if (spriteRenderer != null)
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        UpdateCollision();
    }

    void OnScannerStateChanged(bool inside)
    {
        playerInsideScanner = inside;
        UpdateCollision();
    }

    void UpdateCollision()
    {
        bool shouldEnable = correctColorActive && playerInsideScanner;

        foreach (var col in colliders)
        {
            if (!shouldEnable && col.enabled)
                StartCoroutine(DisableNextFrame(col));
            else
                col.enabled = shouldEnable;
        }
    }

    IEnumerator DisableNextFrame(Collider2D col)
    {
        yield return new WaitForFixedUpdate();
        col.enabled = false;
    }


    void DisableCollision()
    {
        foreach (var col in colliders)
            col.enabled = false;
    }

    void RestoreOriginalState()
    {
        if (tilemapRenderer != null)
            tilemapRenderer.maskInteraction = originalTilemapMask;

        if (spriteRenderer != null)
            spriteRenderer.maskInteraction = originalSpriteMask;
    }
}
