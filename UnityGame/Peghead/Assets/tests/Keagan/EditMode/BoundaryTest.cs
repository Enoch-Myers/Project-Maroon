using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovementBoundaryTest
{
    private LevelSelectManager levelSelect;
    private LevelNode testNodeA, testNodeB;

    [SetUp]
    public void Setup() //Creates 2 Level Nodes, and connectes B to A's right side
    {
        SceneManager.LoadScene("LevelSelect");
    }

    [UnityTest]
    public IEnumerator Test_Movement()
    {
        yield return null; // Wait a frame for scene to load
        
        // GameObject managerObj = new GameObject();
        // levelSelect = managerObj.AddComponent<LevelSelectManager>();

        levelSelect = GameObject.Find("LevelSelectManager").GetComponent<LevelSelectManager>();

        // testNodeA = new GameObject().AddComponent<LevelNode>();
        // testNodeB = new GameObject().AddComponent<LevelNode>();

        testNodeA = GameObject.Find("LevelNode0").GetComponent<LevelNode>();
        testNodeB = GameObject.Find("LevelNode1").GetComponent<LevelNode>();

        testNodeA.right = testNodeB;
        levelSelect.currentNode = testNodeA;

        levelSelect.MoveToNode(testNodeB);
        Assert.AreEqual(testNodeB, levelSelect.currentNode, "Player should be at Node B.");
    }
}
