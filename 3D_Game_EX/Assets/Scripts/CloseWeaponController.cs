﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 미완성 클래스 >> 추상 클래스
public abstract class CloseWeaponController : MonoBehaviour {
   

    // 현재 장착된 Hand형 타입 무기.
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중??
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;
    [SerializeField]
    protected LayerMask layerMask;

    //Update is called once per frame // update 어차피 실행 X >> 추상 클래스 이므로 
    //void Update()
    //{
    //    if (isActivate)
    //        TryAttack();
    //}

    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            // Debug.Log("인벤토리 활성화 X");
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    // 코루틴 실행
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점.
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }
    // 미완성 >> 추상 코루틴.
    protected abstract IEnumerator HitCoroutine();
    

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        // transform.forward == transform.TransformDirection(Vector3.forward) 두 개는 같은 의미
        return false;
    }

    // 완성 함수이지만, 추가 편집이 가능한 함수 >> 가상 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>(); // Transform 으로 해줘야 오류 안뜸 모든 객체의 가장 기본적인 타입
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);

    }
}
