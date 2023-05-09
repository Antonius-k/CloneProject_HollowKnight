using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EndingDialogue : MonoBehaviour
{
    public Queue<string> sentences; // string을 담을 큐
    public string currentSentence;

    public TextMeshPro text;

    public void OnDialogue(string[] lines)
    {


        // 큐 초기화
        sentences = new Queue<string>();
        sentences.Clear();

        // string 배열의 값을 전부 큐에 담음
        foreach (var line in lines)
        {
            sentences.Enqueue(line);
        }
        StartCoroutine(DialogueFlow());
    }

    IEnumerator DialogueFlow()
    {

        yield return null;

        while (sentences.Count > 0) // 큐의 개수만큼 반복, 대사를 TextMesh에 넣어줌
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
        //Destroy(gameObject);

    }
}
