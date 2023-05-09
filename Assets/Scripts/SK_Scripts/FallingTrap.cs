using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    public float destroyDelay = 2f;
    //Trigger check 
    public bool isTriggered;
    private Rigidbody2D rigidbody;

    private SpriteRenderer spriteRenderer;

    //������ �Լ�ó��
    public Rigidbody2D Rigidbody { 
        get => rigidbody; 
        set => rigidbody = value; 
    }

    private void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        isTriggered = false;
    }

    private void Update()
    {
        if (isTriggered)
        {
            isTriggered = false;
            fall();
        }
    }
    //gravityScale ���� �÷��� ����߸���.
    public void fall()
    {
        Rigidbody.gravityScale = 1;
        StartCoroutine(fadeCoroutine());
    }

    //�����ð� �Ŀ� ���İ��� ���̴ٰ� ���� ��Ų��.
    private IEnumerator fadeCoroutine()
    {
        while (destroyDelay > 0)
        {
            destroyDelay -= Time.deltaTime;

            if (spriteRenderer.color.a > 0)
            {
                Color newColor = spriteRenderer.color;
                newColor.a -= Time.deltaTime / destroyDelay;
                spriteRenderer.color = newColor;
                yield return null;
            }
        }
        Destroy(gameObject);
    }
}
