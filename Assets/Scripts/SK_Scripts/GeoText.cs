using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeoText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject playerController;

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        print(text);

        text.text = playerController.GetComponent<PlayerController>().geo.ToString();

    }
}
