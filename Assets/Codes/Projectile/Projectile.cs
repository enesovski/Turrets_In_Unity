using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [Header("Launch")]
    public float launchForce;
    public float destroyTime = 5f;
    public float accuracyDeviationAngle;

    [Header("Impact")]
    public int damage;
    public float impactForce;
    public GameObject impactParticle;

    [Header("Debug")]
    [SerializeField] private float collisionDisableTime;

    [HideInInspector] public Rigidbody rb;

    public void Launch(Vector3 dir)
    {
        StartCoroutine(DisableCollider());
        rb = GetComponent<Rigidbody>();

        float randomAngle = Random.Range(-accuracyDeviationAngle, accuracyDeviationAngle);

        Vector3 finalDirection = Quaternion.AngleAxis(randomAngle, Vector3.up) * dir;

        rb.AddForce(finalDirection * launchForce, ForceMode.Impulse);
        transform.rotation = Quaternion.LookRotation(dir);

        Debug.Log("Collision : " + rb.velocity.magnitude);
        Destroy(gameObject , destroyTime);


    }

    private IEnumerator DisableCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(collisionDisableTime);
        GetComponent<Collider>().enabled = true;

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Impact(collision);
    }

    protected virtual void Impact(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(transform.forward * impactForce);
        }

        if (collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }

        //VFX
        Destroy(Instantiate(impactParticle , transform.position , transform.rotation ) , 0.5f);

        Destroy(gameObject);
    }

}
