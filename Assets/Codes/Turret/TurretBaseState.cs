using UnityEngine;

public abstract class TurretBaseState : ScriptableObject
{
    protected TurretStateManager stateManager;

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

}
