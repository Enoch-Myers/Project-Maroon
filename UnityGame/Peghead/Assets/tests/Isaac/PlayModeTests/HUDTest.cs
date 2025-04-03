using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

public class HUDTest
{
    [UnityTest]
    public IEnumerator HUDTestWithEnumeratorPasses()
    {
        // Load the scene
        yield return SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Single);

        PlayerHealth playerHealth = GameObject.FindFirstObjectByType<PlayerHealth>();
        DieWinScreen dieWinScreen = GameObject.FindFirstObjectByType<DieWinScreen>();

        yield return new WaitForSeconds(1f);

        for (int i = 1; i < 4; i++)
        {
            playerHealth.TakeDamage();
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);

        // Reload the scene
        yield return SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Single);
        
        // Wait for new DieWinScreen to be created by GameManager
        yield return new WaitUntil(() =>
            GameObject.FindFirstObjectByType<DieWinScreen>(FindObjectsInactive.Include) != null);

        // Get the new instance (even if inactive)
        DieWinScreen dieWinScreen2 = GameObject.FindFirstObjectByType<DieWinScreen>(FindObjectsInactive.Include);
        Assert.IsNotNull(dieWinScreen2, "DieWinScreen not found after scene reload");

        yield return new WaitForSeconds(1f);

        // Force enable it if needed before calling ShowWin
        dieWinScreen2.gameObject.SetActive(true);
        dieWinScreen2.ShowWin();

        yield return new WaitForSeconds(2f);
    }

}
