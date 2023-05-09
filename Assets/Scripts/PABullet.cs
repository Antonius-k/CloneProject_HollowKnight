using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PABullet : MonoBehaviour
{

    public Vector3 direction;
    public int damage = 1;
    public float movingSpeed = 5f;
    public float destroyTime = 0f;

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        direction = (target.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어를 향해 이동
        transform.position += direction * movingSpeed * Time.deltaTime;

    }

    //총알과 부딪히면 플레이어의 체력을 줄이자
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer)== "Player") {

            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.Hurt(damage);

            Destroy(gameObject, destroyTime);

        }else if(LayerMask.LayerToName(collision.gameObject.layer) == "Platform")
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
