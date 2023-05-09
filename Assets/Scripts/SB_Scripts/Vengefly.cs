using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Vengefly
// ����: idle, trace, attcak, dead
// �� �ݰ� ������(idle), �÷��̾ Ÿ������ ������ �� ������ ����(trace)
// ���� ��(dead)
public class Vengefly : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    public enum CurrentState { idle, trace, attack, dead };
    public CurrentState currentState = CurrentState.idle;

    private Transform target;
    private Transform vengefly;
    private NavMeshAgent nvAgent;

    public float traceDis = 10f;    // ���� �Ÿ�
    public float attackDis = 5f;    // ���� �Ÿ�
    public int hp = 2;  // ���

    private bool isDead = false;    // ���� �޾Ƽ� ������ true
    private bool isTrace = false;

/*    private bool hasTarget
    {
        get
        {
            float dist = Vector2.Distance(target.transform.position, transform.position);
            // ������ ��� ���� true
            if(dist <= traceDis)
            {
                return true;
            }
            // �׷��� ������ false
            return false;
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        //vengefly = this.gameObject.GetComponent<Transform>();
        // target ã��
        target = GameObject.Find("Player").transform;
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = transform.GetComponentInChildren<Animator>();

        StartCoroutine(this.CheckState());
        StartCoroutine(this.CheckStateForAction());
    }

    // target�� �ִ��� Ȯ��
    IEnumerator CheckState()
    {
        while (!isDead) // �ױ� ������ �ݺ�
        {
            /*// ����
            if (hasTarget)  // ���� target�� traceDis ���� ������ hasTarget == true
            {
                nvAgent.isStopped = false;
                nvAgent.SetDestination(target.transform.position);
            }
            // ���� ��� ����
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
            // ���� ����ڰ� zŰ�� ������
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

    // ���� ����
    void Attacked()
    {
        hp--;
    }

    // ���
    void Die()
    {
        isDead = true;
        anim.SetBool("Die", isDead);
        Destroy(gameObject);
    }
}
