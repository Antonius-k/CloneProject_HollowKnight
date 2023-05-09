using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Vengefly
// 상태: idle, trace, attcak, dead
// 입 닫고 날개짓(idle), 플레이어를 타겟으로 잡으면 입 벌리고 추적(trace)
// 죽을 때(dead)
public class Vengefly : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState currentState = CurrentState.idle;

    private Transform target;
    private Transform vengefly;
    private NavMeshAgent nvAgent;

    public float traceDis = 10f;    // 추적 거리
    public float attackDis = 5f;    // 공격 거리
    public int hp = 2;  // 목숨

    private bool isDead = false;    // 공격 받아서 죽으면 true
    private bool isTrace = false;

/*    private bool hasTarget
    {
        get
        {
            float dist = Vector2.Distance(target.transform.position, transform.position);
            // 추적할 대상 존재 true
            if(dist <= traceDis)
            {
                return true;
            }
            // 그렇지 않으면 false
            return false;
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        //vengefly = this.gameObject.GetComponent<Transform>();
        // target 찾기
        target = GameObject.Find("Player").transform;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = transform.GetComponentInChildren<Animator>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    // target이 있는지 확인
    IEnumerator CheckState()
    {
        while (!isDead) // 죽기 전까지 반복
        {
            /*// 추적
            if (hasTarget)  // 만약 target이 traceDis 내에 있으면 hasTarget == true
            {
                nvAgent.isStopped = false;
                nvAgent.SetDestination(target.transform.position);
            }
            // 추적 대상 없음
            else
            {
                nvAgent.isStopped = true;
            }*/
            float dist = Vector2.Distance(target.transform.position, transform.position);

            if(dist <= traceDis)
            {
                currentState = CurrentState.trace;
            }
            else if(dist <= attackDis)
            {
                currentState = CurrentState.attack;
            }
            else
            {
                currentState = CurrentState.idle;
            }
            yield return new WaitForSeconds(0.1f);

        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (currentState)
            {
                case CurrentState.idle:
                    nvAgent.isStopped = true;
                    break;
                case CurrentState.trace:
                    nvAgent.destination = target.transform.position;
                    anim.SetBool("Trace", isTrace);
                    break;
                case CurrentState.attack:

                    break;
            }
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        while (true)
        {
            // 만약 사용자가 z키를 누르면
            if (Input.GetKey(KeyCode.Z))
            {
                Attacked();
            }
            if (hp == 0)
            {
                Die();
                break;
            }
        }

    }

    // 공격 받음
    void Attacked()
    {
        hp--;
    }

    // 사망
    void Die()
    {
        isDead = true;
        anim.SetBool("Die", isDead);
        Destroy(gameObject);
    }
}
