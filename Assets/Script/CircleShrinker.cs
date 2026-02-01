using Rive;
using Rive.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CircleShrinker : MonoBehaviour
{
    private Vector3 lastPosition;
    private float currentScale;
    private float targetScale;
    public ScreenTransition transition;
    
    [Header("Shrink Settings")]
    [SerializeField] private float shrinkAmount = 0.01f;
    [SerializeField] private float movementThreshold = 0.01f;
    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1f;
    [SerializeField] public float rewardScale = 1f;

    [Header("Smoothness Settings")]
    [SerializeField] private float scaleSpeed = 8f;

    public RiveWidget RivenWidget;
    private StateMachine AnimStateMachine;
    private SMINumber circleSizeProgress;

    public bool enableDebugControl = false;

    public GameObject shrinkingObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = shrinkingObject.transform.position;
        currentScale = maxScale;
        targetScale = maxScale;
        shrinkingObject.transform.localScale = Vector3.one * currentScale;


        AnimStateMachine = RivenWidget.StateMachine;
        circleSizeProgress = AnimStateMachine.GetNumber("Progress");

        circleSizeProgress.Value = (currentScale / maxScale) * 100;
    }


    // Update is called once per frame
    void Update()
    {
        if(AnimStateMachine == null)
        {
            AnimStateMachine = RivenWidget.StateMachine;
            circleSizeProgress = AnimStateMachine.GetNumber("Progress");

            circleSizeProgress.Value = (currentScale / maxScale) * 100;
        }
        
        ShrinkOnMovement();
        ApplySmoothedScale();

        circleSizeProgress.Value = (currentScale / maxScale) * 100;

        if(enableDebugControl)
        {
            if (Keyboard.current.rightShiftKey.wasPressedThisFrame)
            {
                IncreaseSize();
            }

            if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
            {
                DecreseSize();
            }
        }

        if(currentScale <= minScale + 0.0001f)
        {
            transition?.Play();
        }
    }

    private void ShrinkOnMovement()
    {
        float distance = Vector3.Distance(shrinkingObject.transform.position, lastPosition);
        
        if (distance > movementThreshold)
        {
            float shrinkFactor = distance * shrinkAmount;
            targetScale = Mathf.Max(targetScale - shrinkFactor, minScale);
        }
        
        lastPosition = shrinkingObject.transform.position;
    }

    private void ApplySmoothedScale()
    {
        currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleSpeed);
        shrinkingObject.transform.localScale = Vector3.one * currentScale;
    }

    public void IncreaseSize()
    {
        targetScale = Mathf.Min(rewardScale, maxScale);
    }

    public void DecreseSize()
    {   
        targetScale = Mathf.Max(targetScale - 0.1f, minScale);
    }
}
