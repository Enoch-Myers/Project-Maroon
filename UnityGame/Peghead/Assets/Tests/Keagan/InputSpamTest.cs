using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MoveSpamTest
{
    private LevelSelectManager levelSelect;
    private LevelNode testNodeA, testNodeB;

    [SetUp]
    public void Setup()
    {
        GameObject managerObj = new GameObject();
        levelSelect = managerObj.AddComponent<LevelSelectManager>();

        testNodeA = new GameObject().AddComponent<LevelNode>();
        testNodeB = new GameObject().AddComponent<LevelNode>();

        testNodeA.right = testNodeB;  //A->B
        testNodeB.left = testNodeA;  //A<-B

        levelSelect.currentNode = testNodeA; //start @ A
    }

    [Test]
    public void Test_MoveSpam()
    {
        for(int i=0;i<100000;i++){
            levelSelect.MoveToNode(testNodeB);
            levelSelect.MoveToNode(testNodeA);
        }
    }
}