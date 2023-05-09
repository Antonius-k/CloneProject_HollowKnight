using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSentences : MonoBehaviour
{
    public string[] sentences;  // ���
    public Transform chatPos;   // ��ġ
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

    // �÷��̾ ���� ���� ���� ������
    // "���" ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ButtonListen.SetActive(true);
            Destroy(NPCArea);
            // ���� ��ư�� Ŭ���ϸ�
            // ��ư ����
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
