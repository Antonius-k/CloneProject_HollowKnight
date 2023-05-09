using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region ask
    public int hp; //ü��
    public int geo; //���� ����
    public int attackDamage=1;

    public float moveSpeed; //�̵����ǵ�
    public float jumpSpeed; //�������ǵ�
    public int jumpCount; //����ī��Ʈ

    public Vector2 knuckBackPos; //�˹����

    //����üũ
    bool isGrounded;//�׶��� üũ
    bool updateCheck;//������Ʈ üũ ����
    bool isAttackable; //���� ���� üũ

    //���� ���
    public GameObject attackForwardEffect; //���� ���� ���

    //����Ʈ
    public GameObject damageEffect; //�����ϸ� �ǳ��� �ִϸ��̼�
    public GameObject damageEffect2; //����Ʈ
    public GameObject damageEffect3;
    //���ݹ����� ����Ʈ
    public GameObject hurtEffect1;

    //������Ʈ ������
    private Animator animator;
    private Rigidbody2D rigidbody;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    #endregion

    //���� �Ŵ���.. �׳� �սô�
    private AudioSource sfxPlayer;
    public AudioClip[] sound;



    // Start is called before the first frame update
    void Start()
    {
        //����
        sfxPlayer = GetComponent<AudioSource>();


        //����
        updateCheck = true;
        isAttackable = true;

        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        transform = gameObject.GetComponent<Transform>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();

        // ü�¿ö󰡴� ���� ����
        rest.SetActive(false);  
        sprRest = rest.GetComponent<SpriteRenderer>();
        rest2.SetActive(false);
        sprRest2 = rest2.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerState();
        
        //true���
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

        //����
        Vector2 velocity;
        velocity.x = h;
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;

        //�ݴ� ���� ������
        float moveDirection = -transform.localScale.x * h;
        if(moveDirection < 0)
        {
            Vector2 scale;
            scale.x = h < 0 ? 1 : -1;
            scale.y = 1;
            transform.localScale = scale;

            //���̶��
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

    //�÷��̾� ���� ������Ʈ
    void UpdatePlayerState()
    {
        //������ üũ�ϰ� ���̸� ���ϸ��̼� �����ض�
        isGrounded = CheckGrounded();
        animator.SetBool("IsGround", isGrounded);

        //������ٵ���y�� �޾ƿ��� y���� 0���� ������ ���ϸ��̼� �����ض�
        float verticalVelocity = rigidbody.velocity.y;
        animator.SetBool("IsDown", verticalVelocity < 0);

        //����, ���̰ų� �������� 0�̶��
        if(isGrounded && verticalVelocity == 0)
        {
            animator.SetBool("IsJump", false);//���� ���ְ�
            animator.ResetTrigger("IsJumpFirst"); //1������ ������ ���½����ְ�
            //animator.ResetTrigger("IsJumpScenod");//2������ �����ŵ� ����
            animator.SetBool("IsDown", false); //�ٿ� ���ϸ��̼� ���ְ�

            //���� ������ �ٽ� 2��
            jumpCount = 2;
        }
    }

    //�׶��� üũ
    private bool CheckGrounded()
    {
        Vector2 origin = transform.position;
        float radius = 0.2f;

        //�Ʒ��� ����
        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        float distance = 0.5f;
        //���̾� platform ������, ���� wall,platform���� ����
        LayerMask layerMask = LayerMask.GetMask("Platform");
        //Ư�� ������ŭ üũ(������ġ,����,����,�߻�Ÿ�,���̾��ũ)
        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        //print("11"+hitRec.collider);
        return hitRec.collider != null;
    }

    //����
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
            //2�� ����
            animator.SetTrigger("IsJumpSecond");
        }else if(jumpCount == 1)
        {
            //1�� ����
            animator.SetTrigger("IsJumpFirst");
        }
    }
    void JumpControl()
    {
        //���� ���� ������ ������ �Ʒ��� �� ����������
        if (!Input.GetButtonDown("Jump"))
        {
            return;
        }

        if (false)
        {
        }
        //���� ���� ī��Ʈ�� 0���� ũ�� ������
        if(jumpCount > 0)
        {
            sfxPlayer.PlayOneShot(sound[1]);
            Jump();
        }
    }

    //���� ��ư
    private void AttackControl()
    {
        //�Ϲݰ���
        if (Input.GetButtonDown("Fire1") && isAttackable)
        {
            Attack();
            sfxPlayer.PlayOneShot(sound[0]);
        }
        //���콺 ������ ����
        if(Input.GetButtonDown("Fire2") && isAttackable)
        {

        }
    }

    //����(����,��,�Ʒ�)
    private void Attack()
    {
        //���� ����
        AttackForward();
    }

    //�������,������, �Ʒ�����.. ��ƴپ�
    private void AttackForward()
    {
        //���� �ִϸ��̼� �����ϰ� 
        animator.SetTrigger("IsAttack");
        //������� ������Ʈ ���ش�.
        attackForwardEffect.SetActive(true);
        
        //���� ���ϰ�
        Vector2 detectDirection;
        detectDirection.x = -transform.localScale.x;
        detectDirection.y = 0;
        
        //���� �ڷ�ƾ(������� ������Ʈ, ���ݹ���);
        StartCoroutine(attackCoroutine(attackForwardEffect, detectDirection));
    }


    int hitCount = 0;

    //���� ��
    private IEnumerator attackCoroutine(GameObject attackEffect, Vector2 detectDirection)
    {
        Vector2 origin = transform.position;

        float radius = 0.6f;
        float distance = 1.5f;
        LayerMask layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Trap") | LayerMask.GetMask("Geo") | LayerMask.GetMask("GeoMount");

        //CircleCastAll origin(��ǥ) ��ġ���� direction(����)���� distance(�Ÿ�) ��ŭ ������ �ִ� ����
        //radius(������) ũ����  Circle�� �ִµ� �� Circle�� layerMask(Ÿ�ٿ�����Ʈ�� ���̾� �̸�)
        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(origin, radius, detectDirection, distance, layerMask);

        foreach (RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;
            string layerName = LayerMask.LayerToName(obj.layer);

            //���̿� �ε��� ����� ���ʹ̸�
            if (layerName == "Enemy")
            {
                //�÷��̾��� ���ݷ� �������� �ش�.
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
        //���ݿ�����Ʈ�� 0.05f �ڿ� ����.
        yield return new WaitForSeconds(0.05f);
        attackEffect.SetActive(false);        
        isAttackable = true;
    }


    //===========================================���ƾƾ�
    // �̼�: ���ο��� �ɾ��
    //�ð� ������ ���� �ð��� ����ϴ�.
    bool stopping;
    //���� �ð�
    public float stopTime=2;
    public float slowTime=2;

    //�ð��� ���� Ÿ�̹��� ������ ��Ȳ�� �����ϰ� �ۼ��ؿ�
    //1. �浹�ϸ� �ڷ�ƾ ȣ��
    public void TimeStop()
    {
        if (!stopping)
        {
            stopping = true;
            Time.timeScale = 0;

            //Ÿ�̸ӽ� ����
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

        //�⺻ �帣�� �ð� = 1
        Time.timeScale = 1;
        stopping = false;
    }
    //===========================================

    //�� ������ ��ŭ �÷��̾��� ü���� ���� ��Ų��.
    public void Hurt(int damage)
    {
        //������ ���̰�
        hp = hp - damage;
        animator.SetTrigger("IsHurt");
        sfxPlayer.PlayOneShot(sound[2]);


        //�˹� ȿ�� �ֱ�
        Vector2 vec2;
        //���� x���� -���� +���� * -10;
        vec2.x = transform.localScale.x * knuckBackPos.x;
        vec2.y = knuckBackPos.y;
        rigidbody.AddForce(vec2, ForceMode2D.Impulse);
        
        //update���� �̵��� �ֱ⿡ x�� 0�� �Ǿ� x�� �˹� ������ �ȵǾ� �ڷ�ƾ ���
        updateCheck = false;
        StartCoroutine(NuckbackCoroutine());


        //���ο� ���
        //TimeStop();

        //ü�� 0���ϸ�
        if (hp <= 0)
        {
            Die();
            //���� Ȱ��ȭ�� �� �̸� �������� SceneManager.GetActiveScene().name
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //return;
        }
        else
        {
            //������ ������ color ���� ������
            //spriteRenderer.color = ;

            GameObject obj =Instantiate(hurtEffect1, transform.position, Quaternion.Euler(Vector2.zero));
            Destroy(obj, 2f);
        }
    }

    //�˹� �ڷ�ƾ
    IEnumerator NuckbackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        updateCheck = true;
    }

    //����
    void Die()
    {
        print("�������");
        //���ϸ��̼� ����
        animator.SetTrigger("IsDead");
        gameObject.SetActive(false);
        //respawn ��ġ �̾Ƽ� ����ġ���� �߰� ��
    }

    //������ ��ġ �����ٱ�?
    void Respawn()
    {

    }

    //Geo�� �����ϸ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            geo++;
            Destroy(collision.gameObject);
        }
    }


    //ü�� �ö󰡴� ���� ===========================================
    public GameObject rest; // ü�� ���� ����
    public GameObject rest2; // ü�� ���� ����

    SpriteRenderer sprRest;
    SpriteRenderer sprRest2;

    // ���ڿ� �����ϸ� hp ���� �߰��غ�����
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Chair")    // ���� Chair �� trigger ��
        if(collision.gameObject.CompareTag("Chair"))
        {
            // '�޽�' ����
            rest.SetActive(true);
            rest2.SetActive(true);
            StartCoroutine(FadeInCoroutine());

            // ���� ȭ��ǥ(W Ű) ������
            print("2");
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                print("Rest");
                // '�޽�' �����(���İ� ����, a �� 0)
                StartCoroutine(FadeOutCoroutine());
                // �����Ǵ� ����Ʈ
                // ���� �ð��� ���� ��, hp++
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
    //ü�� �ö󰡴� ���� ===========================================

}
