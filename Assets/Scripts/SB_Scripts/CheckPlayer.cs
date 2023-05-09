using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public string[] sentences;  // 대사
    public Transform chatPos;   // 위치
    public GameObject chatBoxPrefab;
    public GameObject Button;
    void Start()
    {
        Button = GameObject.Find("Button");
        Button.SetActive(false);
    }
    public void DrawDialogue()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.GetComponent<DialogueSystem>().OnDialogue(sentences);
    }

    // 플레이어가 일정 범위 내로 들어오면
    // "듣기" 생성
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 버튼 생성
            Button.SetActive(true);
            // 만약 버튼을 클릭하면
        }
    }
}
