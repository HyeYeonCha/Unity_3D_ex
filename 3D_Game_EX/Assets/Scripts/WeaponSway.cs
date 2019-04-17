using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    // 기존 위치
    private Vector3 originPos;


    // 현재 위치
    private Vector3 currentPos;

    // sway 한계
    [SerializeField]
    private Vector3 limitPos;

    // 정조준 sway 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    // 부드러운 움직임 정도.
    [SerializeField]
    private Vector3 smoothSway;

    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;




	// Use this for initialization
	void Start () {
        originPos = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        TrySway();
	}

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) // 상하좌우가 0이 아닐때
            Swaying();
        else
            BackToOriginPos();
    }


    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if(!theGunController.isFineSightMode) // 정조준 상태일 때
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                   Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, -limitPos.y), originPos.z);
            
        } else // 정조준 상태가 아닐 때 (살짝 흔들림)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                   Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, -fineSightLimitPos.y), originPos.z);
        }

        transform.localPosition = currentPos;

    }

    private void BackToOriginPos()
    {

        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;

    }


}
