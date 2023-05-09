using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public string[] sentences;  // ���
    public Transform chatPos;   // ��ġ
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

    // �÷��̾ ���� ���� ���� ������
    // "���" ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // ��ư ����
            Button.SetActive(true);
            // ���� ��ư�� Ŭ���ϸ�
        }
    }
}
