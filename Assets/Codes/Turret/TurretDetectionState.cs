using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Turret Detection State", menuName = "Turret States/Turret Detection State")]

public class TurretDetectionState : TurretBaseState
{
    [Header("Detection Properties")]
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] string detectionTag;
    [SerializeField] TargetType aimableTargets;

    [SerializeField] private float minimumRange;
    [SerializeField] private float maximumRange;

    public void InitializeState(TurretStateManager stateManager)
    {
        this.stateManager = stateManager;
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        UpdateDetection();
    }

    private void UpdateDetection()
    {
        Target[] targets = FindObjectsOfType<Target>();
        List<Target> spesificTargets = targets.Where(t => t.targetType == aimableTargets).ToList();

        if (spesificTargets.Count == 0)
            return;

        GameObject closestTarget = null;
        float shortestDistance = Mathf.Infinity; ;

        for (int i = 0; i < spesificTargets.Count; i++)
        {

            float distance = Vector3.Distance(spesificTargets[i].transform.position, stateManager.transform.position);
            if (distance <= shortestDistance)
            {
                closestTarget = spesificTargets[i].gameObject;
                shortestDistance = distance;
            }
        }

        if (closestTarget != null && (shortestDistance < maximumRange && shortestDistance > minimumRange))
        {
            stateManager.UpdateTarget(closestTarget.transform);
            stateManager.SwitchState(stateManager.lockState);
        }
    }

    public bool CanAimable(Transform targetTransform)
    {
        float distance = Vector3.Distance(targetTransform.position , stateManager.transform.position);

        if (targetTransform != null && (distance < maximumRange && distance > minimumRange))
        {
            return true;
        }
        return false;
    }

    public float GetMaximumRange()
    {
        return maximumRange;
    }

    public float GetMinimumRange()
    {
        return minimumRange;
    }
}
