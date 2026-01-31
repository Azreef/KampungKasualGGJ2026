using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class ColorVisionPlatform : MonoBehaviour
{
    TilemapRenderer tilemapRenderer;
    Collider2D tilemapCol;
    CompositeCollider2D compositeCol;

    SpriteMaskInteraction originalMask;

    [Header("Color Vision")]
    [SerializeField] private VisionColor platformColor;

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
    }

    void OnDisable()
    {
        ColorVisionController.OnColorVisionChanged -= OnColorChanged;
        RestoreOriginalState();
    }

    void OnColorChanged(VisionColor activeColor)
    {
        bool isActive = activeColor == platformColor;

        if (isActive)
            EnableCollision();
        else
            DisableCollision();

        // Always respect the mask
        tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
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
}
