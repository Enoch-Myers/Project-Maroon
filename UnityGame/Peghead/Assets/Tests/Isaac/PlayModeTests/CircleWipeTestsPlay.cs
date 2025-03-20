using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CircleWipeTestsPlay
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    [UnityTest]
    public IEnumerator CircleEndAnimation0Size()
    {
        GameObject titleScreen = GameObject.Find("TitleScreen");
        Assert.IsNotNull(titleScreen);

        RectTransform circleRect = SceneLoader.Instance.transform.Find("CircleWipe/Circle").GetComponent<RectTransform>();
        Assert.IsNotNull(circleRect);

        float transitionTime = SceneLoader.Instance.transitionTime;
        Assert.AreEqual(1f, transitionTime);

        titleScreen.GetComponent<TitleScreen>().OnAnyButtonPress();

        yield return new WaitForSeconds(transitionTime); // Wait for the circle wipe transition to finish

        // Boundary test: Make sure the black circle has completely enveloped the screen (i.e. the inside circle has size of (0, 0))
        var dist = Vector2.Distance(new Vector2(0, 0), circleRect.sizeDelta);
        Assert.IsTrue(dist < .1, dist + " is not less than .01!");
    }

    [TearDown]
    public void TearDown()
    {
        SceneManager.UnloadSceneAsync("TitleScreen");
    }
}