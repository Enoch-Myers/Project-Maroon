using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelActivationTest
{
    private LevelSelectManager levelSelect;
    private LevelNode testNode;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    [UnityTest]
    public IEnumerator Test_LevelActivation_WhenUnlocked()
    {
        yield return null; // wait one frame for scene load

        // fetch references from the loaded scene
        testNode = GameObject.Find("LevelNode0").GetComponent<LevelNode>();

        testNode.isLocked = false;
        testNode.levelID = "FakeLevel";

        LogAssert.Expect(LogType.Log, "Starting Level: FakeLevel");

        testNode.ActivateNode();

        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_LevelActivation_WhenLocked()
    {
        yield return null;

        testNode = GameObject.Find("LevelNode0").GetComponent<LevelNode>();

        testNode.isLocked = true;
        testNode.levelID = "FakeLevel";

        LogAssert.Expect(LogType.Log, "Attempted to start locked level: FakeLevel");

        testNode.ActivateNode();

        yield return null;
    }
}