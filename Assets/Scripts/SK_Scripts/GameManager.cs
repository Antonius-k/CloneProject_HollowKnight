using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �ʿ�Ӽ� : �������, Ready, Start, Playing, GameOver
    //[System.NonSerialized]
    public enum GameState
    {
        Ready,
        Start,
        Playing,
        GameOver
    };

    //[SerializeField]
    public GameState m_state = GameState.Ready;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case GameState.Ready:
                ReadyState();
                break;
            case GameState.Start:
                StartState();
                break;
            case GameState.Playing:
                PlayingState();
                break;
            case GameState.GameOver:
                GameOverState();
                break;
        }
    }

    // 2�ʰ� ��ٸ��ȴٰ� ���¸� Start �� �����ϰ� �ʹ�.
    float currentTime = 0;
    public float readyDelayTime = 4;
    public float startDelayTime = 2;
    public float gameOverDelayTime = 4;
    private void ReadyState()
    {
        // 2�ʰ� ��ٸ��ȴٰ� ���¸� Start �� �����ϰ� �ʹ�.
        // 1. �ð��� �귶���ϱ�
        currentTime += Time.deltaTime;
        // 2. �ð��� �����ϱ�.
        if (currentTime > readyDelayTime)
        {
            // 3. ���¸� Start �� ����
            m_state = GameState.Start;
            currentTime = 0;
        }
    }

    // 2�ʰ� ��ٸ��ȴٰ� ���¸� Playing �� �����ϰ� �ʹ�.
    private void StartState()
    {
        // 1. �ð��� �귶���ϱ�
        currentTime += Time.deltaTime;
        // 2. �ð��� �����ϱ�.
        if (currentTime > startDelayTime)
        {
            m_state = GameState.Playing;
            currentTime = 0;
        }
    }

    private void PlayingState()
    {

    }

    private void GameOverState()
    {

    }

    //��ü EnemyController�� class�ʿ�
    


    //���� ��������==================
    
    //float currentTime;
    Vector3 dir;
    // ���� ����
    float currentAngle;
    
    GameObject[] geo;
    //������ ���� ������
    /*public void GeoRespawn(int geoCount,GameObject gameObject)
    {
        geo = Resources.LoadAll<GameObject>("Geo");
        for (int i = 0; i < geoCount; i++)
        {
            GameObject geoo = Instantiate(geo[i], gameObject.transform.position, Quaternion.identity) as GameObject;
        
        }
    }


    public IEnumerator GeoRespawnT(int geoCount, GameObject gameObject)
    {
        geo = Resources.LoadAll<GameObject>("Geo");
        for (int i = 0; i < geoCount; i++)
        {
            print("���� ��������");
            GameObject geoo = Instantiate(geo[i],gameObject.transform.position,Quaternion.identity) as GameObject;
        }
        yield return null;
    }*/

    public float time = 1;
    float deltaAngle;
    float currentTimeT;
    float currentAngleT;
    public GameObject geoSample;
    /*public IEnumerator GeoLastT(int geoCount, GameObject gameObject)
    {
        currentTimeT = time;
        deltaAngle = geoCount / 360;
        currentAngleT = 0;
        for (int i = 0; i < geoCount; i++)
        {
            GameObject geo1 = Instantiate(geoSample, gameObject.transform.position, Quaternion.Euler(0, deltaAngle, 0));
            currentAngleT += deltaAngle;
            geo1.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5),ForceMode2D.Impulse);
        }
        yield return null;
    }*/

    public void GeoRespawn(int geoCount, GameObject gameObject)
    {
        currentTimeT = time;
        deltaAngle = geoCount / 360;
        currentAngleT = 0;
        for (int i = 0; i < geoCount; i++)
        {
            GameObject geo1 = Instantiate(geoSample, gameObject.transform.position, Quaternion.Euler(0, deltaAngle, 0));
            currentAngleT += deltaAngle;
            geo1.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 10), ForceMode2D.Impulse);
        }
    }

    //===================================================================

    //�ð� ������ ���� �ð��� ����ϴ�.
    bool stopping;
    //���� �ð�
    public float stopTime;
    public float slowTime;
    
    //1. �浹�ϸ�
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
    //2.
    IEnumerator Stop()
    {
        yield return new WaitForSecondsRealtime(stopTime);
        //slow Time
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(slowTime);
        //show Time end ====

        //�⺻ �帣�� �ð� = 1
        Time.timeScale = 1;
        stopping = false;
    }
}
