using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {


    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // 필요하면 호출 필요없으면 HUD 비활성화
    [SerializeField]
    private GameObject go_BullentHUD;

    // 총알 개수 텍스트에 반영
    [SerializeField]
    private Text[] text_Bullet;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckBullet();
	}

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBullctCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletcount.ToString();

    }

    
}
