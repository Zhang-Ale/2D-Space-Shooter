using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuFlickers : MonoBehaviour
{
    public TextMeshProUGUI LeftPlayerT;
    public TextMeshProUGUI RightPlayerT;
    public GameObject LeftPlay;
    public GameObject RightPlay;
    public TextMeshProUGUI PlayText;

    void Start()
    {
        StartCoroutine(Flickering());
    }

    IEnumerator Flickering()
    {
        while (true)
        {
            LeftPlayerT.alpha = 255f;
            RightPlayerT.alpha = 255f;
            LeftPlay.SetActive(true);
            RightPlay.SetActive(true);
            PlayText.alpha = 255f;
            yield return new WaitForSeconds(0.5f);
            LeftPlayerT.alpha = 0f;
            RightPlayerT.alpha = 0f;
            LeftPlay.SetActive(false);
            RightPlay.SetActive(false);
            PlayText.alpha = 0f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
