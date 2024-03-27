using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject optionsMenu; 

    public void ChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
