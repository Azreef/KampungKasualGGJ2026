using UnityEngine;
using UnityEngine.InputSystem;

public class CircleShrinker : MonoBehaviour
{
    private Vector3 lastPosition;
    private float currentScale;
    private float targetScale;
    
    [Header("Shrink Settings")]
    [SerializeField] private float shrinkAmount = 0.01f;
    [SerializeField] private float movementThreshold = 0.01f;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1f;
    
    [Header("Smoothness Settings")]
    [SerializeField] private float scaleSpeed = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = transform.position;
        currentScale = maxScale;
        targetScale = maxScale;
        transform.localScale = Vector3.one * currentScale;
    }

    // Update is called once per frame
    void Update()
    {
        ShrinkOnMovement();
        ApplySmoothedScale();

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            IncreaseSize();
        }
        
        if(Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            DecreseSize();
        }
    }

    private void ShrinkOnMovement()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        
        if (distance > movementThreshold)
        {
            float shrinkFactor = distance * shrinkAmount;
            targetScale = Mathf.Max(targetScale - shrinkFactor, minScale);
        }
        
        lastPosition = transform.position;
    }

    private void ApplySmoothedScale()
    {
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleSpeed);
        transform.localScale = Vector3.one * currentScale;
    }

    public void IncreaseSize()
    {
        targetScale = Mathf.Min(0.39f, maxScale);
    }

    public void DecreseSize()
    {
        targetScale = Mathf.Max(targetScale - 0.1f, minScale);
    }
}
