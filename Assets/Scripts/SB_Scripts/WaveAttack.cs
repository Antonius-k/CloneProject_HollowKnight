using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����
// player direction���� �̵�, 3�� �� destroy
// �浹�ϸ� �÷��̾ ������ ����

public class WaveAttack : MonoBehaviour
{
    public Transform target;
    
    public float speed = 5f;
    public int attackDamage = 1; // ����� ���ݷ�

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
        // �÷��̾� �������� �̵�
        transform.position += direction * speed * Time.deltaTime;

        // ���� 3�ʰ� ������ destroy
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

    private void Attack(GameObject collision)   //  ����� ����, �÷��̾� ������
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);
    }
}
