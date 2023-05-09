using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 한 번 때릴 때마다 지오 5개 씩 생성
public class GeoMount : MonoBehaviour
{
    public GameObject player;
    public int geo = 5; // 한번 때릴 때마다 떨굴 지오 개수
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
            // 플레이어 뒤로 밀기
*//*            // 지오가 플레이어 오른쪽에 있으면
            if(transform.position.x >= player.transform.position.x)
            {
                // 왼쪽으로 밀기

            }
            else        // 지오가 플레이어 왼쪽에 있으면

            {
                // 오른쪽으로 밀기
            }*//*

            hitCount++;
            DropGeo();
        }
*/
    }



/*    private void OnCollisionEnter2d(Collision2D collision)
    {
        // 플레이어가 지오 더미를 한 번 때릴 때마다 지오가 5개 씩 생성됨
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