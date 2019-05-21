using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxeController : CloseWeaponController {

    // 활성화 여부
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }


    void Update()
    {
        if (isActivate)
            TryAttack();
    }


    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                } else if(hitInfo.transform.tag == "WeekAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit"); // 하드코딩.. >> 빠르게 하려고
                    hitInfo.transform.GetComponent<WeekAnimal>().Demage(1, transform.position);
                    // currentCloseWeapon.damege >> 데미지 지금은 1로 줌 Test 중이라
                }

                // 근데 이거는 아직 구현 안했으므로 일단 보류..
                //else if (hitInfo.transform.tag == "StrongAnimal")
                //{
                //    SoundManager.instance.PlaySE("Animal_Hit"); // 하드코딩.. >> 빠르게 하려고
                //    hitInfo.transform.GetComponent<StrongAnimal>().Demage(1, transform.position);
                //    // currentCloseWeapon.damege >> 데미지 지금은 1로 줌 Test 중이라
                //}



                isSwing = false; // 중복 실행 X
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
