using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // ī�޶� Ʈ������
    public Transform targetCamera;

    // ��� �ð�
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
        
        // ī�޶� ����ũ ������ ���� ���� ����� �ٸ���.
        if (cameraShakeType != CameraShakeType.Animation)
        {
            StopAllCoroutines();
            StartCoroutine(Play());
        }
    }

    private Vector2 velocity;
    private float smoothTimeX;
    private float smoothTimeY;

    // ��� �ð����� ī�޶����ũ ����
    IEnumerator Play()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, playerPos.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, playerPos.transform.position.y, ref velocity.y, smoothTimeY);

        Vector3 v3 = new Vector3(posX, posY, transform.position.z);
        // ī�޶����ũ �ʱ�ȭ
        cameraShake.Init(targetCamera.position);
               
        //cameraShake.Init(playerPos.position);
        
        float currentTime = 0;
        // ��� �ð� ���� ī�޶����ũ Ŭ������ Play() �Լ� ȣ��
        while (currentTime < playTime)
        {
            currentTime += Time.deltaTime;
            cameraShake.Play(targetCamera, info);
            // ī�޶�� ĳ������ �Ÿ��� �����ϰ� ������Ű�� ���󰡰��ϰ�ʹ�.
            
            yield return null;
        }
        // ����� ������ Stop() �Լ� ȣ��
        cameraShake.Stop(targetCamera);
    }


}
