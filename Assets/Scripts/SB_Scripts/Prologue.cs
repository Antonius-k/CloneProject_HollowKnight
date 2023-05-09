using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Prologue : MonoBehaviour
{
    public float currentTime = 0;
    public float sceneTime = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > sceneTime)
        {
            SceneManager.LoadScene("StartScene");
            currentTime = 0;
        }

    }
}
