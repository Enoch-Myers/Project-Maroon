using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class Blow_up_script : MonoBehaviour
{
    Collider2D[] inExplosionRadius = null; // explosion radius collider2d
    public GameObject ExplosionEffectPrefab;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Barrel collided");

        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Projectile"))
        {
            Debug.Log("that bih blew up");
            Explode();
        }
        if (collision.collider.CompareTag("Player"))
        {
            Player_Health playerHealth = collision.collider.GetComponent<Player_Health>();

            if (playerHealth != null)
            {
                playerHealth.currentHealth = 0;
            }
        }
    }
    void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D other in inExplosionRadius)
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
        Instantiate(ExplosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos() // draw gizmos
    {
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
