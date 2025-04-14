using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveSpamTest
{
    private LevelSelectManager levelSelect;
    private LevelNode testNodeA, testNodeB;

    [SetUp]
    public void Setup() //Creates 2 Level Nodes, and connectes B to A's right side
    {
        SceneManager.LoadScene("LevelSelect");
    }

    [UnityTest]
    public IEnumerator Test_MoveSpam()
    {
        yield return null; // Wait a frame for the scene to load

        // GameObject managerObj = new GameObject();
        // levelSelect = managerObj.AddComponent<LevelSelectManager>();

        levelSelect = GameObject.Find("LevelSelectManager").GetComponent<LevelSelectManager>();

        // testNodeA = new GameObject().AddComponent<LevelNode>();
        // testNodeB = new GameObject().AddComponent<LevelNode>();

        testNodeA = GameObject.Find("LevelNode0").GetComponent<LevelNode>();
        testNodeB = GameObject.Find("LevelNode1").GetComponent<LevelNode>();

        testNodeA.right = testNodeB;  //A->B
        testNodeB.left = testNodeA;  //A<-B

        levelSelect.currentNode = testNodeA; //start @ A

        for(int i=0;i<1_000_000_000;i++){
            levelSelect.MoveToNode(testNodeB);
            levelSelect.MoveToNode(testNodeA);
        }
    }
}