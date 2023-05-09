using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venge : Monster
{
    Vector3 originPos;
    
    public enum CurrentState { Idle, Trace, Attack, Damaged, Return,  Dead };
    CurrentState currentState;

    Animator anim;

    private Transform target;

    public float speed = 10f;   // 이동 속도
    public float moveDis = 30f; // 이동 가능 거리
    public float traceDis = 20f;    // 추적 거리
    public float attackDis = 0.05f;    // 공격 거리
    public float dist;
    public int attackPower = 1; // 파리의 공격력
    public int hp = 2;  // 목숨
    public int geo = 4;

    float currentTime = 0;
    float attackDelay = 1f;

    bool check = true;
    // Start is called before the first frame update
    void Start()
    {
        currentState = CurrentState.Idle;
        target = GameObject.Find("Player").transform;
        originPos = transform.position;

        anim = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x >= target.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if((transform.position.x < target.transform.position.x))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        
        dist = Vector3.Distance(target.transform.position, transform.position);
        switch (currentState)
        {
            case CurrentState.Idle:

                Idle();
                break;

            case CurrentState.Trace:
                Trace();
                break;
            case CurrentState.Attack:
                Attack();
                break;
            case CurrentState.Damaged:
                Damaged();
                break;
            case CurrentState.Dead:
                Die();
                break;
            case CurrentState.Return:
                Return();
                break;
        }

        // 만약 플레이어에게 두 번의 공격을 받으면 사망
        if (hp <= 0)
        {
            if (check)
            {
                GameManager.Instance.GeoRespawn(geo, gameObject);
                Die();
            }
        }
    }




    // 최초(기본)
    private void Idle()
    {
        
        if (dist <= traceDis)   // 만약 추적거리 안에 플레이어가 있으면
        {
            currentState = CurrentState.Trace;  // 추적 상태로

        }
    }

    // 추적 상태
    private void Trace()
    {
        // 공격 받으면 일정거리 밀려남

        if (dist > moveDis) // 만약 이동가능 거리를 벗어나면
        {
            currentState = CurrentState.Return; // 리턴 상태로
        }
        else if(dist > attackDis)    // 만약 공격거리 밖에 있으면
        {
            anim.SetTrigger("IdleToTrace");
            Vector3 dir = (target.transform.position - transform.position).normalized;  // 방향

            transform.position +=  dir * speed * Time.deltaTime;    // 플레이어를 향해 이동


        }
        else // 그렇지 않으면
        {
            currentState = CurrentState.Attack; // 공격 상태로
            currentTime = attackDelay;
        }
    }

    // 공격 상태
    private void Attack()
    {
        if(dist <= attackDis)   // 만약 공격거리 안에 있으면
        {
            // 일정 시간 간격으로 공격
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // 플레이어 스크립트의 damage 접근
                //target.GetComponent<스크립트명>().함수명(변수);
                target.GetComponent<PlayerController>().Hurt(attackPower);
                anim.SetTrigger("TraceToAttack");
                //transform.position = target.transform.position;
                currentTime = 0;
            }
        }
        else // 그렇지 않으면 현재상태를
        {
            currentState = CurrentState.Trace;  // 추적 상태로
            currentTime = 0;
        }
    }

    // 피격 상태 (플레이어에게 공격을 받으면)
    // hp--
    private void Damaged()
    {
        // 공격 받은 후(피격), 추적 상태로
        currentState = CurrentState.Trace;
    }

    public void AttackEnemy(int power)  // 플레이어가 파리 공격(파리가 공격 받았을 때)
    {
        if (currentState == CurrentState.Damaged || currentState == CurrentState.Dead || currentState == CurrentState.Return)
        {
            return;
        }
        hp -= power;
        if (hp > 0) // fly의 체력이 0보다 크면 피격 상태
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

    private void Return()
    {
        // 만약 초기 위치에서의 벗어나면 초기 위치로 이동
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dirReturn = (originPos - transform.position).normalized;
            transform.position += dirReturn * speed * Time.deltaTime;
        }
        else // 그렇지 않다면 위치를 초기 위치로 & 기본 상태로
        {
            transform.position = originPos;
            currentState = CurrentState.Idle;
        }
    }

    public override void Damaged(int damage)
    {
        //base.Damaged_1();
        this.hp--;
    }
    
}
