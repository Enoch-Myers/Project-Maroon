using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovementTest
{
    private LevelSelectManager levelSelect;
    private LevelNode testNodeA, testNodeB;

    [SetUp]
    public void Setup() //Creates 2 Level Nodes, and connectes B to A's right side
    {
        SceneManager.LoadScene("LevelSelect");
    }

    [UnityTest]
    public IEnumerator Test_BoundaryMovement()
    {
        yield return null; // Wait a frame for the scene to load

        levelSelect = GameObject.Find("LevelSelectManager").GetComponent<LevelSelectManager>();

        testNodeA = GameObject.Find("LevelNode0").GetComponent<LevelNode>();
        testNodeB = GameObject.Find("LevelNode1").GetComponent<LevelNode>();

        testNodeA.right = testNodeB;
        levelSelect.currentNode = testNodeA;

        levelSelect.MoveToNode(null);
        Assert.AreEqual(testNodeA, levelSelect.currentNode, "Player should not move to a null node.");
    }
}