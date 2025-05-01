using UnityEngine;

public class ShipFollower : MonoBehaviour
{
    public LevelSelectManager manager;
    public float followSpeed = 5f;
    public float bobbingAmplitude = 0.2f;
    public float bobbingFrequency = 2f;

    private Vector3 offset;
    private Vector3 lastTargetPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (manager == null)
        {
            Debug.LogError("ShipFollower: LevelSelectManager not assigned!");
            enabled = false;
            return;
        }

        offset = transform.position - manager.currentNode.transform.position;
        lastTargetPosition = manager.currentNode.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (manager.currentNode == null) return;

        Vector3 nodePosition = manager.currentNode.transform.position;
        Vector3 targetPos = nodePosition + offset;
        targetPos.y += Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        // flip sprite based on movement direction
        float deltaX = nodePosition.x - lastTargetPosition.x;
        if (Mathf.Abs(deltaX) > 0.01f)
        {
            spriteRenderer.flipX = deltaX < 0; // if moving left, face left
        }

        lastTargetPosition = nodePosition;
    }
}
