using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region ask
    public int hp; //체력
    public int geo; //지오 갯수
    public int attackDamage=1;

    public float moveSpeed; //이동스피드
    public float jumpSpeed; //점프스피드
    public int jumpCount; //점프카운트

    public Vector2 knuckBackPos; //넉백방향

    //상태체크
    bool isGrounded;//그라운드 체크
    bool updateCheck;//업데이트 체크 변수
    bool isAttackable; //공격 가능 체크

    //공격 모션
    public GameObject attackForwardEffect; //공격 정면 모션

    //이펙트
    public GameObject damageEffect; //공격하면 피나는 애니메이션
    public GameObject damageEffect2; //이펙트
    public GameObject damageEffect3;
    //공격받을때 이펙트
    public GameObject hurtEffect1;

    //컴포넌트 가져와
    private Animator animator;
    private Rigidbody2D rigidbody;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    #endregion

    //사운드 매니저.. 그냥 합시다
    private AudioSource sfxPlayer;
    public AudioClip[] sound;



    // Start is called before the first frame update
    void Start()
    {
        //사운드
        sfxPlayer = GetComponent<AudioSource>();


        //공격
        updateCheck = true;
        isAttackable = true;

        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        transform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();

        // 체력올라가는 의자 관련
        rest.SetActive(false);  
        sprRest = rest.GetComponent<SpriteRenderer>();
        rest2.SetActive(false);
        sprRest2 = rest2.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerState();
        
        //true라면
        if (updateCheck)
        {
            Move();
            JumpControl();
            AttackControl();
        }
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal") * moveSpeed;

        //방향
        Vector2 velocity;
        velocity.x = h;
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;

        //반대 방향 움직임
        float moveDirection = -transform.localScale.x * h;
        if(moveDirection < 0)
        {
            Vector2 scale;
            scale.x = h < 0 ? 1 : -1;
            scale.y = 1;
            transform.localScale = scale;

            //땅이라면
            if (isGrounded)
            {
                animator.SetTrigger("IsRotate");
            }

        }else if(moveDirection > 0)
        {
            animator.SetBool("IsRun", true);
        }

        if(Input.GetAxis("Horizontal") == 0)
        {
            animator.SetTrigger("stopTrigger");
            animator.ResetTrigger("IsRotate");
            animator.SetBool("IsRun", false);
        }
    }

    //플레이어 상태 업데이트
    void UpdatePlayerState()
    {
        //땅인지 체크하고 땅이면 에니메이션 동작해라
        isGrounded = CheckGrounded();
        animator.SetBool("IsGround", isGrounded);

        //리지드바디의y값 받아오고 y값이 0보다 작으면 에니메이션 동작해라
        float verticalVelocity = rigidbody.velocity.y;
        animator.SetBool("IsDown", verticalVelocity < 0);

        //만약, 땅이거나 수직값이 0이라면
        if(isGrounded && verticalVelocity == 0)
        {
            animator.SetBool("IsJump", false);//점프 꺼주고
            animator.ResetTrigger("IsJumpFirst"); //1단점프 프리거 리셋시켜주고
            //animator.ResetTrigger("IsJumpScenod");//2단점프 프리거도 리셋
            animator.SetBool("IsDown", false); //다운 에니메이션 꺼주고

            //땅에 닿으면 다시 2로
            jumpCount = 2;
        }
    }

    //그라운드 체크
    private bool CheckGrounded()
    {
        Vector2 origin = transform.position;
        float radius = 0.2f;

        //아래쪽 감지
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 0.5f;
        //레이어 platform 가져옴, 벽은 wall,platform으로 만듬
        LayerMask layerMask = LayerMask.GetMask("Platform");
        //특정 범위만큼 체크(현재위치,지름,방향,발사거리,레이어마스크)
        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        //print("11"+hitRec.collider);
        return hitRec.collider != null;
    }

    //점프
    void Jump()
    {
        Vector2 velocity;
        velocity.x = rigidbody.velocity.x;
        velocity.y = jumpSpeed;
        rigidbody.velocity = velocity;

        animator.SetBool("IsJump", true);
        jumpCount -= 1;
        
        if(jumpCount == 0)
        {
            //2단 점프
            animator.SetTrigger("IsJumpSecond");
        }else if(jumpCount == 1)
        {
            //1단 점프
            animator.SetTrigger("IsJumpFirst");
        }
    }
    void JumpControl()
    {
        //만약 점프 누르지 않으면 아래로 쭉 실행하지마
        if (!Input.GetButtonDown("Jump"))
        {
            return;
        }

        if (false)
        {
        }
        //만약 점프 카운트가 0보다 크면 점프해
        if(jumpCount > 0)
        {
            sfxPlayer.PlayOneShot(sound[1]);
            Jump();
        }
    }

    //공격 버튼
    private void AttackControl()
    {
        //일반공격
        if (Input.GetButtonDown("Fire1") && isAttackable)
        {
            Attack();
            sfxPlayer.PlayOneShot(sound[0]);
        }
        //마우스 오른쪽 공격
        if(Input.GetButtonDown("Fire2") && isAttackable)
        {

        }
    }

    //공격(정면,위,아래)
    private void Attack()
    {
        //정면 공격
        AttackForward();
    }

    //정면공격,위공격, 아래공격.. 어렵다아
    private void AttackForward()
    {
        //공격 애니메이션 실행하고 
        animator.SetTrigger("IsAttack");
        //정면공격 오브젝트 켜준다.
        attackForwardEffect.SetActive(true);
        
        //방향 정하고
        Vector2 detectDirection;
        detectDirection.x = -transform.localScale.x;
        detectDirection.y = 0;
        
        //공격 코루틴(정면공격 오브젝트, 공격방향);
        StartCoroutine(attackCoroutine(attackForwardEffect, detectDirection));
    }


    int hitCount = 0;

    //수정 중
    private IEnumerator attackCoroutine(GameObject attackEffect, Vector2 detectDirection)
    {
        Vector2 origin = transform.position;

        float radius = 0.6f;
        float distance = 1.5f;
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Geo") | LayerMask.GetMask("GeoMount");

        //CircleCastAll origin(좌표) 위치에서 direction(방향)으로 distance(거리) 만큼 떨어져 있는 곳에
        //radius(반지름) 크기의  Circle이 있는데 이 Circle에 layerMask(타겟오브텍트의 레이어 이름)
        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        foreach (RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;
            string layerName = LayerMask.LayerToName(obj.layer);

            //레이에 부딪힌 대상이 에너미면
            if (layerName == "Enemy")
            {
                //플레이어의 공격력 데미지를 준다.
                obj.GetComponentInParent<Monster>().Damaged(attackDamage);

                Vector3 enemyPosition = obj.transform.position;
                Instantiate(damageEffect, enemyPosition, Quaternion.Euler(Vector2.zero));
                Destroy(Instantiate(damageEffect3, enemyPosition, Quaternion.Euler(Vector2.zero)),2f);
            }else if(layerName == "GeoMount")
            {
                GameManager.Instance.GeoRespawn(5, obj);
                hitCount++;

                if (hitCount >= 5)
                {
                    Destroy(obj, 2f);
                }
            }

        }
        //공격오브젝트를 0.05f 뒤에 끈다.
        yield return new WaitForSeconds(0.05f);
        attackEffect.SetActive(false);        
        isAttackable = true;
    }


    //===========================================으아아악
    // 미션: 슬로우모션 걸어보기
    //시간 제어기와 멈출 시간을 만듭니다.
    bool stopping;
    //멈출 시간
    public float stopTime=2;
    public float slowTime=2;

    //시간을 멈출 타이밍을 각자의 상황에 적절하게 작성해요
    //1. 충돌하면 코루틴 호출
    public void TimeStop()
    {
        if (!stopping)
        {
            stopping = true;
            Time.timeScale = 0;

            //타이머신 시작
            StartCoroutine("Stop");
        }
    }
    //3.
    IEnumerator Stop()
    {
        //yield return new WaitForSecondsRealtime(stopTime);
        //slow Time
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(slowTime);
        //show Time end ====

        //기본 흐르는 시간 = 1
        Time.timeScale = 1;
        stopping = false;
    }
    //===========================================

    //적 데미지 만큼 플레이어의 체력을 감소 시킨다.
    public void Hurt(int damage)
    {
        //데미지 줄이고
        hp = hp - damage;
        animator.SetTrigger("IsHurt");
        sfxPlayer.PlayOneShot(sound[2]);


        //넉백 효과 주기
        Vector2 vec2;
        //나의 x방향 -인지 +인지 * -10;
        vec2.x = transform.localScale.x * knuckBackPos.x;
        vec2.y = knuckBackPos.y;
        rigidbody.AddForce(vec2, ForceMode2D.Impulse);
        
        //update에서 이동을 주기에 x가 0이 되어 x값 넉백 적용이 안되어 코루틴 사용
        updateCheck = false;
        StartCoroutine(NuckbackCoroutine());


        //슬로우 모션
        //TimeStop();

        //체력 0이하면
        if (hp <= 0)
        {
            Die();
            //현재 활성화된 씬 이름 가져오기 SceneManager.GetActiveScene().name
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //return;
        }
        else
        {
            //데미지 입으면 color 투명도 높여줘
            //spriteRenderer.color = ;

            GameObject obj =Instantiate(hurtEffect1, transform.position, Quaternion.Euler(Vector2.zero));
            Destroy(obj, 2f);
        }
    }

    //넉백 코루틴
    IEnumerator NuckbackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        updateCheck = true;
    }

    //죽음
    void Die()
    {
        print("으앙쥬금");
        //에니메이션 실행
        animator.SetTrigger("IsDead");
        gameObject.SetActive(false);
        //respawn 위치 뽑아서 그위치에서 뜨게 해
    }

    //리스폰 위치 정해줄까?
    void Respawn()
    {

    }

    //Geo와 접촉하면
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            geo++;
            Destroy(collision.gameObject);
        }
    }


    //체력 올라가는 의자 ===========================================
    public GameObject rest; // 체력 증가 의자
    public GameObject rest2; // 체력 증가 의자

    SpriteRenderer sprRest;
    SpriteRenderer sprRest2;

    // 의자와 접촉하면 hp 증가 추가해보겠음
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Chair")    // 만약 Chair 와 trigger 면
        if(collision.gameObject.CompareTag("Chair"))
        {
            // '휴식' 생성
            rest.SetActive(true);
            rest2.SetActive(true);
            StartCoroutine(FadeInCoroutine());

            // 위로 화살표(W 키) 누르면
            print("2");
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                print("Rest");
                // '휴식' 사라짐(알파값 조절, a 가 0)
                StartCoroutine(FadeOutCoroutine());
                // 충전되는 이펙트
                // 일정 시간이 지난 후, hp++
                hp++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Chair")
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    float fadeCount = 0;
    IEnumerator FadeInCoroutine()
    {
        while (fadeCount < 1f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            sprRest.color = new Color(255, 255, 255, fadeCount);
            sprRest2.color = new Color(255, 255, 255, fadeCount);
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.005f);
            sprRest.color = new Color(255, 255, 255, fadeCount);
            sprRest2.color = new Color(255, 255, 255, fadeCount);

        }
    }
    //체력 올라가는 의자 ===========================================

}
