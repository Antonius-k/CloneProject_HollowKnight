using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoRotate : MonoBehaviour
{
    public float rotationSpeed = 300f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime * 100, 0f);
    }
}
