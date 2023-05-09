using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public Queue<string> sentences; // string�� ���� ť

    public string currentSentence;
    public TextMeshPro text;
    public GameObject Button;
    public void OnDialogue(string[] lines)
    {
        Button = GameObject.Find("Button");

        Button.SetActive(false);

        // ť �ʱ�ȭ
        sentences = new Queue<string>();
        sentences.Clear();

        // string �迭�� ���� ���� ť�� ����
        foreach (var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(DialogueFlow());
    }

    IEnumerator DialogueFlow()
    {

        yield return null;

        while (sentences.Count > 0) // ť�� ������ŭ �ݺ�, ��縦 TextMesh�� �־���
        {
            currentSentence = sentences.Dequeue();
            print(currentSentence);
            //text.text = currentSentence;
            foreach (var letter in currentSentence)
            {
                text.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(3f);

            text.text = null;
        }
        Destroy(gameObject);

    }
}