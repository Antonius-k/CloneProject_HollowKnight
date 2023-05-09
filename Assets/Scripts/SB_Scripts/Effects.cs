using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public float speed = 5;
    public int attackDamage = 1; // 이펙트 공격력

    public Transform target;    // player
    public GameObject platform; // 
    Vector3 dir;


    void Start()
    {
        GameObject player = GameObject.Find("Player");

        if (player)
        {
            target = player.transform;
            //70 % 확률로 아래로 방향을 잡고
            // 1. 확률을 구해야 한다.
            int randomic = Random.Range(0, 10);

            // 타겟 방향으로 설정하기
            // 2. 확률이 70% 에 속했으니까
            if (randomic < 7)
            {
                // 3. 방향을 아래로 설정하고 싶다.
                dir = Vector3.down;
            }
            //그렇지않으면 
            else
            {
                //타겟쪽으로
                dir = target.position - transform.position;
                dir.Normalize();
            }

        }
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 만약 플레이어면
        if(collision.gameObject.name == "Player")
        {
            // 플레이어 hp--
            Attack(collision.gameObject);

        }

        Destroy(gameObject);

    }

    private void Attack(GameObject collision)
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);

        print("이팩트가 attack");
    }
}
