using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour {

    public static bool inventoryActivated = false;


    // 필요한 컴포넌트 
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotParent;


    // 슬롯들
    private Slot[] slots;

    public Slot[] GetSlots() { return slots; }

    [SerializeField]
    private Item[] items;

    public void LoadToInven(int _arrayNum, string _itemName, int _itmeNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i].itemName == _itemName)
            {
                slots[_arrayNum].AddItem(items[i], _itmeNum);
            }
        }
    }

	// Use this for initialization
	void Start () {
        slots = go_SlotParent.GetComponentsInChildren<Slot>(); // 미리 선언해준 slot[] 배열안의 슬롯들이 싹 들어감
	}
	
	// Update is called once per frame
	void Update () {
        TryOpenInventory();
	}


    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            
                OpenInventory();
             else
            
                CloseInventory();
            
        }
    }


    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
        PlayerController.isActivated = false;
    }

    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
        PlayerController.isActivated = true;
    }

    public void AcquireItem(Item _item, int _count = 1)
    {

        if(Item.ItemType.Equipment != _item.itemType)
        {
            // 아이템이 있다면 아이템을 증가 시켜주고
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

       
        // 아이템이 없다면 빈자리를 찾아서 아이템을 넣어줌
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
            
        }
    }

}
