using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Serializable키워드를 붙이고 Monobehaviour를 상속받고 있는 DatabaseManager의 클래스에서
//이 객체를 생성하면 Inspector에 노출되어 값을 지정할 수 있다.
[System.Serializable]

public class Item
{
    public int itemID;//아이템의 고유 ID값. 중복 불가능.
    public string itemName; //아이템의 이름, 중복 가능. (고대유물, 고대유물)
    public string itemDescription; //아이템의 설명
    public int itemCount; //아이템 소지 개수
    public Sprite itemIcon; //아이템의 아이콘.

    public ItemType itemType;
    
    //열거형
    public enum ItemType
    {
        //소모품,장비,퀘스트,그외
        Use,
        Equip,
        Quest,
        ETC
    }

    //생성자 클래스 호출되는 순간 채워준다.
    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCount =1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
