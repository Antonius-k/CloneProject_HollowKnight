using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� �� ���� ������ ���� 5�� �� ����
public class GeoMount : MonoBehaviour
{
    public GameObject player;
    public int geo = 5; // �ѹ� ���� ������ ���� ���� ����
    //public GameObject sword;

    int hitCount = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
/*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            // �÷��̾� �ڷ� �б�
*//*            // ������ �÷��̾� �����ʿ� ������
            if(transform.position.x >= player.transform.position.x)
            {
                // �������� �б�

            }
            else        // ������ �÷��̾� ���ʿ� ������

            {
                // ���������� �б�
            }*//*

            hitCount++;
            DropGeo();
        }
*/
    }



/*    private void OnCollisionEnter2d(Collision2D collision)
    {
        // �÷��̾ ���� ���̸� �� �� ���� ������ ������ 5�� �� ������
        if(collision.gameObject.tag.Equals("Sword"))
        {
            hitCount++;
            print(hitCount);
            DropGeo();
        }
    }
*/
/*private void DropGeo()
{
    //StartCoroutine(GameManager.Instance.GeoRespawn(geo, gameObject));
    GameManager.Instance.GeoRespawn(geo, gameObject);


    if (hitCount >= 5)
    {
        Destroy(gameObject, 2f);
    }


}
}
*/