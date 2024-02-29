using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using UnityEditor.Timeline.Actions;

public class TurretStateManager : MonoBehaviour
{
    private TurretBaseState currentState;
    [Header("States")]
    [HorizontalLine(color: EColor.Yellow)]
    [Expandable]
    public TurretDetectionState detectionState;
    [Expandable]
    public TurretLockState lockState;

    public Transform currentTarget;

    [Header("Turret Settings")]
    [HorizontalLine(color: EColor.Yellow)]
    [SerializeField] private Transform turretBody;
    [SerializeField] private Transform turretGun;
    public Gun[] guns;

    private void Start()
    {
        //Inializing states

        detectionState.InitializeState( this );
        lockState.InitializeState( this , turretBody , turretGun , guns );

        currentState = detectionState;
        currentState.EnterState();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(TurretBaseState state)
    {
        currentState.ExitState();
        currentState = state;
        state.EnterState();
    }

    public void UpdateTarget(Transform target)
    {
        currentTarget = target;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionState == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , detectionState.GetMaximumRange());
        Gizmos.DrawWireSphere(transform.position, detectionState.GetMinimumRange());
    }


}
[System.Serializable]
public class Gun
{
    public Transform firePoint;
    public ParticleSystem fireParticle;
    [Header("Animation")]
    [SerializeField] Animator animator;

    public void PlayFireAnimation()
    {
        fireParticle.Play();
        //
        if(animator != null ) animator.SetTrigger("Fire");
    }

}