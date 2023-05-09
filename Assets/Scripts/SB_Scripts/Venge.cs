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

    public float speed = 10f;   // �̵� �ӵ�
    public float moveDis = 30f; // �̵� ���� �Ÿ�
    public float traceDis = 20f;    // ���� �Ÿ�
    public float attackDis = 0.05f;    // ���� �Ÿ�
    public float dist;
    public int attackPower = 1; // �ĸ��� ���ݷ�
    public int hp = 2;  // ���
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

        // ���� �÷��̾�� �� ���� ������ ������ ���
        if (hp <= 0)
        {
            if (check)
            {
                GameManager.Instance.GeoRespawn(geo, gameObject);
                Die();
            }
        }
    }




    // ����(�⺻)
    private void Idle()
    {
        
        if (dist <= traceDis)   // ���� �����Ÿ� �ȿ� �÷��̾ ������
        {
            currentState = CurrentState.Trace;  // ���� ���·�

        }
    }

    // ���� ����
    private void Trace()
    {
        // ���� ������ �����Ÿ� �з���

        if (dist > moveDis) // ���� �̵����� �Ÿ��� �����
        {
            currentState = CurrentState.Return; // ���� ���·�
        }
        else if(dist > attackDis)    // ���� ���ݰŸ� �ۿ� ������
        {
            anim.SetTrigger("IdleToTrace");
            Vector3 dir = (target.transform.position - transform.position).normalized;  // ����

            transform.position +=  dir * speed * Time.deltaTime;    // �÷��̾ ���� �̵�


        }
        else // �׷��� ������
        {
            currentState = CurrentState.Attack; // ���� ���·�
            currentTime = attackDelay;
        }
    }

    // ���� ����
    private void Attack()
    {
        if(dist <= attackDis)   // ���� ���ݰŸ� �ȿ� ������
        {
            // ���� �ð� �������� ����
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // �÷��̾� ��ũ��Ʈ�� damage ����
                //target.GetComponent<��ũ��Ʈ��>().�Լ���(����);
                target.GetComponent<PlayerController>().Hurt(attackPower);
                anim.SetTrigger("TraceToAttack");
                //transform.position = target.transform.position;
                currentTime = 0;
            }
        }
        else // �׷��� ������ ������¸�
        {
            currentState = CurrentState.Trace;  // ���� ���·�
            currentTime = 0;
        }
    }

    // �ǰ� ���� (�÷��̾�� ������ ������)
    // hp--
    private void Damaged()
    {
        // ���� ���� ��(�ǰ�), ���� ���·�
        currentState = CurrentState.Trace;
    }

    public void AttackEnemy(int power)  // �÷��̾ �ĸ� ����(�ĸ��� ���� �޾��� ��)
    {
        if (currentState == CurrentState.Damaged || currentState == CurrentState.Dead || currentState == CurrentState.Return)
        {
            return;
        }
        hp -= power;
        if (hp > 0) // fly�� ü���� 0���� ũ�� �ǰ� ����
        {
            currentState = CurrentState.Damaged;
        }        
        else // �׷��� �ʴٸ� ���
        {
            currentState = CurrentState.Dead;
        }

    }

    // ��� & ���� 3��
    private void Die()
    {
        // ���� �� �ִϸ��̼�
        anim.SetTrigger("Die");
        print("Die");
        Destroy(gameObject, 1f);
        check = false;


    }

    private void Return()
    {
        // ���� �ʱ� ��ġ������ ����� �ʱ� ��ġ�� �̵�
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dirReturn = (originPos - transform.position).normalized;
            transform.position += dirReturn * speed * Time.deltaTime;
        }
        else // �׷��� �ʴٸ� ��ġ�� �ʱ� ��ġ�� & �⺻ ���·�
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
