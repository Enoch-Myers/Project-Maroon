using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    public int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth{
        get { return currentHealth; }
    }

    void Start(){
        currentHealth = maxHealth;
    }

    void Update(){
        // Basic "follow/avoid" logic if player is assigned
        if(player != null){
            if(transform.position.x < player.position.x){
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else if(transform.position.x > player.position.x){
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
    }

    public void TakeDamage(int damageAmount){
        if(currentHealth < 0) currentHealth = 0;

        if(currentHealth == 0) Debug.Log("Boss defeated!");
    }
}