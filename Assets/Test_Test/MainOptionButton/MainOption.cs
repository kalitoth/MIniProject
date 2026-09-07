using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainOption : MonoBehaviour
{
 public void Resume()
 {
     Time.timeScale = 1;
    
     transform.parent.gameObject.SetActive(false);

 }


    public void OptionScene()
    {
        SceneManager.LoadSceneAsync("Option");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void firstMenu()
    {
        SceneManager.LoadSceneAsync("Intro");
    }

}
