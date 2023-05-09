using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCanvas : MonoBehaviour
{
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    public Sprite healthFull;
    public Sprite healthEmpty;

    private Image[] hearts = new Image[5];

    public GameObject playerController;
    
    int hp;

    void Start()
    {
        hearts[0] = heart1.GetComponent<Image>();
        hearts[1] = heart2.GetComponent<Image>();
        hearts[2] = heart3.GetComponent<Image>();
        hearts[3] = heart4.GetComponent<Image>();
        hearts[4] = heart5.GetComponent<Image>();
    }

    void Update()
    {

        //-경우일때 예외처리 해줘야 된다.
        hp = playerController.GetComponent<PlayerController>().hp;

        for (int i = 0; i < hp; ++i)
        {
            hearts[i].sprite = healthFull;
        }

        for (int i = hp; i < 5; ++i)
        {
            hearts[i].sprite = healthEmpty;
        }
    }
}
