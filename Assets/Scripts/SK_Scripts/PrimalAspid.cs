using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrimalAspid : Monster
{
    #region PrimalAspid 속성
    public float speed = 2f;    // 벌레 속력
    public float moveDis = 10f; // 이동 가능 거리
    public float detectDis = 5f;    // 플레이어 탐지 거리
    public float attackDis = 2f;    // 공격 거리
    public int hp = 3;  // 목숨
    public int geo = 5; //지오
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

    #region Enemy 속성정의
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
        //에너미 플레이어 방향으로 전환
        float direction = target.position.x - transform.position.x;
        int enemyDir = direction > 0 ? -1 : direction < 0 ? 1 : 0;

        if(enemyDir != 0)
        {
            Vector3 vec3 = transform.localScale;
            vec3.x = enemyDir;
            transform.localScale = vec3;
        }
        //===================

        // 0 이하 사망
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

    //3초 뒤 Move전환
    public float currentTime;
    float idleDelayTime = 3f;
    private void Idle()
    {
        currentTime += Time.deltaTime;
        if (currentTime > idleDelayTime)
        {
            //3.상태를 이동으로 전환
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
        //돌아다니다가
        if (mustPatrol)
        {
            rigidbody.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
        }

        //플레이어가 감지되면
        Vector2 origin = transform.position;

        //detectDirection거리 안에 들어오면
        float radius = 10f;
        float distance = 1.5f;
        LayerMask layerMask = LayerMask.GetMask("Player") | LayerMask.GetMask("PlatForm") | LayerMask.GetMask("Enemy");

        //CircleCastAll origin(좌표) 위치에서 direction(방향)으로 distance(거리) 만큼 떨어져 있는 곳에
        //radius(반지름) 크기의  Circle이 있는데 이 Circle에 layerMask(타겟오브텍트의 레이어 이름)
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
        //멈춰서
        rigidbody.velocity = Vector2.zero;
        //발사!
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            // 현재 위치에서 쏜다.
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