using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Mirror;

public class GameTime : NetworkBehaviour
{
    public bool gameOver = false;
    public AudioSource backgroundMusic;
    public float _Volume = 1f;
    [SerializeField]
    private float timeRemaining = 10;
    bool timeIsRunning;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI CountThree;
    public TextMeshProUGUI CountTwo;
    public TextMeshProUGUI CountOne;
    public TextMeshProUGUI GameOverT;
    public TextMeshProUGUI RetMenuT;
    public Animator GameOverAnim;

    void Start()
    {
        RetMenuT = GameObject.Find("ReturnToMenuTxt").GetComponent<TextMeshProUGUI>();
        
        RetMenuT.faceColor = new Color32(255, 255, 255, 0);
        GameOverT.alpha = 0f;
        GameOverAnim = GameOverT.GetComponent<Animator>();
        GameOverAnim.SetBool("GameOverOn", false);
        StartCoroutine(CountTime(1f));
        StartCoroutine(PlayBackgroundMusic(3f));
        StartCoroutine(TxtFlicker());
    }

    void Update()
    {
        StartCoroutine(DisplayTime(timeRemaining));
        End();
    }

    IEnumerator TxtFlicker()
    {
        while (true)
        {
            RetMenuT.text = " Press anywhere  to return to menu";
            yield return new WaitForSeconds(0.5f);
            RetMenuT.text = ".";
            yield return new WaitForSeconds(0.5f);
        }           
    }

    IEnumerator CountTime(float _wait)
    {
        CountThree.alpha = 255f;
        yield return new WaitForSeconds(_wait);
        CountThree.alpha = 0f;
        CountTwo.alpha = 255f;
        yield return new WaitForSeconds(_wait);
        CountTwo.alpha = 0f;
        CountOne.alpha = 255f;
        yield return new WaitForSeconds(_wait);
        CountOne.alpha = 0f;
        yield return null;
    }

    IEnumerator DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 0;
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        //62 % 60 = 1min2sec; 125 & 60 = 2min5sec; 46 % 60 = 46sec
        //float milliSeconds = (timeToDisplay % 1) * 1000;

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        yield return new WaitForSeconds(4f);
        timeIsRunning = true;        
    }

    void End()
    {
        if (timeIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                gameOver = true;
                timeIsRunning = false;
                StartCoroutine(GameOver());
                if (Input.GetMouseButton(0))
                {
                    int currentScene = SceneManager.GetActiveScene().buildIndex;
                    SceneManager.LoadScene(currentScene + 2);
                    
                }
            }
        }
    }
    IEnumerator GameOver()
    {
        float startVolume = backgroundMusic.volume;
        backgroundMusic.volume -= startVolume * Time.deltaTime / 1.5f;
        GameOverT.alpha = 255f;
        GameOverAnim.SetBool("GameOverOn", true);
        
        RetMenuT.faceColor = new Color32(255, 255, 255, 255);
        yield return null;
    }

    IEnumerator PlayBackgroundMusic(float waitTime)
    {
        backgroundMusic.volume = _Volume;
        yield return new WaitForSeconds(waitTime);
        backgroundMusic.Play();
    }
}
