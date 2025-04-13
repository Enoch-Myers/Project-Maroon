using UnityEngine;

public class Projectile_behavior : MonoBehaviour
{
    public float speed = 10f;

    public Vector2 direction = Vector2.right;

    private void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
