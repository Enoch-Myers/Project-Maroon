using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CircleWipeTestsEditor
{
    [SetUp]
    public void SetUp()
    {
        // Load the scene before running the test
        EditorSceneManager.OpenScene("Assets/Scenes/TitleScreen.unity");
    }

    [Test]
    public void CircleEditorSize2203()
    {
        // Find the Circle object in the scene
        GameObject circleObject = GameObject.Find("SceneLoader/CircleWipe/Circle");
        Assert.IsNotNull(circleObject);

        // Get the RectTransform of the Circle object
        RectTransform circleRect = circleObject.GetComponent<RectTransform>();
        Assert.IsNotNull(circleRect);

        // Boundary test: Ensure that the circle's size is square and that dimensions are 2203x2203
        Assert.AreEqual(2203, circleRect.rect.width);
        Assert.AreEqual(2203, circleRect.rect.height);
    }
}