using UnityEngine;

public class PlayerCircleCollisionController : MonoBehaviour
{
    public GameObject circle;             // Circle GameObject that reveals platform
    public float circleRadius = 2f;       // Radius of the visible circle
    public LayerMask platformLayer;       // Layer mask for platform layer

    public Transform feetPosition;        // Empty GameObject at player's feet for ground check
    public float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGroundedInsideCircle();
    }

    void CheckGroundedInsideCircle()
    {
        Vector2 circleCenter = circle.transform.position;

        // Get all colliders overlapping with ground check circle around feet
        Collider2D[] hits = Physics2D.OverlapCircleAll(feetPosition.position, groundCheckRadius, platformLayer);

        isGrounded = false;

        foreach (Collider2D hit in hits)
        {
            // Find closest point on the collider to feet position
            Vector2 closestPoint = hit.ClosestPoint(feetPosition.position);

            // Check if that closest point is inside the circle mask radius
            if (Vector2.Distance(closestPoint, circleCenter) <= circleRadius)
            {
                isGrounded = true;
                break;
            }
        }

        // Optional: debug log
        // Debug.Log("Is Grounded inside circle? " + isGrounded);

        // Now, handle player grounded state (this depends on your existing movement code)
        // Example: Disable gravity if grounded, enable if not
        if (isGrounded)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;  // stop vertical velocity on ground
        }
        else
        {
            rb.gravityScale = 1;  // or your normal gravity scale
        }
    }

    void OnDrawGizmosSelected()
    {
        if (feetPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(feetPosition.position, groundCheckRadius);

            if (circle != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(circle.transform.position, circleRadius);
            }
        }
    }
}
