using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : MonoBehaviour
{
    public string[] sentences;  // ���
    public Transform chatPos;   // ��ġ
    public GameObject chatBoxPrefab;

    //public GameObject ButtonListen;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void TalkNPC()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.transform.position = chatPos.transform.position;
        go.GetComponent<ChatSystem>().OnDialogue(sentences);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TalkNPC();
        }
    }
}
