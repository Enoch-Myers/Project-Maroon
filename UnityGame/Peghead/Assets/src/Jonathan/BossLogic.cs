using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    void Update()
    {
        if(player != null){
            if(transform.position.x < player.position.x) {
                transform.position += Vector3.right * speed * Time.deltaTime;
            } else if(transform.position.x > player.position.x) {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
        }
    }
}
