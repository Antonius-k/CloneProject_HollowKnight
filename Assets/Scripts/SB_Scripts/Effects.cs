using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public float speed = 5;
    public int attackDamage = 1; // ����Ʈ ���ݷ�

    public Transform target;    // player
    public GameObject platform; // 
    Vector3 dir;


    void Start()
    {
        GameObject player = GameObject.Find("Player");

        if (player)
        {
            target = player.transform;
            //70 % Ȯ���� �Ʒ��� ������ ���
            // 1. Ȯ���� ���ؾ� �Ѵ�.
            int randomic = Random.Range(0, 10);

            // Ÿ�� �������� �����ϱ�
            // 2. Ȯ���� 70% �� �������ϱ�
            if (randomic < 7)
            {
                // 3. ������ �Ʒ��� �����ϰ� �ʹ�.
                dir = Vector3.down;
            }
            //�׷��������� 
            else
            {
                //Ÿ��������
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
        // ���� �÷��̾��
        if(collision.gameObject.name == "Player")
        {
            // �÷��̾� hp--
            Attack(collision.gameObject);

        }

        Destroy(gameObject);

    }

    private void Attack(GameObject collision)
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);

        print("����Ʈ�� attack");
    }
}
