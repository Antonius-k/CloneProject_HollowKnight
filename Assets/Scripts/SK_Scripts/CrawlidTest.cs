using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlidTest : Monster
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public Animator anim;

    // 좌우로 이동
    // 속도
    public float speed = 5;
    public bool isEdge = false;
    public bool isDead = false;
    public float groundY;
    public int hp = 3;

    // 현재 위치
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

        // 만약 플레이어에게 두 번의 공격을 받으면 사망
        if (hp <= 0)
        {
            Die();
        }
    }

    // 회전
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
            //데미지 줘야 댐
            Attack();


        }
        //데미지 줘야됨
        //else if(collision.gameObject.tag.)
    }

    // 사망
    // 죽으면서 지오(열매)를 2개 제공 & 뒤집어짐(애니메이션)
    private void Die()
    {
        // 죽을 때 애니메이션
        anim.SetTrigger("Die");
        print("Die");
        Destroy(gameObject, 1f);
    }

    //공격
    private void Attack()
    {
        print("crawild attack");
    }

    //override Test
    public override void Damaged(int damage)
    {
        print("일반상속");
        //부모(Monster)의 기본 데미지 함수
        //base.Damaged_1(); 갯셋
        hp--;
    }


    
}
