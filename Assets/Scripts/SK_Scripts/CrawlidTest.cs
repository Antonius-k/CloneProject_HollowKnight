using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlidTest : Monster
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public Animator anim;

    // �¿�� �̵�
    // �ӵ�
    public float speed = 5;
    public bool isEdge = false;
    public bool isDead = false;
    public float groundY;
    public int hp = 3;

    // ���� ��ġ
    float pos;

    // Start is called before the first frame update
    void Start()
    {
        groundY = transform.position.y;
        rb = transform.GetComponent<Rigidbody2D>();
        pos = transform.position.x;
        anim = transform.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        pos -= Time.deltaTime * speed;
        transform.position = new Vector3(pos, groundY, 0);

        // ���� �÷��̾�� �� ���� ������ ������ ���
        if (hp <= 0)
        {
            Die();
        }
    }

    // ȸ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Boundary"))
        {
            anim.SetTrigger("Rotate");

            speed = -speed;
            pos -= Time.deltaTime * speed;
            transform.position = new Vector3(pos, groundY, 0);

            //transform.Rotate(0, -180, 0);

        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
            //������ ��� ��
            Attack();


        }
        //������ ��ߵ�
        //else if(collision.gameObject.tag.)
    }

    // ���
    // �����鼭 ����(����)�� 2�� ���� & ��������(�ִϸ��̼�)
    private void Die()
    {
        // ���� �� �ִϸ��̼�
        anim.SetTrigger("Die");
        print("Die");
        Destroy(gameObject, 1f);
    }

    //����
    private void Attack()
    {
        print("crawild attack");
    }

    //override Test
    public override void Damaged(int damage)
    {
        print("�Ϲݻ��");
        //�θ�(Monster)�� �⺻ ������ �Լ�
        //base.Damaged_1(); ����
        hp--;
    }


    
}
