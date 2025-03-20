using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    GameObject textLabel;
    IEnumerator flashTextCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textLabel = transform.Find("Text").gameObject;
        flashTextCoroutine = FlashText();
        StartCoroutine(flashTextCoroutine);
    }

    public void OnAnyButtonPress()
    {
        StopCoroutine(flashTextCoroutine);
        SceneLoader.Instance.LoadSceneAsync("LevelSelect");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            OnAnyButtonPress();
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
