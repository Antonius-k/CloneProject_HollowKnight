using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어가 특정지점(여기)를 밟으면 보스바닥 destroy
public class CheckPlayerFall : MonoBehaviour
{
    private Transform target;   // 플레이어를 타겟으로
    public GameObject platform;
    public GameObject platform2;
    public Camera camera;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //접촉하면 카메라도 좀 멀리서 잡아주는게 좋을듯해서
        


        //플레이어와 접촉
        if (collision.gameObject.tag.Equals("Player"))
        {
            Destroy(platform, 1f);
            Destroy(platform2, 1f);
        }
    }
}
