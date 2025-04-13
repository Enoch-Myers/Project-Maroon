using UnityEngine;

// Parent class
public class BaseBoss : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public int maxHealth = 100;
    public int currentHealth;

    // Unity calls Start() automatically if this component is enabled.
    public virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // Unity calls Update() automatically each frame.
    public virtual void Update()
    {
        if (player != null)
        {
            // Basic horizontal movement toward the player.
            if (transform.position.x < player.position.x)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else if (transform.position.x > player.position.x)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
    }

    // Virtual method for taking damage.
    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth == 0)
            Debug.Log("Boss defeated: " + gameObject.name);
    }
}

// Stage Two boss – inherits from BaseBoss and overrides some functionality.
public class BStage2 : BaseBoss
{
    // Override to take only half damage.
    public override void TakeDamage(int damageAmount)
    {
        int actualDamage = Mathf.FloorToInt(damageAmount * 0.5f); // Half damage
        base.TakeDamage(actualDamage);
    }

    // Optionally override Update() for additional behavior.
    public override void Update()
    {
        base.Update();
        // Additional logic for stage two (e.g., better pathing) can be added here.
    }
}
