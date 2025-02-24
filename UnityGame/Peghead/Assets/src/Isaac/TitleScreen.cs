using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    GameObject textLabel;
    IEnumerator flashTextCoroutine;
    SceneLoader sceneLoader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textLabel = transform.Find("Text").gameObject;
        sceneLoader = FindFirstObjectByType<SceneLoader>();
        flashTextCoroutine = FlashText();
        StartCoroutine(flashTextCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            StopCoroutine(flashTextCoroutine);
            sceneLoader.LoadSceneAsync("LevelSelect");
        }
    }

    IEnumerator FlashText()
    {
        while (true)
        {
            textLabel.SetActive(!textLabel.activeInHierarchy);
            yield return new WaitForSeconds(.75f);
        }
    }
}
