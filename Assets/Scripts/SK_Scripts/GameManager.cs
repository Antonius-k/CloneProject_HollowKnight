using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 필요속성 : 현재상태, Ready, Start, Playing, GameOver
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

    // 2초간 기다리렸다가 상태를 Start 로 변경하고 싶다.
    float currentTime = 0;
    public float readyDelayTime = 4;
    public float startDelayTime = 2;
    public float gameOverDelayTime = 4;
    private void ReadyState()
    {
        // 2초간 기다리렸다가 상태를 Start 로 변경하고 싶다.
        // 1. 시간이 흘렀으니까
        currentTime += Time.deltaTime;
        // 2. 시간이 됐으니까.
        if (currentTime > readyDelayTime)
        {
            // 3. 상태를 Start 로 변경
            m_state = GameState.Start;
            currentTime = 0;
        }
    }

    // 2초간 기다리렸다가 상태를 Playing 로 변경하고 싶다.
    private void StartState()
    {
        // 1. 시간이 흘렀으니까
        currentTime += Time.deltaTime;
        // 2. 시간이 됐으니까.
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

    //전체 EnemyController할 class필요
    


    //지오 떨구기이==================
    
    //float currentTime;
    Vector3 dir;
    // 누적 각도
    float currentAngle;
    
    GameObject[] geo;
    //죽으면 지오 떨구기
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
            print("지오 떨어져라");
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

    //시간 제어기와 멈출 시간을 만듭니다.
    bool stopping;
    //멈출 시간
    public float stopTime;
    public float slowTime;
    
    //1. 충돌하면
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
    //2.
    IEnumerator Stop()
    {
        yield return new WaitForSecondsRealtime(stopTime);
        //slow Time
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(slowTime);
        //show Time end ====

        //기본 흐르는 시간 = 1
        Time.timeScale = 1;
        stopping = false;
    }
}
