using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret Lock State", menuName = "Turret States/Turret Lock State")]
public class TurretLockState : TurretBaseState
{
    private enum GunFireMode
    {
        AllGuns , InOrder
    }


    private Transform currentTarget;
    private Vector3 currentAim;

    [Header("Fire Settings")]
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GunFireMode gunFireMode;
    [SerializeField] private float timePerFire;

    [Header("Aim Settings")]
    [SerializeField] private float turretRotationSpeed;
    [SerializeField] private float fireAngleThreshold;

    private float timeSinceLastFire = 0;

    private Transform turretBody;
    private Transform turretGun;
    private Gun[] guns;

    private int currentWeaponIndex = 0;
    private Gun selectedGun;
    private float projectileSpeed;

    private delegate void FireMode();
    private FireMode FireModeEvent;

    public void InitializeState(TurretStateManager stateManager 
        , Transform turretBody , Transform turretGun , Gun[] guns)
    {
        // Edit This

        projectileSpeed = projectile.GetComponent<Projectile>().launchForce /
            projectile.GetComponent<Rigidbody>().mass;

        this.stateManager = stateManager;
        this.turretBody = turretBody;
        this.turretGun = turretGun;
        this.guns = guns;

        if (this.guns.Length == 0)
        {
            Debug.LogError("Error: There is no gun defined in guns list!");
        }

        currentWeaponIndex = 0;
        timeSinceLastFire = 0;

        switch (gunFireMode)
        {
            case GunFireMode.AllGuns:
                FireModeEvent += FireAllGuns;
                break;
            case GunFireMode.InOrder:
                FireModeEvent += FireGunsInOrder;
                break;
        }
    }


    public override void EnterState()
    {
        currentTarget = stateManager.currentTarget;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        Debug.Log(stateManager.gameObject.name + "Lock Update");
        if (currentTarget == null || !stateManager.detectionState.CanAimable(currentTarget))
        {
            currentTarget = null;
            stateManager.currentTarget = null;
            stateManager.SwitchState(stateManager.detectionState);
            return;
        }

        currentAim = AimCalculator.CalculateAimByObjectVelocity(currentTarget.GetComponent<Rigidbody>() , turretBody.position ,  projectileSpeed);

        LookToTarget();

        if (!IsInFireAngle()) return;

        timeSinceLastFire += Time.deltaTime;
        if (timeSinceLastFire >= timePerFire)
        {
            if (FireModeEvent != null)
                FireModeEvent.Invoke();
            timeSinceLastFire = 0;
        }
    }

    private bool IsInFireAngle()
    {
        Vector3 dir = (currentAim - turretGun.position).normalized;

        if(Vector3.Angle( dir , turretGun.forward) < fireAngleThreshold)
        {
            return true;
        }

        return false;
    }

    private void FireAllGuns()
    {
        for ( int i = 0; i < guns.Length; i++ )
        {
            Projectile projectileObj = Instantiate(projectile, guns[i].firePoint.position,
                guns[i].firePoint.rotation).GetComponent<Projectile>();

            if (projectileObj != null)
            {
                projectileObj.Launch(guns[i].firePoint.forward);
            }

            guns[i].PlayFireAnimation();
        }
    }

    private void FireGunsInOrder()
    {
        selectedGun = guns[currentWeaponIndex];

        //Projectile
        selectedGun.PlayFireAnimation();

        Projectile projectileObj = Instantiate(projectile , selectedGun.firePoint.position , 
            selectedGun.firePoint.localRotation).GetComponent<Projectile>();

        if (projectileObj != null)
        {
            projectileObj.Launch(selectedGun.firePoint.forward);
        }

        currentWeaponIndex++;
        if (currentWeaponIndex == guns.Length)
        {
            currentWeaponIndex = 0;
        }

    }

    private void LookToTarget()
    {
        Vector3 turretLookDir = currentAim - turretBody.position;
        Quaternion turretLookRotation = Quaternion.LookRotation( turretLookDir );
        Vector3 turretRotation = Quaternion.Lerp( turretBody.rotation ,
            turretLookRotation, Time.deltaTime*turretRotationSpeed).eulerAngles;

        turretBody.rotation = Quaternion.Euler(0f , turretRotation.y , 0f);

        Vector3 gunLookDir = currentAim - turretGun.position;
        Quaternion gunLookRotation = Quaternion.LookRotation(gunLookDir);
        Vector3 gunRotation = Quaternion.Lerp(turretGun.rotation,
            gunLookRotation, Time.deltaTime * turretRotationSpeed).eulerAngles;

        turretGun.localRotation = Quaternion.Euler(gunRotation.x, 0f, 0f);
    }
}
