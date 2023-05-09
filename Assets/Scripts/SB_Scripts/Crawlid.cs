using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������(Crawlid)
// �����̰� �������Ʈ(Boundary)�� �ε����� ȸ��
// 1. x�� �̵�
// �ٴ��� �պ��ϸ� ���ٴ�, �����Ÿ� �պ�(�ݺ�)
// => ���� �̵�(�ִϸ��̼�), ȸ��(�ִϸ��̼�), ���� �ε����� ȸ��(180��)
// 2. ȸ��
// �ִϸ��̼�
// 3. ���
// �� �� �����ϸ� ���ư��� ���

public class Crawlid : Monster
{
    Rigidbody2D rb;
    public Animator anim;

    // �¿�� �̵�
    // �ӵ�
    public float speed = 5;
    public bool isEdge = false;
    public bool isDead = false;
    public float groundY;
    public int geo = 3;
    //hp
    public int hp=3;
    //���ݷ�
    public int attackDamage=1;

    private Collider2D collilder;
    // ���� ��ġ
    float pos;
    bool check = true;
    // Start is called before the first frame update
    void Start()
    {
        groundY = transform.position.y;
        rb = transform.GetComponent<Rigidbody2D>();
        pos = transform.position.x;
        anim = transform.GetComponentInChildren<Animator>();
        //ƨ��� ���� ���ֱ�
        rb.velocity = Vector2.zero;
        collilder = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        pos -= Time.deltaTime * speed;
        transform.position = new Vector2(pos, groundY);

        // ���� �÷��̾�� �� ���� ������ ������ ���
        if(hp <= 0)
        {
            if (check)
            {
                Die();
            }
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
            transform.position = new Vector2(pos, groundY);
            
            //transform.Rotate(0, -180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾�� ���˽� ������
        if (collision.gameObject.tag.Equals("Player"))
        {
            //��ĥ�� ī�޶� ����ũ �߰�
            Attack(collision.gameObject);
        }
    }

    // ���
    // �����鼭 ����(����)�� 2�� ���� & ��������(�ִϸ��̼�)
    private void Die()
    {
        // ���� �� �ִϸ��̼�
        anim.SetTrigger("Die");
        //Geo ������
        GameManager.Instance.GeoRespawn(geo, gameObject);
        
        check = false;
        //�������ְ� ���༭ ü�� �ٴ°� ������
        collilder.enabled = false;

        Destroy(gameObject, 1f);
    }

    //����
    private void Attack(GameObject collision)
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);
        print("crawild attack");
    }

    //������ ����
    public Vector2 knuckBackPos;
    public float damageSpeed = 0.5f;
    public override void Damaged(int damage)
    {
        //base.Damaged_1();
        print("DamagedChild_PlayerATDamage" + damage);
        //�÷��̾��� ���ݷ� ��ŭ crawild�� hp�� ���
        this.hp = hp - attackDamage;

        //Enemy �˹� ����..
    }
    
}
