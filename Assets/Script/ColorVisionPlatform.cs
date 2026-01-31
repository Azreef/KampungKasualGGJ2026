using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorVisionPlatform : MonoBehaviour
{
    SpriteRenderer sr;
    SpriteMaskInteraction originalMask;

    [SerializeField] private bool isAHidden = false;

    void Awake()
    {
        //sr = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
        originalMask = sr.maskInteraction;

        if (isAHidden)
        {
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            sr.maskInteraction = SpriteMaskInteraction.None;
        }
    }

    void OnDestroy()
    {
        RestoreOriginalState();
    }

#if UNITY_EDITOR
    void OnDisable()
    {
        // Extra safety for editor state changes
        if (!Application.isPlaying)
            RestoreOriginalState();
    }
#endif

    void RestoreOriginalState()
    {
        if (sr != null)
            sr.maskInteraction = originalMask;
    }
}
