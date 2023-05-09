using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충격파
// player direction으로 이동, 3초 후 destroy
// 충돌하면 플레이어에 데미지 입힘

public class WaveAttack : MonoBehaviour
{
    public Transform target;
    
    public float speed = 5f;
    public int attackDamage = 1; // 충격파 공격력

    float currentTime = 0;
    float destroyTime = 3f;

    Vector3 direction;

    void Start()
    {
        target = GameObject.Find("Foot").transform;

        direction = (new Vector3(target.transform.position.x, -64, 0) - transform.position).normalized;
        gameObject.SetActive(true);

    }

    void Update()
    {
        currentTime += Time.deltaTime;
        // 플레이어 방향으로 이동
        transform.position += direction * speed * Time.deltaTime;

        // 만약 3초가 지나면 destroy
        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
            currentTime = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Attack(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void Attack(GameObject collision)   //  충격파 공격, 플레이어 데미지
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);
    }
}
