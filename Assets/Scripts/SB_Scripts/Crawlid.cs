using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 굼벵이(Crawlid)
// 굼벵이가 빈오브젝트(Boundary)에 부딪히면 회전
// 1. x축 이동
// 바닥을 왕복하며 기어다님, 일정거리 왕복(반복)
// => 왼쪽 이동(애니메이션), 회전(애니메이션), 벽에 부딪히면 회전(180도)
// 2. 회전
// 애니메이션
// 3. 사망
// 두 번 공격하면 날아가서 사망

public class Crawlid : Monster
{
    Rigidbody2D rb;
    public Animator anim;

    // 좌우로 이동
    // 속도
    public float speed = 5;
    public bool isEdge = false;
    public bool isDead = false;
    public float groundY;
    public int geo = 3;
    //hp
    public int hp=3;
    //공격력
    public int attackDamage=1;

    private Collider2D collilder;
    // 현재 위치
    float pos;
    bool check = true;
    // Start is called before the first frame update
    void Start()
    {
        groundY = transform.position.y;
        rb = transform.GetComponent<Rigidbody2D>();
        pos = transform.position.x;
        anim = transform.GetComponentInChildren<Animator>();
        //튕기는 현상 없애기
        rb.velocity = Vector2.zero;
        collilder = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        pos -= Time.deltaTime * speed;
        transform.position = new Vector2(pos, groundY);

        // 만약 플레이어에게 두 번의 공격을 받으면 사망
        if(hp <= 0)
        {
            if (check)
            {
                Die();
            }
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
            transform.position = new Vector2(pos, groundY);
            
            //transform.Rotate(0, -180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어와 접촉시 데미지
        if (collision.gameObject.tag.Equals("Player"))
        {
            //합칠때 카메라 쉐이크 추가
            Attack(collision.gameObject);
        }
    }

    // 사망
    // 죽으면서 지오(열매)를 2개 제공 & 뒤집어짐(애니메이션)
    private void Die()
    {
        // 죽을 때 애니메이션
        anim.SetTrigger("Die");
        //Geo 떨구기
        GameManager.Instance.GeoRespawn(geo, gameObject);
        
        check = false;
        //뒤집어주고 꺼줘서 체력 다는거 없애줘
        collilder.enabled = false;

        Destroy(gameObject, 1f);
    }

    //공격
    private void Attack(GameObject collision)
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);
        print("crawild attack");
    }

    //데미지 받음
    public Vector2 knuckBackPos;
    public float damageSpeed = 0.5f;
    public override void Damaged(int damage)
    {
        //base.Damaged_1();
        print("DamagedChild_PlayerATDamage" + damage);
        //플레이어의 공격력 만큼 crawild의 hp를 깎고
        this.hp = hp - attackDamage;

        //Enemy 넉백 구현..
    }
    
}
