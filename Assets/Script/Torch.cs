using UnityEngine;

public class Torch : MonoBehaviour
{
    public CircleShrinker circleShrinker;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered torch area");
            circleShrinker.IncreaseSize();
            Destroy(gameObject);
        }
    }
}
