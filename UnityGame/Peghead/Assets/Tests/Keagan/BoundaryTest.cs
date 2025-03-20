using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MovementBoundaryTest
{
    private LevelSelectManager levelSelect;
    private LevelNode testNodeA, testNodeB;

    [SetUp]
    public void Setup() //Creates 2 Level Nodes, and connectes B to A's right side
    {
        GameObject managerObj = new GameObject();
        levelSelect = managerObj.AddComponent<LevelSelectManager>();

        testNodeA = new GameObject().AddComponent<LevelNode>();
        testNodeB = new GameObject().AddComponent<LevelNode>();

        testNodeA.right = testNodeB;
        levelSelect.currentNode = testNodeA;
    }

    [Test]
    public void Test_Movement()
    {
        levelSelect.MoveToNode(testNodeB);
        Assert.AreEqual(testNodeB, levelSelect.currentNode, "Player should be at Node B.");
    }
}
