using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static bool canPlayerMove = true; // 플레이어의 움직임 제어.

    public static bool isOpenInventory = false; // 인벤터리 활성화.

    public static bool isNight = false;
    public static bool isWater = false;

    public static bool isPause = false;

    private WeaponManager theWM;
    private bool flag = false;

    // Use this for initialization
    void Start () {
        theWM = FindObjectOfType<WeaponManager>();

    }
	
	// Update is called once per frame
	void Update () {
        if (isOpenInventory || isPause)
        {
            // 커서 자체를 잠가주기 >> Locked 으로 해두면 자동적으로 안움직이고 안보이게 된다.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; // 위에 Locked로 이미 안보이겠지만 그냥 보기 쉽게 넣어준거임.
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        if(isWater)
        {
            if(!flag)
            {
                StopAllCoroutines();
                StartCoroutine(theWM.WeaponInCoroutine());
                flag = true;
            }
            
        } else
        {
            if (flag)
            {
                flag = false;
                theWM.WeaponOut();
            }
        }
           

    }
}
