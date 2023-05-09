using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������ϰ� �ȴٰ� �÷��̾ ���� ���� �ȿ� ������
// 1. �÷��̾� �������� ���� ����
// 2. �㸮�� ��
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

    public float speed = 2f;    // ���� �ӷ�
    public float moveDis = 10f; // �̵� ���� �Ÿ�
    public float detectDis = 5f;    // �÷��̾� Ž�� �Ÿ�
    public float attackDis = 2f;    // ���� �Ÿ�
    public float attackSpeed = 5f;
    public float dist;

    public int attackPower = 2; // ������ ���ݷ�
    public int hp = 3;  // ���
    public int geo = 5; //����


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
        // �ϴ� �ȱ�
        // ���� �÷��̾ Ž���Ÿ� �ȿ� ������ ���� ���·� ��ȯ
        // ���� ���� - �÷��̾ ���� ����(�����Ÿ���ŭ �޷���), ���� ��ġ��
        // ���� �÷��̾ �����ϸ� ������ ���·� ��ȯ
        // ���� �÷��̾ ���� �Ÿ��� ����� Idle��

        // �÷��̾�(Ÿ��)�� ���� ������ �Ÿ�
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

        // ���� rotation �� ��
        if (rotation_1 != rotation_2)    // �ٸ���
        {
            currentState = CurrentState.Idle;
        }
    }

    // ȸ��
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
        // �̵�
        transform.position += speed * dir * Time.deltaTime;

        if (dist <= detectDis) // ���� �÷��̾ Ž���Ÿ� ���� ������
        {
            originPos = transform.position; // �� ��ġ�� originPos �� ����
            // �÷��̾� ������ ȸ�� ��, ���� ���·�
            currentState = CurrentState.Trace;
        }
    }

    private void Trace()
    {
        // Ž���Ÿ� �ȿ� ������ �÷��̾ �ٶ�
        anim.SetTrigger("Trace");
        if (transform.position.x >= target.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (dist <= detectDis)   // ���� Ž���Ÿ� �ȿ� ������
        {

            if (dist <= attackDis)   // ���� ���ݰŸ� �ȿ� ������
            {
                anim.SetTrigger("Attack");
                // �÷��̾ ���� ����(�����Ÿ���ŭ �޷���), ���� ��ġ��
                Vector3 dir = (target.transform.position - transform.position).normalized;  // ����
                transform.position += dir * attackSpeed * Time.deltaTime;    // �÷��̾ ���� �̵�
                print("Attack");

            }
            else // ���� ���ݰŸ��� �����
            {
                // ���� ���� ��ġ�� �̵�
                //transform.position = originPos; 
                Vector3.Lerp(transform.position, originPos, 0.5f);
                print("���ư�");
                // Idle ���·�
                currentState = CurrentState.Idle;

            }
        }
        else
        {
            // Idle ���·�
            currentState = CurrentState.Idle;
            //currentTime = 0;

        }

    }

    private void Attack(GameObject collision)
    {
        collision.GetComponent<PlayerController>().Hurt(attackPower);

        print("������ attack");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //�÷��̾�� ���˽� ������
        if (collision.gameObject.tag.Equals("Player"))
        {
            print("111");
            //�˰� �����ϸ�
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
        // �÷��̾��� �˿� ������
        // hp--
    }

    public override void Damaged(int damage)
    {
        print("override_D1");
        anim.SetTrigger("Damaged");
        //�θ�(Monster)�� �⺻ ������ �Լ�
        //base.Damaged_1();
        this.hp--;
        currentState = CurrentState.Idle;
    }



    public void AttackEnemy(int power)  // �÷��̾ ���� ����(������ ���� �޾��� ��)
    {
        if (currentState == CurrentState.Damaged || currentState == CurrentState.Dead)
        {
            return;
        }
        hp -= power;
        if (hp > 0) // ������ ü���� 0���� ũ�� �ǰ� ����
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

}


/*// ���� �ð� �������� ����
currentTime += Time.deltaTime;
if (currentTime > attackDelay)
{
    // ���� �ִϸ��̼�
    Vector3 dir = (target.transform.position - transform.position).normalized;  // ����
    transform.position += attackSpeed * dir * Time.deltaTime;
    // �÷��̾� ��ũ��Ʈ�� damage ����
    //target.GetComponent<��ũ��Ʈ��>().�Լ���(����);

    currentTime = 0;
}*/
