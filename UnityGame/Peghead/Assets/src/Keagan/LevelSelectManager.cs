using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public LevelNode currentNode;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && currentNode.up) MoveToNode(currentNode.up);
        if (Input.GetKeyDown(KeyCode.S) && currentNode.down) MoveToNode(currentNode.down);
        if (Input.GetKeyDown(KeyCode.A) && currentNode.left) MoveToNode(currentNode.left);
        if (Input.GetKeyDown(KeyCode.D) && currentNode.right) MoveToNode(currentNode.right);

        if (Input.GetKeyDown(KeyCode.Return)) currentNode.ActivateNode();
    }

    public void MoveToNode(LevelNode newNode)
    {
        if (newNode == null) {
            return;
        }
        
        currentNode.DeselectNode();
        currentNode = newNode;
        currentNode.SelectNode();
    }
}
