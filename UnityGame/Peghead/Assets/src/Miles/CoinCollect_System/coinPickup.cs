using UnityEngine;
using System.Collections;


public class coinPickup : MonoBehaviour // implementted in enochs player movement script
{
    public Coin_despawner CM;
 

     public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coins"))
        {
            Destroy(other.gameObject);
            CM.coinCount++;
        }
    }
}
