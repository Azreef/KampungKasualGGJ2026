using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CircleCollider2D))]
public class VisionGroundProjector : MonoBehaviour
{
    public LayerMask platformLayer;

    private CircleCollider2D visionCollider;

    void Awake()
    {
        visionCollider = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            visionCollider.bounds.center,
            visionCollider.radius * transform.lossyScale.x,
            platformLayer
        );

        foreach (var hit in hits)
        {
            Physics2D.IgnoreCollision(hit, visionCollider, false);
        }
    }
}
