using UnityEngine;

public class Projectile_behavior : MonoBehaviour
{
    public float speed = 10f;
    // Update is called once per frame
    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
