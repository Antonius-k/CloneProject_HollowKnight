using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadly : MonoBehaviour
{
    public int damage=5;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

        if (layerName == "Player")
        {
            print("damage");
            PlayerController playerController = collision.collider.GetComponent<PlayerController>();
            playerController.Hurt(damage);
        }
        /*else if (layerName == "Enemy")
        {
            EnemyController enemyController = collision.collider.GetComponent<EnemyController>();
            enemyController.hurt(enemyController.health);
        }*/
    }
}
