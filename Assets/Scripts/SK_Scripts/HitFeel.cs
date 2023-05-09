using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeel : MonoBehaviour
{
    //시간 제어기와 멈출 시간을 만듭니다.
    bool stopping;
    //멈출 시간
    public float stopTime;
    //====
    public float slowTime;



    //시간을 멈출 타이밍을 각자의 상황에 적절하게 작성해요
    //1. 충돌하면
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

            //타이머신 시작
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

        //기본 흐르는 시간 = 1
        Time.timeScale = 1;
        stopping = false;
    }


    //knockback 구현해보자
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
