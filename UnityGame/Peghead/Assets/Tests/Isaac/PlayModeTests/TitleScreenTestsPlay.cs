using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TitleScreenTestsPlay
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    [UnityTest]
    public IEnumerator SpamTitleScreen()
    {
        GameObject titleScreen = GameObject.Find("TitleScreen");
        Assert.IsNotNull(titleScreen);

        // Stress test (figure out how many times we instantiate `TitleScreen` before unity crashes)
        for (int i = 0; i < 50_000; i++) {
            GameObject.Instantiate(titleScreen);
        }

        return null;
    }

    [TearDown]
    public void TearDown()
    {
        SceneManager.UnloadSceneAsync("TitleScreen");
    }
}