using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float speed = 2f;
    public bool loop = true;

    private int currentPointIndex = 0;

    void Update()
    {
        if (points.Length == 0)
            return;

        Transform targetPoint = points[currentPointIndex];
        Vector3 direction = targetPoint.position - transform.position;
        float step = speed * Time.deltaTime;

        if (direction.magnitude <= step)
        {
            transform.position = targetPoint.position;
            currentPointIndex++;

            if (currentPointIndex >= points.Length)
            {
                if (loop)
                    currentPointIndex = 0;
                else
                    currentPointIndex = points.Length - 1;
            }
        }
        else
        {
            transform.position += direction.normalized * step;
        }
    }

    // 2D version of parenting when player steps on platform
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player stepped on platform");
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left platform");
            other.transform.SetParent(null);
        }
    }
}
