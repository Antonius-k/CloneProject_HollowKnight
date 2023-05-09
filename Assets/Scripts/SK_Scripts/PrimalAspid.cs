using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrimalAspid : Monster
{
    #region PrimalAspid �Ӽ�
    public float speed = 2f;    // ���� �ӷ�
    public float moveDis = 10f; // �̵� ���� �Ÿ�
    public float detectDis = 5f;    // �÷��̾� Ž�� �Ÿ�
    public float attackDis = 2f;    // ���� �Ÿ�
    public int hp = 3;  // ���
    public int geo = 5; //����
    public Vector2 detectDirection;
    public float detectDistance;

    Transform target;

    public float shootInterval;
    public GameObject paBullet;
    
    private Transform playerTransform;
    private Transform transform;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    #endregion


    bool check = true;
    //patrol
    public bool mustPatrol;

    #region Enemy �Ӽ�����
    enum EnemyState
    {
        Idle,
        Move,
        Die,
    }
    EnemyState m_State = EnemyState.Idle;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();

        transform = gameObject.GetComponent<Transform>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        mustPatrol = true;
        
        m_State = EnemyState.Move;
        
    }

    // Update is called once per frame
    void Update()
    {
        //���ʹ� �÷��̾� �������� ��ȯ
        float direction = target.position.x - transform.position.x;
        int enemyDir = direction > 0 ? -1 : direction < 0 ? 1 : 0;

        if(enemyDir != 0)
        {
            Vector3 vec3 = transform.localScale;
            vec3.x = enemyDir;
            transform.localScale = vec3;
        }
        //===================

        // 0 ���� ���
        if (hp <= 0)
        {
            if (check)
            {
                m_State = EnemyState.Die;
                
            }
        }

        print("State : " + m_State);
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move(detectDirection);
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    //3�� �� Move��ȯ
    public float currentTime;
    float idleDelayTime = 3f;
    private void Idle()
    {
        currentTime += Time.deltaTime;
        if (currentTime > idleDelayTime)
        {
            //3.���¸� �̵����� ��ȯ
            m_State = EnemyState.Move;
            currentTime = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Boundary"))
        {
            walkSpeed = -walkSpeed;
        }
    }

    public float walkSpeed = 50;
    private void Move(Vector2 detectDirection)
    {
        //���ƴٴϴٰ�
        if (mustPatrol)
        {
            rigidbody.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }

        //�÷��̾ �����Ǹ�
        Vector2 origin = transform.position;

        //detectDirection�Ÿ� �ȿ� ������
        float radius = 10f;
        float distance = 1.5f;
        LayerMask layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("PlatForm") | LayerMask.GetMask("Enemy");

        //CircleCastAll origin(��ǥ) ��ġ���� direction(����)���� distance(�Ÿ�) ��ŭ ������ �ִ� ����
        //radius(������) ũ����  Circle�� �ִµ� �� Circle�� layerMask(Ÿ�ٿ�����Ʈ�� ���̾� �̸�)
        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);
        
        foreach(RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;
            string layerName = LayerMask.LayerToName(obj.layer);

            if(layerName == "Player")
            {
                Attact();
            }
            else
            {
                mustPatrol = true;
            }
        }
    }

    private void Die()
    {
        check = false;

        Destroy(gameObject);
        //StartCoroutine(GameManager.Instance.GeoLastT(geo, gameObject));
        GameManager.Instance.GeoRespawn(geo, gameObject);
    }
    public float attackDelayTime = 3f;
    public void Attact()
    {
        mustPatrol = false;
        //���缭
        rigidbody.velocity = Vector2.zero;
        //�߻�!
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            // ���� ��ġ���� ���.
            GameObject bullet = Instantiate(paBullet, transform.position, Quaternion.identity);
            currentTime = 0;

            Destroy(bullet, 5f);
        }
    }

    public override void Damaged(int damage)
    {
        this.hp--;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10f);
        Gizmos.DrawLine(transform.position, transform.position * 5f);
    }
}