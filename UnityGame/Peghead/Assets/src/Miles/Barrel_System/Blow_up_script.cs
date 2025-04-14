using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Blow_up_script : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null; // explosion radius collider2d

    [SerializeField] private float ExplosionForceMulti = 1000;
    [SerializeField] private float ExplosionRadius = 15;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // using K for now will be activated by killing barrels in future // no current attack
        {
            Debug.Log("exploded");
            Explode();
            Debug.Log("exploded2");

        }
    }
    void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach(Collider2D other in inExplosionRadius)
        {
            Rigidbody2D o_rb = other.GetComponent<Rigidbody2D>();
            if (o_rb != null)
            {
                Vector2 distanceVector = other.transform.position - transform.position;
                if (distanceVector.magnitude > 0)// so no NaN error
                {
                    float explosionForce = ExplosionForceMulti / distanceVector.magnitude;
                    o_rb.AddForce(distanceVector.normalized * explosionForce);
                }
            }
        }
    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
