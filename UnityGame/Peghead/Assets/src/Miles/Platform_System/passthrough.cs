using System.Collections;
using UnityEngine;
//still wip
public class passthrough : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();

    }
    private void Update()
    {
        if (_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            _collider.enabled = false;
            StartCoroutine(enableCollider());
        }
    }

    private IEnumerator enableCollider()
    {
        yield return new WaitForSeconds(.5f);
        _collider.enabled=true;
    }
    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        /*var player = other.gameObject.GetComponent<Player>(); // not working yet dont want to mess up compiling
        if (player != null)
        {
            _playerOnPlatform = player;
        }*/
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, false);
    }
}
