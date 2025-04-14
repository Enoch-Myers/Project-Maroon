using UnityEngine;

public class Do_damage : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {//checks if object colides into player if so does damage to player, used for testing while waiting for enemy attacks
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
