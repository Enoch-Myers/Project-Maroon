using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TitleScreenTestsPlay
{
    [UnityTest, Ignore("Stress test is disabled for now")]
    public IEnumerator SpamTitleScreen()
    {
        // Load the scene asynchronously
        yield return SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Single);

        GameObject titleScreen = GameObject.Find("TitleScreen");
        Assert.IsNotNull(titleScreen);

        // Stress test
        for (int i = 0; i < 50_000; i++)
        {
            GameObject.Instantiate(titleScreen);
        }

        yield return null;
    }
}