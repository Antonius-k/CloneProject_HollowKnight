using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �÷��̾ Ư������(����)�� ������ �����ٴ� destroy
public class CheckPlayerFall : MonoBehaviour
{
    private Transform target;   // �÷��̾ Ÿ������
    public GameObject platform;
    public GameObject platform2;
    public Camera camera;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�����ϸ� ī�޶� �� �ָ��� ����ִ°� �������ؼ�
        


        //�÷��̾�� ����
        if (collision.gameObject.tag.Equals("Player"))
        {
            Destroy(platform, 1f);
            Destroy(platform2, 1f);
        }
    }
}
