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


    //처음 드래그가 시작될때
    public void OnBeginDrag(PointerEventData eventData)
    {
        //현재 부모를 인벤토리 트랜스폼으로 변경해준다.
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;

    }

    //드래그가 종료가 될때 한번만 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = false;

        //슬롯의 자리로 변경이 안되었으면 원래 자리로 돌려놓는다.
        if(itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
        }

    }
}
