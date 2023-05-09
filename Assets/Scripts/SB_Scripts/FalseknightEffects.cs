using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스가 공격할 때 떨어지는 효과
// 보스가 공격할 때 계속(무한 생성) 떨어지고 싶다
// -> 세 지점에서 일정 시간 간격으로 무한 생성
// 언제 떨어져?! - 보스가 공격할 때
// 바닥에 닿으면 사라짐 & 이펙트 있음

public class FalseknightEffects : Monster
{
    int realityCount = 0;   // 실체 얼굴이 나오는 횟수, 1회 나오면 Dead 상태 -> 찐 실체 등장

    // *보스 공격 이펙트 관련
    public float destroyDelay = 2f;
    //Trigger check 
    public bool isTriggered;
    public bool isFalling;
    public float createTime = 2;    // 이펙트 생성
    float currentEffectsTime = 0;   // 현재 시각

    public GameObject [] EffectGOD = new GameObject[4];
    public GameObject effectFactory1;    // 이펙트 공장

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }


    // **보스 움직임 & 공격 관련
    Rigidbody2D rb;
    Animator anim;

    private Transform target;   // 플레이어를 타겟으로
    private GameObject checkPlayerPos;

    public float jumpPower = 1; // 점프 힘
    public float jumptime = 3f; // 점프 시간 간격
    public float attackTime = 1f; // 공격 시간
    public float stateChangeTime = 3f;  // 상태 변화 시간 간격
    public int hp = 30; // False Knight 체력 30
    public int attackDamage = 2; // False Knight 공격력
    public int geo;


    float currentTime;  // 현재 시각
    float stateCurrentTime; // 상태 시각
    float moveCurrentTime;  // 움직임 시각

    bool check = true;
    //bool checkPlayer = false;   // 플레이어가 CheckPlayer를 밟았는지 확인
    bool isGround = false;  // 보스가 땅을 밟았는지 확인

    Vector3 originPosition; // 처음 위치

    //파티클
    public GameObject dieEffect; //으앙쥬금

    public GameObject wave;
    // 보스 상태
    public enum enemyState
    {
        BeforeFalling,
        Idle,
        Jump,   // 한쪽
        Attack2,    // 양쪽
        Damaged,
        Dead, 
        RealityFace,    // 실체 얼굴
        Reality,    // 실체
    };

    public enemyState m_state = enemyState.BeforeFalling;

    //사운드 좀 실례하겠습니다.
    private AudioSource sfxBoss;
    public AudioClip[] sound;

    private void Start()
    {
        //사운드
        sfxBoss = GetComponent<AudioSource>();

        // *보스 이펙트 관련
        //Rigidbody = Effect.GetComponent<Rigidbody2D>();
        //Rigidbody.gravityScale = 0;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        isTriggered = false;    // 플레이어가 특정 지점을 밟았을 때 체크를 위함 -> 보스가 무언가를 하면 떨어짐
        isFalling = false;  // 보스가 공격을 함, 떨어지는 것 확인
        
        // **보스 움직임 & 공격 관련
        target = GameObject.Find("Player").transform;
        checkPlayerPos = GameObject.Find("CheckPlayer");

        rb = GetComponent<Rigidbody2D>();
        originPosition = transform.position;    // 초기 위치 저장
        anim = transform.GetComponentInChildren<Animator>();

        //waveAttack.SetActive(false);
        dustEffect.SetActive(false);
    }

    GameObject[] effect = new GameObject[4];
    private void Update()
    {
        // *보스 공격 이펙트

        if (isGround)
        {
            if(m_state == enemyState.RealityFace)
            {
                isGround = false;
            }
            else
            {
                currentEffectsTime += Time.deltaTime;
                if (currentEffectsTime > createTime)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        effect[i] = Instantiate(effectFactory1);
                        effect[i].transform.position = EffectGOD[i].transform.position;
                    }
                    currentEffectsTime = 0;
                }
            }

        }
        
        // **보스 움직임
        
        if (hp <= 0)
        {
            if (check)
            {
                print(check);
                Die();
                m_state = enemyState.Dead;
            }
        }

        if(hp <= 25)        // 만약 hp 가 25 이하이면 FakeDead 상태로, FakeDead 상태에서 RealityFace 로 보냄
        {
            m_state = enemyState.RealityFace;
            print("여기 안돼?");
            // 데미지 -2
            if(hp <= 15)
            {
                m_state = enemyState.Jump;
                if(hp <= 10)
                {
                    m_state = enemyState.RealityFace;
                }
            }

        }

        if (transform.position.x >= target.transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if ((transform.position.x < target.transform.position.x))
        {
            transform.localScale = new Vector3(2, 2, 2);
        }

        switch (m_state)
        {
            case enemyState.BeforeFalling:   // 기본 상태
                BeforeFalling();
                break;
            case enemyState.Idle:   // 기본 상태
                Idle();
                break;
            case enemyState.Jump:   // 공격1
                Jump();
                break;
            case enemyState.Attack2:   // 공격2 상태
                Attack2();
                break;
            case enemyState.Damaged:   // 
                break;
            case enemyState.Dead:   // 
                break;
            case enemyState.RealityFace:   // 실체 얼굴만
                RealityFace();
                break;
            case enemyState.Reality:   // 
                Reality();
                break;
        }
        currentTime += Time.deltaTime;
        stateCurrentTime += Time.deltaTime;
        moveCurrentTime += Time.deltaTime;
    }


    // **보스 움직임
    private void BeforeFalling()    // 떨어지기 전, 아무것도 안함
    {
        // 플레이어가 CheckPlayer를 밟으면,
        // isGround 체크해서 true 면 Idle 상태로
        if (isGround)
        {
            // 땅에 도착하면 이펙트 & 카메라 흔들
            Dust(); // 이펙트
            m_state = enemyState.Idle;
        }
    }

    public GameObject reward;   // 도시의 문장(부적, 보상)
    public GameObject rewardFactory;
    Vector3 bossDiePos;
    private void Die()
    {
        anim.enabled = true;
        anim.SetTrigger("Die");
        print("Die");

        check = false;
        isFalling = false;
        GameManager.Instance.GeoRespawn(geo, gameObject);

        bossDiePos = transform.position;
        Vector2 origin = transform.position;
        Destroy(gameObject, 2f);

        // 부적 생성
        reward = Instantiate(rewardFactory);
        reward.transform.position = bossDiePos;

        Instantiate(dieEffect, origin, Quaternion.Euler(Vector2.zero));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //플레이어와 접촉시 데미지
        if (collision.gameObject.tag.Equals("Player"))
        {
            CameraShake.Instance.PlayCameraShake();
            Attack(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 보스가 땅에 닿았는지 확인, 땅에 닿으면 돌맹이(이펙트) 떨구기 위함
        if (collision.gameObject.name == "CheckGround")
        {
            isGround = true;
        }
    }

    private void Attack(GameObject collision)   // 보스 공격, 플레이어 데미지
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);
    }

    private void Idle()
    {
        anim.enabled = true;

        isFalling = false;
        // 만약 현재 시각이 더 크면
        if (stateCurrentTime > stateChangeTime)
        {
            //dustEffect.SetActive(false);

            float stateRandom = Random.Range(0, 10);
            if (stateRandom < 5)
            {
                m_state = enemyState.Jump;  // 한방향 공격(공격1)
            }
            else
            {
                m_state = enemyState.Attack2;    // 양방향 공격(공격2)
            }
            stateCurrentTime = 0;

        }
    }
    public float xDir;  // 보스 움직임 방향 (1, -1)
    //Vector3 dir;
    public float speed = 5; // 보스 이동 속력
    // 점프 함수
    private void Jump() // 공격1
    {
        anim.enabled = true;

        if (currentTime > jumptime)
        {
            isFalling = true;

            float random = Random.Range(0, 10);
            if (random < 5)
            {
                xDir = +1;
                //print("R");
                // 공격 애니메이션
                anim.SetTrigger("Attack1");

                // 랜덤으로 충격파 나올지 안나올지 - 무조건 나오는 걸로,,
                sfxBoss.PlayOneShot(sound[1]);

            }
            else
            {
                xDir = -1;
                //print("L");
                // 공격 애니메이션
                anim.SetTrigger("Attack1");
                // 랜덤으로 충격파 나올지 안나올지 - 무조건 나오는 걸로,,
                sfxBoss.PlayOneShot(sound[2]);

            }
            rb.AddForce(new Vector3(10 * xDir, 50, 0) * jumpPower, ForceMode2D.Impulse);
            //print("Jump");
            currentTime = 0;
        }

        m_state = enemyState.Idle;
    }

    public override void Damaged(int damage)    // 보스 데미지, 플레이어가 공격
    {
        if(m_state == enemyState.RealityFace)
        {
            sfxBoss.PlayOneShot(sound[4]);
            this.hp -= 2;
            spriteRenderer.sprite = sprites[1];
            print("또 때려?");
        }
        else
        {
            sfxBoss.PlayOneShot(sound[4]);
            this.hp--;
            print("보스 hp 1 감소");
        }
        print("override_D1");
        //부모(Monster)의 기본 데미지 함수
        //base.Damaged_1();
    }

    // 공격 함수
    // 오/왼 랜덤으로 1회 점프 -> 공격
    // Random
    private void Attack2()   // 공격2
    {
        anim.enabled = true;

        //print("Attack");
        if (currentTime > attackTime)
        {
            isFalling = true;

            float random = Random.Range(0, 10);
            if (random < 5)
            {
                sfxBoss.PlayOneShot(sound[3]);

                xDir = +1;
                //print("R");
                //anim.SetTrigger("IdleToAttack");

                // 공격 애니메이션 (공격2)
            }
            else
            {
                xDir = -1;
                //print("L");
                //anim.SetTrigger("IdleToAttack");

                // 공격 애니메이션 (공격2)
            }
            rb.AddForce(new Vector3(10 * xDir, 50, 0) * jumpPower, ForceMode2D.Impulse);
            //print("Jump");
            currentTime = 0;
        }

        m_state = enemyState.Idle;
    }

    // 실체 얼굴, 일정 횟수 공격 당하면 다시 Idle 상태로 전환
    public Sprite[] sprites = new Sprite[2];   // 배열 이용 - 0이 idle / 1이 으악
    private void RealityFace()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 1f)
        {
            anim.enabled = false;
            spriteRenderer.sprite = sprites[0]; // 실체 얼굴의 기본 상태로 스프라이트 변경

            realityCount++; // 실체 얼굴 횟수 1 증가
            currentTime = 0;
        }

        //만약 hp 가 4 이하면 실체가 옷에서 분리(애니메이션)

