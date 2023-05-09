using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeel : MonoBehaviour
{
    //�ð� ������ ���� �ð��� ����ϴ�.
    bool stopping;
    //���� �ð�
    public float stopTime;
    //====
    public float slowTime;



    //�ð��� ���� Ÿ�̹��� ������ ��Ȳ�� �����ϰ� �ۼ��ؿ�
    //1. �浹�ϸ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TimeStop();
    }
    
    void TimeStop()
    {
        if (!stopping)
        {
            stopping = true;
            Time.timeScale = 0;

            //Ÿ�̸ӽ� ����
            StartCoroutine("Stop");
        }
    }

    //3.
    IEnumerator Stop()
    {
        yield return new WaitForSecondsRealtime(stopTime);
        //slow Time
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(slowTime);
        //show Time end ====

        //�⺻ �帣�� �ð� = 1
        Time.timeScale = 1;
        stopping = false;
    }


    //knockback �����غ���
    public IEnumerator Knockback(float knockbackDuration, float knockbackPower, Transform obj, Rigidbody rigidbody)
    {
        float timer = 0;

        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rigidbody.AddForce(-direction * knockbackPower);
        }

        yield return 0;
    }
}
