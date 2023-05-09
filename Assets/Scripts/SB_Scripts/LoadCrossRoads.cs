using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCrossRoads : MonoBehaviour
{
    public GameObject crossRoads;
    //public Image crossRoadsImage;
    public CanvasGroup crossRoadsGroup;


    
    // Start is called before the first frame update
    void Start()
    {
        crossRoads.SetActive(false);
        //crossRoadsImage = crossRoads.GetComponentInChildren<Image>();
        crossRoadsGroup = crossRoads.GetComponent<CanvasGroup>();


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //crossRoadsGroup.alpha = 
            //crossRoadsImage.
            crossRoads.SetActive(true);
            StartCoroutine(FadeCoroutine());
        }
    }

    IEnumerator FadeCoroutine()
    {
        float fadeCount = 0;
        while(fadeCount < 1f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            crossRoadsGroup.alpha = fadeCount;
        }
        Destroy(crossRoads, 3f);

    }

}
