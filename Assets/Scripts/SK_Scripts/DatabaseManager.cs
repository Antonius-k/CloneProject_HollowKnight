using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    //List�� Item
    public List<Item> itemList = new List<Item>();


    // Start is called before the first frame update
    void Start()
    {
        itemList.Add(new Item(001, "��������", "ü�� 50ä���ִ� ������ ����", Item.ItemType.Use));
        itemList.Add(new Item(002, "���Ÿ� ����", "���Ÿ� ������ �����ϰ� ���ִ� ����", Item.ItemType.Equip));
        itemList.Add(new Item(003, "�ֹ���", "�ֹ��� ����", Item.ItemType.Equip));


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
