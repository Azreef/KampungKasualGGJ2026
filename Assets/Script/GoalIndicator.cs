using Rive;
using Rive.Components;
using UnityEngine;
using UnityEngine.UI;

public class GoalIndicator : MonoBehaviour
{
    [Header("World References")]
    public Transform player;
    public Transform pointA;
    public Transform pointB;

    private float totalDistance;

    public RiveWidget RivenWidget;
    private StateMachine AnimStateMachine;
    private SMINumber goalHeightProgress;

    void Start()
    {
        // Calculate total distance once
        totalDistance = Vector3.Distance(pointA.position, pointB.position);

        AnimStateMachine = RivenWidget.StateMachine;
        goalHeightProgress = AnimStateMachine.GetNumber("ProgressHeight");

        goalHeightProgress.Value = 0;
    }

    void Update()
    {
        UpdateProgress();
    }

    void UpdateProgress()
    {
        // Vector from A to B
        Vector3 AtoB = pointB.position - pointA.position;
        
        // Vector from A to player
        Vector3 AtoPlayer = player.position - pointA.position;
        
        // Project player position onto A-B line
        float projectedDistance = Vector3.Dot(AtoPlayer, AtoB.normalized);
        
        // Convert to 0 → 1 range
        float progress = Mathf.Clamp01(projectedDistance / totalDistance);

        // Move indicator in UI
        //indicator.anchoredPosition = Vector2.Lerp(startPosition, endPosition, progress);

        goalHeightProgress.Value = progress * 100;
        Debug.Log(goalHeightProgress.Value);
    }
}
