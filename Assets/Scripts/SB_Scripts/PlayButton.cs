using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public GameObject credit;
    public GameObject panel;

    private void Start()
    {
        credit.SetActive(false);
    }

    public void ClickPlayButton()
    {
        credit.SetActive(true);
    }

    public void ClickQuitButton()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SceneManager.LoadScene("StartScene");
        }
    }

}
