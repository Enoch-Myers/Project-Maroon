using UnityEngine;

public class Do_damage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collided");
        if(collision.gameObject.CompareTag("Player"))
        {
            var healthComp = collision.gameObject.GetComponent<Player_Health>();
            if(healthComp != null )
            {
                healthComp.TakeDamage(1);
                Debug.Log("did damage");
            }
            else
            {
                Debug.Log("no damage");
            }
        }
    }
}
