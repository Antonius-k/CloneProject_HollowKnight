using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구부정하게 걷다가 플레이어가 일정 범위 안에 들어오면
// 1. 플레이어 방향으로 몸을 돌림
// 2. 허리를 핌
// Idle, Attack, Damaged, Return, Dead
public class HuskBully : Monster
{
    Animator anim;
    Vector3 originPos;

    public enum CurrentState
    {
        Idle,
        Rotate,
        Trace,
        Damaged,
        Dead
    };

    CurrentState currentState;

    private Transform target;

    public float speed = 2f;    // 벌레 속력
    public float moveDis = 10f; // 이동 가능 거리
    public float detectDis = 5f;    // 플레이어 탐지 거리
    public float attackDis = 2f;    // 공격 거리
    public float attackSpeed = 5f;
    public float dist;

    public int attackPower = 2; // 벌레의 공격력
    public int hp = 3;  // 목숨
    public int geo = 5; //지오


    float rotation_1;
    float rotation_2;

    bool check = true;

    Vector3 dir;

    /*    float currentTime = 0;
        float attackDelay = 0.1f;*/


    void Start()
    {
        currentState = CurrentState.Idle;
        target = GameObject.Find("Player").transform;
        originPos = transform.position;
        dir = Vector3.left;

        anim = transform.GetComponentInChildren<Animator>();

    }

    void Update()
    {
        // 일단 걷기
        // 만약 플레이어가 탐지거리 안에 들어오면 공격 상태로 전환
        // 공격 상태 - 플레이어를 향해 돌진(일정거리만큼 달려감), 몸통 박치기
        // 만약 플레이어가 공격하면 데미지 상태로 전환
        // 만약 플레이어가 일정 거리를 벗어나면 Idle로

        // 플레이어(타겟)와 벌레 사이의 거리
        dist = Vector3.Distance(target.transform.position, transform.position);

        if (hp <= 0)
        {
            if (check)
            {
                GameManager.Instance.GeoRespawn(geo, gameObject);
                currentState = CurrentState.Dead;
            }
        }

        switch (currentState)
        {
            case CurrentState.Idle:
                Idle();
                break;
            case CurrentState.Rotate:
                Rotate();
                break;
            case CurrentState.Trace:
                Trace();
                break;
            case CurrentState.Damaged:
                Damaged();
                break;
            case CurrentState.Dead:
                Die();
                break;
        }
    }

    private void Rotate()
    {
        print("Rotate");

        // 만약 rotation 값 비교
        if (rotation_1 != rotation_2)    // 다르면
        {
            currentState = CurrentState.Idle;
        }
    }

    // 회전
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Boundary"))
        {
            transform.localScale = new Vector3(-rotation_1, 1, 1);
            rotation_2 = transform.localScale.x;
            currentState = CurrentState.Rotate;
            dir = -dir;
        }
    }

    private void Idle()
    {
        print("Idle");
        rotation_1 = transform.localScale.x;
        anim.SetTrigger("Idle");
        // 이동
        transform.position += speed * dir * Time.deltaTime;

        if (dist <= detectDis) // 만약 플레이어가 탐지거리 내에 있으면
        {
            originPos = transform.position; // 그 위치를 originPos 로 지정
            // 플레이어 쪽으로 회전 후, 공격 상태로
            currentState = CurrentState.Trace;
        }
    }

    private void Trace()
    {
        // 탐지거리 안에 있으면 플레이어를 바라봄
        anim.SetTrigger("Trace");
        if (transform.position.x >= target.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (dist <= detectDis)   // 만약 탐지거리 안에 있으면
        {

            if (dist <= attackDis)   // 만약 공격거리 안에 있으면
            {
                anim.SetTrigger("Attack");
                // 플레이어를 향해 돌진(일정거리만큼 달려감), 몸통 박치기
                Vector3 dir = (target.transform.position - transform.position).normalized;  // 방향
                transform.position += dir * attackSpeed * Time.deltaTime;    // 플레이어를 향해 이동
                print("Attack");

            }
            else // 만약 공격거리를 벗어나면
            {
                // 공격 전의 위치로 이동
                //transform.position = originPos; 
                Vector3.Lerp(transform.position, originPos, 0.5f);
                print("돌아감");
                // Idle 상태로
                currentState = CurrentState.Idle;

            }
        }
        else
        {
            // Idle 상태로
            currentState = CurrentState.Idle;
            //currentTime = 0;

        }

    }

    private void Attack(GameObject collision)
    {
        collision.GetComponent<PlayerController>().Hurt(attackPower);

        print("벌레가 attack");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //플레이어와 접촉시 데미지
        if (collision.gameObject.tag.Equals("Player"))
        {
            print("111");
            //검과 접촉하면
            if (collision.gameObject.tag.Equals("Sword"))
            {
                //print(222222);
                //Damaged_1();
            }
            else
            {
                Attack(collision.gameObject);
            }
        }
    }

    private void Damaged()
    {
        // 플레이어의 검에 맞으면
        // hp--
    }

    public override void Damaged(int damage)
    {
        print("override_D1");
        anim.SetTrigger("Damaged");
        //부모(Monster)의 기본 데미지 함수
        //base.Damaged_1();
        this.hp--;
        currentState = CurrentState.Idle;
    }



    public void AttackEnemy(int power)  // 플레이어가 벌레 공격(벌레가 공격 받았을 때)
    {
        if (currentState == CurrentState.Damaged || currentState == CurrentState.Dead)
        {
            return;
        }
        hp -= power;
        if (hp > 0) // 벌레의 체력이 0보다 크면 피격 상태
        {
            currentState = CurrentState.Damaged;
        }
        else // 그렇지 않다면 사망
        {
            currentState = CurrentState.Dead;
        }

    }


    // 사망 & 지오 3개
    private void Die()
    {
        // 죽을 때 애니메이션
        anim.SetTrigger("Die");
        print("Die");
        Destroy(gameObject, 1f);
        check = false;

    }

}


/*// 일정 시간 간격으로 공격
currentTime += Time.deltaTime;
if (currentTime > attackDelay)
{
    // 공격 애니메이션
    Vector3 dir = (target.transform.position - transform.position).normalized;  // 방향
    transform.position += attackSpeed * dir * Time.deltaTime;
    // 플레이어 스크립트의 damage 접근
    //target.GetComponent<스크립트명>().함수명(변수);

    currentTime = 0;
}*/
