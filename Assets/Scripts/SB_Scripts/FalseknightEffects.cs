using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ �� �������� ȿ��
// ������ ������ �� ���(���� ����) �������� �ʹ�
// -> �� �������� ���� �ð� �������� ���� ����
// ���� ������?! - ������ ������ ��
// �ٴڿ� ������ ����� & ����Ʈ ����

public class FalseknightEffects : Monster
{
    int realityCount = 0;   // ��ü ���� ������ Ƚ��, 1ȸ ������ Dead ���� -> �� ��ü ����

    // *���� ���� ����Ʈ ����
    public float destroyDelay = 2f;
    //Trigger check 
    public bool isTriggered;
    public bool isFalling;
    public float createTime = 2;    // ����Ʈ ����
    float currentEffectsTime = 0;   // ���� �ð�

    public GameObject [] EffectGOD = new GameObject[4];
    public GameObject effectFactory1;    // ����Ʈ ����

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }


    // **���� ������ & ���� ����
    Rigidbody2D rb;
    Animator anim;

    private Transform target;   // �÷��̾ Ÿ������
    private GameObject checkPlayerPos;

    public float jumpPower = 1; // ���� ��
    public float jumptime = 3f; // ���� �ð� ����
    public float attackTime = 1f; // ���� �ð�
    public float stateChangeTime = 3f;  // ���� ��ȭ �ð� ����
    public int hp = 30; // False Knight ü�� 30
    public int attackDamage = 2; // False Knight ���ݷ�
    public int geo;


    float currentTime;  // ���� �ð�
    float stateCurrentTime; // ���� �ð�
    float moveCurrentTime;  // ������ �ð�

    bool check = true;
    //bool checkPlayer = false;   // �÷��̾ CheckPlayer�� ��Ҵ��� Ȯ��
    bool isGround = false;  // ������ ���� ��Ҵ��� Ȯ��

    Vector3 originPosition; // ó�� ��ġ

    //��ƼŬ
    public GameObject dieEffect; //�������

    public GameObject wave;
    // ���� ����
    public enum enemyState
    {
        BeforeFalling,
        Idle,
        Jump,   // ����
        Attack2,    // ����
        Damaged,
        Dead, 
        RealityFace,    // ��ü ��
        Reality,    // ��ü
    };

    public enemyState m_state = enemyState.BeforeFalling;

    //���� �� �Ƿ��ϰڽ��ϴ�.
    private AudioSource sfxBoss;
    public AudioClip[] sound;

    private void Start()
    {
        //����
        sfxBoss = GetComponent<AudioSource>();

        // *���� ����Ʈ ����
        //Rigidbody = Effect.GetComponent<Rigidbody2D>();
        //Rigidbody.gravityScale = 0;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        isTriggered = false;    // �÷��̾ Ư�� ������ ����� �� üũ�� ���� -> ������ ���𰡸� �ϸ� ������
        isFalling = false;  // ������ ������ ��, �������� �� Ȯ��
        
        // **���� ������ & ���� ����
        target = GameObject.Find("Player").transform;
        checkPlayerPos = GameObject.Find("CheckPlayer");

        rb = GetComponent<Rigidbody2D>();
        originPosition = transform.position;    // �ʱ� ��ġ ����
        anim = transform.GetComponentInChildren<Animator>();

        //waveAttack.SetActive(false);
        dustEffect.SetActive(false);
    }

    GameObject[] effect = new GameObject[4];
    private void Update()
    {
        // *���� ���� ����Ʈ

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
        
        // **���� ������
        
        if (hp <= 0)
        {
            if (check)
            {
                print(check);
                Die();
                m_state = enemyState.Dead;
            }
        }

        if(hp <= 25)        // ���� hp �� 25 �����̸� FakeDead ���·�, FakeDead ���¿��� RealityFace �� ����
        {
            m_state = enemyState.RealityFace;
            print("���� �ȵ�?");
            // ������ -2
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
            case enemyState.BeforeFalling:   // �⺻ ����
                BeforeFalling();
                break;
            case enemyState.Idle:   // �⺻ ����
                Idle();
                break;
            case enemyState.Jump:   // ����1
                Jump();
                break;
            case enemyState.Attack2:   // ����2 ����
                Attack2();
                break;
            case enemyState.Damaged:   // 
                break;
            case enemyState.Dead:   // 
                break;
            case enemyState.RealityFace:   // ��ü �󱼸�
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


    // **���� ������
    private void BeforeFalling()    // �������� ��, �ƹ��͵� ����
    {
        // �÷��̾ CheckPlayer�� ������,
        // isGround üũ�ؼ� true �� Idle ���·�
        if (isGround)
        {
            // ���� �����ϸ� ����Ʈ & ī�޶� ���
            Dust(); // ����Ʈ
            m_state = enemyState.Idle;
        }
    }

    public GameObject reward;   // ������ ����(����, ����)
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

        // ���� ����
        reward = Instantiate(rewardFactory);
        reward.transform.position = bossDiePos;

        Instantiate(dieEffect, origin, Quaternion.Euler(Vector2.zero));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //�÷��̾�� ���˽� ������
        if (collision.gameObject.tag.Equals("Player"))
        {
            CameraShake.Instance.PlayCameraShake();
            Attack(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ ���� ��Ҵ��� Ȯ��, ���� ������ ������(����Ʈ) ������ ����
        if (collision.gameObject.name == "CheckGround")
        {
            isGround = true;
        }
    }

    private void Attack(GameObject collision)   // ���� ����, �÷��̾� ������
    {
        collision.GetComponent<PlayerController>().Hurt(attackDamage);
    }

    private void Idle()
    {
        anim.enabled = true;

        isFalling = false;
        // ���� ���� �ð��� �� ũ��
        if (stateCurrentTime > stateChangeTime)
        {
            //dustEffect.SetActive(false);

            float stateRandom = Random.Range(0, 10);
            if (stateRandom < 5)
            {
                m_state = enemyState.Jump;  // �ѹ��� ����(����1)
            }
            else
            {
                m_state = enemyState.Attack2;    // ����� ����(����2)
            }
            stateCurrentTime = 0;

        }
    }
    public float xDir;  // ���� ������ ���� (1, -1)
    //Vector3 dir;
    public float speed = 5; // ���� �̵� �ӷ�
    // ���� �Լ�
    private void Jump() // ����1
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
                // ���� �ִϸ��̼�
                anim.SetTrigger("Attack1");

                // �������� ����� ������ �ȳ����� - ������ ������ �ɷ�,,
                sfxBoss.PlayOneShot(sound[1]);

            }
            else
            {
                xDir = -1;
                //print("L");
                // ���� �ִϸ��̼�
                anim.SetTrigger("Attack1");
                // �������� ����� ������ �ȳ����� - ������ ������ �ɷ�,,
                sfxBoss.PlayOneShot(sound[2]);

            }
            rb.AddForce(new Vector3(10 * xDir, 50, 0) * jumpPower, ForceMode2D.Impulse);
            //print("Jump");
            currentTime = 0;
        }

        m_state = enemyState.Idle;
    }

    public override void Damaged(int damage)    // ���� ������, �÷��̾ ����
    {
        if(m_state == enemyState.RealityFace)
        {
            sfxBoss.PlayOneShot(sound[4]);
            this.hp -= 2;
            spriteRenderer.sprite = sprites[1];
            print("�� ����?");
        }
        else
        {
            sfxBoss.PlayOneShot(sound[4]);
            this.hp--;
            print("���� hp 1 ����");
        }
        print("override_D1");
        //�θ�(Monster)�� �⺻ ������ �Լ�
        //base.Damaged_1();
    }

    // ���� �Լ�
    // ��/�� �������� 1ȸ ���� -> ����
    // Random
    private void Attack2()   // ����2
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

                // ���� �ִϸ��̼� (����2)
            }
            else
            {
                xDir = -1;
                //print("L");
                //anim.SetTrigger("IdleToAttack");

                // ���� �ִϸ��̼� (����2)
            }
            rb.AddForce(new Vector3(10 * xDir, 50, 0) * jumpPower, ForceMode2D.Impulse);
            //print("Jump");
            currentTime = 0;
        }

        m_state = enemyState.Idle;
    }

    // ��ü ��, ���� Ƚ�� ���� ���ϸ� �ٽ� Idle ���·� ��ȯ
    public Sprite[] sprites = new Sprite[2];   // �迭 �̿� - 0�� idle / 1�� ����
    private void RealityFace()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 1f)
        {
            anim.enabled = false;
            spriteRenderer.sprite = sprites[0]; // ��ü ���� �⺻ ���·� ��������Ʈ ����

            realityCount++; // ��ü �� Ƚ�� 1 ����
            currentTime = 0;
        }

        //���� hp �� 4 ���ϸ� ��ü�� �ʿ��� �и�(�ִϸ��̼�)

/*        if (hp <= 4)
        {

        }*/

    }



    // ��ü - �ʿ������!
    private void Reality()
    {
        anim.enabled = false;

    }


    //public GameObject FKAttackEffect;
    float waveTime = 5f;

    // ** ����� ** //
    private void WaveAttack()
    {
        print("WAVE");
        wave.SetActive(true);

        // �¿� ���� & ũ��
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


    // *** Effect �� ���� �͵� *** //
    // ������ �� ���� ����Ʈ : ó�� / ���� ��
    public GameObject dustEffect;
    //float effectTime = 3f;
    private void Dust()
    {
        print("DUST");
        // ���� ��ġ ���� (y�� -64�� ����)
        dustEffect.SetActive(true);
        GameObject dust = Instantiate(dustEffect, transform.position, Quaternion.Euler(Vector2.zero));
        //dustEffect.transform.position = transform.position; // �ϴ��� �䷸��
        Destroy(dust, 1f);
    }

    // ö�� �������� �� ����Ʈ, ��ġ ���� - �� �̰� �ִϸ��̼ǿ��� �����ϳ�
    private void DustAttack()
    {
        float bossPosX = transform.position.x;
        dustEffect.SetActive(true);

        Vector2 pos;
        // ���� ��ġ ����
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
