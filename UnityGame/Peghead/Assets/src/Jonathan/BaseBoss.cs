using UnityEngine;

//Parent Class other bosses inherit from
public class BaseBoss : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public int maxHealth = 100;
    protected int currentHealth;

    public int CurrentHealth{
        get{ return currentHealth; }
    }

    public virtual void Start(){
        currentHealth = maxHealth;
    }

    public virtual void Update(){
        if(player != null){
            if(transform.position.x < player.position.x){
                transform.position += Vector3.right * speed * Time.deltaTime;
            }else if(transform.position.x > player.position.x){
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
    }

    public virtual void TakeDamage(int damageAmount){
        currentHealth -= damageAmount;
        if(currentHealth < 0) currentHealth = 0;
        
        if (currentHealth == 0) Debug.Log("Boss defeated: " + gameObject.name);
    }
}