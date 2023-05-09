using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 카메라 트랜스폼
    public Transform targetCamera;

    // 재생 시간
    public float playTime = 0.1f;

    public Transform playerPos;

    //public bool isCollision = false;

    public enum CameraShakeType
    {
        Random,
        Sine,
        Animation
    }
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
    }


    public CameraShakeType cameraShakeType = CameraShakeType.Random;

    CameraShakeBase cameraShake;

    [SerializeField]
    CameraShakeInfo info;
    // Start is called before the first frame update
    void Start()
    {
        cameraShake = CreateCameraShake(cameraShakeType);
        playerPos = GameObject.Find("Player").transform;
    }


    public static CameraShakeBase CreateCameraShake(CameraShakeType type)
    {
        switch (type)
        {
            case CameraShakeType.Random:
                return new CS_Random();
            case CameraShakeType.Sine:
                break;
            case CameraShakeType.Animation:
                break;
        }
        return null;
    }

    
    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("Fire1"))
        {
            PlayCameraShake();
        }*/
    }

    public void PlayCameraShake()
    {
        
        // 카메라 셰이크 종류에 따라 실행 방법이 다르다.
        if (cameraShakeType != CameraShakeType.Animation)
        {
            StopAllCoroutines();
            StartCoroutine(Play());
        }
    }

    private Vector2 velocity;
    private float smoothTimeX;
    private float smoothTimeY;

    // 재생 시간동안 카메라셰이크 실행
    IEnumerator Play()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, playerPos.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, playerPos.transform.position.y, ref velocity.y, smoothTimeY);

        Vector3 v3 = new Vector3(posX, posY, transform.position.z);
        // 카메라셰이크 초기화
        cameraShake.Init(targetCamera.position);
               
        //cameraShake.Init(playerPos.position);
        
        float currentTime = 0;
        // 재생 시간 동안 카메라셰이크 클래스의 Play() 함수 호출
        while (currentTime < playTime)
        {
            currentTime += Time.deltaTime;
            cameraShake.Play(targetCamera, info);
            // 카메라와 캐릭터의 거리를 일정하게 유지시키며 따라가게하고싶다.
            
            yield return null;
        }
        // 재생이 끝나면 Stop() 함수 호출
        cameraShake.Stop(targetCamera);
    }


}
