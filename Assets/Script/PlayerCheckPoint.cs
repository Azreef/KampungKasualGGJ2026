using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    // Assign this in the inspector or set it in Start()
    public Vector3 checkpointPosition;

    void Start()
    {
        // Initialize checkpointPosition to player's starting position
        checkpointPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            // Reset player position to checkpoint
            transform.position = checkpointPosition;

            // Optional: Add other effects like playing sound, animation, or reducing health here
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            // Update checkpoint position when player touches a checkpoint
            checkpointPosition = collision.transform.position;
        }
    }
}
