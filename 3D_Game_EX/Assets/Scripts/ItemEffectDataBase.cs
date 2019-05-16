using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; // 아이템 이름 >> 키값
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.")]
    public string[] part; // 어느 부위 회복시킬지?
    public int[] num; // 수치
}


public class ItemEffectDataBase : MonoBehaviour {

    [SerializeField]
    private ItemEffect[] itemEffect;

    // 필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private SlotToolTip theSlotToolTip;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void ShowToolTip(Item _itme, Vector3 _pos)
    {
        // 징검다리 역할 >> Slot Script에서 너무 많은 Find가 실행되지 않도록
        theSlotToolTip.ShowToolTip(_itme, _pos);
    }

    public void HideToolTip ()
    {
        theSlotToolTip.HideToolTip();
        // 근데 이 방법 자주쓰면은 함수의 함수를 호출하기 떄문에 스텍 오버플로우가 발생할 수 있음...
    }

    public void UseItem(Item _itme)
    {

        if (_itme.itemType == Item.ItemType.Equipment)
        {
            // 장착
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_itme.weaponType, _itme.itemName));
            // Gun, SubMuchineGun1 >> 저번에 이런식으로 넣었던 거
        }


        else if (_itme.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffect.Length; x++)
            {
                if(itemEffect[x].itemName == _itme.itemName)
                {

                    for (int y = 0; y < itemEffect[x].part.Length; y++)
                    {

                        switch(itemEffect[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffect[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffect[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffect[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffect[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffect[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위. HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다.");
                                break;
                        }
                        Debug.Log(_itme.itemName + "을 소모했습니다.");
                    }
                    return;

                }
                
            }
            Debug.Log("ItemDataBase에 일치하는 itemName 없습니다.");
        }
    }

}
