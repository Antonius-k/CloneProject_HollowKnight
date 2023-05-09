using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform itemTr;
    private Transform inventoryTr;

    private Transform itemListTr;
    private CanvasGroup canvasGroup;


    public static GameObject draggingItem = null;

    

    

    // Start is called before the first frame update
    void Start()
    {
        

        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("InventoryTest").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }



    

    // Update is called once per frame
    void Update()
    {
        
    }


    //ó�� �巡�װ� ���۵ɶ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        //���� �θ� �κ��丮 Ʈ���������� �������ش�.
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;

    }

    //�巡�װ� ���ᰡ �ɶ� �ѹ��� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = false;

        //������ �ڸ��� ������ �ȵǾ����� ���� �ڸ��� �������´�.
        if(itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
        }

    }
}
