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

    //List로 Item
    public List<Item> itemList = new List<Item>();


    // Start is called before the first frame update
    void Start()
    {
        itemList.Add(new Item(001, "빨간포션", "체력 50채워주는 마법의 물약", Item.ItemType.Use));
        itemList.Add(new Item(002, "원거리 부적", "원거리 공격을 가능하게 해주는 부적", Item.ItemType.Equip));
        itemList.Add(new Item(003, "애벌래", "애벌래 갯수", Item.ItemType.Equip));


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
