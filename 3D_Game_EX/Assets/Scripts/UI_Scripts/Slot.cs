using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 인터페이스는 클래스와 달리 다중 상속이 가능.
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {

    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지.


    // 필요한 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private WeaponManager theWeaponmanager;

    void Start()
    {
        // 하이어라키뷰가 아닌 프리팹일 경우 이게 더 ([SerializeField] 보다) 좋다. (더 잘찾음)
        theWeaponmanager = FindObjectOfType<WeaponManager>();
    }

    // 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득 
	public void AddItem(Item _itme, int _count = 1)
    {
        item = _itme;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment) // StateMachine 로 구현 해둠.
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        } else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
       

        SetColor(1);

    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
        

    }

    // 이 스크립트가 적용된 객체에 마우스를 가져다 우클릭하면 실행.
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null) {
                if(item.itemType == Item.ItemType.Equipment)
                {
                    // 장착
                    StartCoroutine(theWeaponmanager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                    // Gun, SubMuchineGun1 >> 저번에 이런식으로 넣었던 거
                }
                else
                {
                    // 소모
                    Debug.Log(item.itemName + "을 소모했습니다.");
                    SetSlotCount(-1);
                    
                }
            }
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            // 처음에 안나타나는 오류 해결..
            DragSlot.instance.SetColor(1);
            DragSlot.instance.transform.position = eventData.position;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 드래그가 멈췄을 때 무조건 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0); // 투명화
        DragSlot.instance.dragSlot = null;
    }

    // 드래그가 멈췄을 때 만일 그 위가 슬롯이라면 호출
    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        // 복사본 생성
        Item _tempItme = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItme != null)
            DragSlot.instance.dragSlot.AddItem(_tempItme, _tempItemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();

    }

}
