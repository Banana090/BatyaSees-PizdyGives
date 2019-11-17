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
        StartCoroutine(FadeIn("Easy"));
    }

    public void LoadMedium()
    {

    }

    public void LoadHard()
    {

    }

    public void LoadTutorial()
    {

    }

    private IEnumerator FadeIn(string sceneName)
    {
        blackScreen.SetTrigger("Show");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
