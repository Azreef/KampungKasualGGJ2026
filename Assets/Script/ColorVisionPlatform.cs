using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D  ))]
public class ColorVisionPlatform : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;
    Collider2D tilemapCol;
    CompositeCollider2D compositeCol;

    SpriteMaskInteraction originalMask;

    [Header("Color Vision")]
    [SerializeField] private VisionColor platformColor;

    bool correctColorActive = false;
    bool playerInsideScanner = false;


    void Awake()
    {

        tilemapRenderer = GetComponent<TilemapRenderer>();

        tilemapCol = GetComponent<TilemapCollider2D>();
        compositeCol = GetComponent<CompositeCollider2D>();

        originalMask = tilemapRenderer.maskInteraction;

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

        // Always respect mask
        tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        UpdateCollision();
    }



    void EnableCollision()
    {
        if (tilemapCol != null)
            tilemapCol.enabled = true;

        if (compositeCol != null)
            compositeCol.enabled = true;
    }

    void DisableCollision()
    {
        if (tilemapCol != null)
            tilemapCol.enabled = false;

        if (compositeCol != null)
            compositeCol.enabled = false;
    }

    void RestoreOriginalState()
    {
        if (tilemapRenderer != null)
            tilemapRenderer.maskInteraction = originalMask;
    }

    void OnScannerStateChanged(bool inside)
    {
        playerInsideScanner = inside;
        UpdateCollision();
    }

    void UpdateCollision()
    {
        if (correctColorActive && playerInsideScanner)
            EnableCollision();
        else
            DisableCollision();
    }

}
