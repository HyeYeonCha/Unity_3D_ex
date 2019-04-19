using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ProjectView에서 오른쪽 마우스 눌렀을 때 메뉴 생성
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]

// GameObject에 붙일 필요 없어짐 !!
public class Item : ScriptableObject {

    public string itemName; // 아이템의 이름.
    public ItemType itemType; // 아이템의 유형.
    public Sprite itemImage; // 아이템의 이미지. (Sprite는 Canvase 필요 없음 !! >> World 좌표만 있으면 가능)
    public GameObject itemPrefab; // 아이템의 프리팹.

    public string weaponType; // 무기 유형.

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }


    
}
