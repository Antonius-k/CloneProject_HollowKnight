using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SerializableŰ���带 ���̰� Monobehaviour�� ��ӹް� �ִ� DatabaseManager�� Ŭ��������
//�� ��ü�� �����ϸ� Inspector�� ����Ǿ� ���� ������ �� �ִ�.
[System.Serializable]

public class Item
{
    public int itemID;//�������� ���� ID��. �ߺ� �Ұ���.
    public string itemName; //�������� �̸�, �ߺ� ����. (��������, ��������)
    public string itemDescription; //�������� ����
    public int itemCount; //������ ���� ����
    public Sprite itemIcon; //�������� ������.

    public ItemType itemType;
    
    //������
    public enum ItemType
    {
        //�Ҹ�ǰ,���,����Ʈ,�׿�
        Use,
        Equip,
        Quest,
        ETC
    }

    //������ Ŭ���� ȣ��Ǵ� ���� ä���ش�.
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