/*        if (hp <= 4)
        {

        }*/

    }



    // 실체 - 필요없을듯!
    private void Reality()
    {
        anim.enabled = false;

    }


    //public GameObject FKAttackEffect;
    float waveTime = 5f;

    // ** 충격파 ** //
    private void WaveAttack()
    {
        print("WAVE");
        wave.SetActive(true);

        // 좌우 반전 & 크기
        if (transform.position.x >= target.transform.position.x)
        {
            wave.transform.localScale = new Vector3(-2, 2, 2);
        }
        else if ((transform.position.x < target.transform.position.x))
        {
            wave.transform.localScale = new Vector3(2, 2, 2);
        }
        Instantiate(wave, transform.position, Quaternion.Euler(Vector2.zero));

    }


    // *** Effect 에 관한 것들 *** //
    // 떨어질 때 먼지 이펙트 : 처음 / 점프 후
    public GameObject dustEffect;
    //float effectTime = 3f;
    private void Dust()
    {
        print("DUST");
        // 생성 위치 설정 (y는 -64로 고정)
        dustEffect.SetActive(true);
        GameObject dust = Instantiate(dustEffect, transform.position, Quaternion.Euler(Vector2.zero));
        //dustEffect.transform.position = transform.position; // 일단은 요렇게
        Destroy(dust, 1f);
    }

    // 철퇴 내려쳤을 때 이펙트, 위치 조정 - 아 이걸 애니메이션에서 조절하네
    private void DustAttack()
    {
        float bossPosX = transform.position.x;
        dustEffect.SetActive(true);

        Vector2 pos;
        // 생성 위치 조절
        if (transform.position.x >= target.transform.position.x)
        {
            pos = new Vector2(bossPosX-5, -62);
            Instantiate(dustEffect, pos, Quaternion.Euler(Vector2.zero));
        }
        else if ((transform.position.x < target.transform.position.x))
        {
            pos = new Vector2(bossPosX + 5, -62);
            Instantiate(dustEffect, pos, Quaternion.Euler(Vector2.zero));

        }

    }

}
