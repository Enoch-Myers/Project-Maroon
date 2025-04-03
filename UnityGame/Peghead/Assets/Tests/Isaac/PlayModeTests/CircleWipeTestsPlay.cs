using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CircleWipeTestsPlay
{
    [UnityTest, Ignore("This test is temporarily disabled")]
    public IEnumerator CircleEndAnimation0Size()
    {
        yield return SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Single);

        GameObject titleScreen = GameObject.Find("TitleScreen");
        Assert.IsNotNull(titleScreen);

        RectTransform circleRect = SceneLoader.Instance.transform.Find("CircleWipe/Circle").GetComponent<RectTransform>();
        Assert.IsNotNull(circleRect);

        float transitionTime = SceneLoader.Instance.transitionTime;
        Assert.AreEqual(1f, transitionTime);

        titleScreen.GetComponent<TitleScreen>().OnAnyButtonPress();

        yield return new WaitForSeconds(transitionTime);

        var dist = Vector2.Distance(Vector2.zero, circleRect.sizeDelta);
        Assert.IsTrue(dist < 0.1f, dist + " is not less than 0.1!");
    }
}
