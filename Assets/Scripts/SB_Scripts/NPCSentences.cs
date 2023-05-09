using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSentences : MonoBehaviour
{
    public string[] sentences;  // 대사
    public Transform chatPos;   // 위치
    public GameObject chatBoxPrefab;
    public GameObject ButtonListen;
    public GameObject NPCArea;

    Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        ButtonListen = GameObject.Find("ButtonListen");
        ButtonListen.SetActive(false);
        originPos = transform.position;
    }

    private void Update()
    {
        transform.position = originPos;
    }

    public void TalkNPC()
    {
        //ButtonListen.SetActive(false);

        GameObject go = Instantiate(chatBoxPrefab);
        go.transform.position = chatPos.transform.position;
        go.GetComponent<ChatSystem>().OnDialogue(sentences);
    }

    // 플레이어가 일정 범위 내로 들어오면
    // "듣기" 생성
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ButtonListen.SetActive(true);
            Destroy(NPCArea);
            // 만약 버튼을 클릭하면
            // 버튼 생성
/*            if (ButtonListen.activeSelf)
            {
                print("111111");
            }*/
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //ButtonListen.SetActive(false);
    }
}
