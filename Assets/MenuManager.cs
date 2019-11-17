using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator blackScreen;

    private void Start()
    {
        blackScreen.SetTrigger("Hide");
    }

    public void LoadEasy()
    {
        StartCoroutine(FadeIn(1));
    }

    public void LoadMedium()
    {
        StartCoroutine(FadeIn(2));
    }

    public void LoadHard()
    {
        StartCoroutine(FadeIn(3));
    }

    private IEnumerator FadeIn(int sceneID)
    {
        blackScreen.SetTrigger("Show");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneID);
    }
}
