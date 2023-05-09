using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrapTrigger : MonoBehaviour
{
    public GameObject fallingTrap;
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //¡¢√À¥ÎªÛ Player∏È true
        if (collision.gameObject.tag == "Player")
        {
            fallingTrap.GetComponent<FallingTrap>().isTriggered = true;
        }
        else
        {
            return;
        }
    }    
}
