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
        //�÷��̾ ���� �̵�
        transform.position += direction * movingSpeed * Time.deltaTime;

    }

    //�Ѿ˰� �ε����� �÷��̾��� ü���� ������
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
