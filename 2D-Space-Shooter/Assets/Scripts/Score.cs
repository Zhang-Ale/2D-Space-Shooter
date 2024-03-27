using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI LeftScore;
    public int LeftCount;

    public TextMeshProUGUI RightScore;
    public int RightCount;
    
    void Awake()
    {        
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MenuScene")
        {
            Destroy(this.gameObject);
        }

        /*int buildIndex = currentScene.buildIndex;
        switch (buildIndex)
        {
            case 0:
                Destroy(this.gameObject);
                break;
        }*/
    }

    public void LeftScored()
    {
        LeftCount += 1;
        LeftScore.text = "Score: " + LeftCount;       
    }

    public void RightScored()
    {
        RightCount += 1;
        RightScore.text = "Score: " + RightCount;
    }
}
