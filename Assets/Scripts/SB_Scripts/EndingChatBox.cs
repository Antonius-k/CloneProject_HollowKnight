using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingChatBox : MonoBehaviour
{
    public string[] sentences;  // 대사
    public Transform chatPos;   // 위치
    public GameObject chatBoxPrefab;

    void Start()
    {

    }
    public void DrawDialogue()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.GetComponent<DialogueSystem>().OnDialogue(sentences);
    }


}
