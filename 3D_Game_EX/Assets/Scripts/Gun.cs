using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public string gunName; // 총의 이름.
    public float range; // 총의 사정거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadTime; // 재장전 속도.

    public int damege; // 총의 데미지

    public int reloadBullctCount; // 총알 재장전 개수
    public int currentBulletcount; // 현재 탄알창에 남아있는 총알의 개수.
    public int maxBullectCount; // 현재 소유 가능 총알 개수
    public int carryBulletCount; // 현재 소유하고 있는 총알 개수

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준사의 반동 세기.

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;

	
}
