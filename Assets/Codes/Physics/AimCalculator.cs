using UnityEngine;

public static class AimCalculator
{
    const float VELOCITY_THRESHOLD = 0.2f;

    public static Vector3 CalculateAimByObjectVelocity(Rigidbody target , Vector3 firePoint  , float projectileSpeed)
    {
        if(target.velocity.magnitude < VELOCITY_THRESHOLD)
            return target.position;

        float distance = Vector3.Distance(target.position , firePoint);
        float t = distance / projectileSpeed + target.velocity.magnitude;
        
        Vector3 calculatedPos = target.position + target.velocity * t;
        return calculatedPos;
    }

    public static Vector3 CalculateAimByObjectVelocity(Transform target, Vector3 firePoint , Vector3 speed, float projectileSpeed)
    {
        if (speed.magnitude < VELOCITY_THRESHOLD)
            return target.position;

        float distance = Vector3.Distance(target.position, firePoint);
        float t = distance / 60;

        Vector3 calculatedPos = target.position + speed * t;
        return calculatedPos;
    }

}
