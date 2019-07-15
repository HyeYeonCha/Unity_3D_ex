using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(GunController))] // 반드시 넣어주게 되고 SerializeField로 선언해준거 못뺌!
public class WeaponManager : MonoBehaviour {


    // 현재 무기의 타입.
    [SerializeField]
    private string currentWeaponType;


    // 무기 중복 교체 실행 방지.
    public static bool isChangeWeapon = false;
    // 공유 자원 (직접 접근_참조) , 클래스 변수 > 정적변수

    // static 변수를 사용하면 메모리적으로 낭비일 뿐만 아니라 보호수준도 떨어진다.
    // 그러므로 한 번에 선언하여 바꿀 일이 거의 없는 경우 몇 군데만 사용 하는 것이 바람직하다.
    // 많이 남발하지 않도록 주의!


    // 현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;


    // 무기 교체 딜레이, 무기 교체가 완전히 끝난 시점.
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    // 무기 종류들 전부 관리.
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickAxes;



    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    // 키값을 이용하여 값을 (쌍을이뤄) 저장한 후에 이를 이용해 편리하게 값을 넣고 뺄 수 있다.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickAxeDictionary = new Dictionary<string, CloseWeapon>();


    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickAxeController thePickAxeController;





    // Use this for initialization
    void Start () {

        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickAxes.Length; i++)
        {
            pickAxeDictionary.Add(pickAxes[i].closeWeaponName, pickAxes[i]);
        }

    }
	
	// Update is called once per frame
	void Update () {
		if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손")); //  이것은 하드코딩 나중에는 변수로 받아주기
            // 무기 교체 실행 (서브머신건)
            if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1")); ; // 무기 교체 실행 (맨손)

            if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe")); ; // 무기 교체 실행 (도끼)

            if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe")); ; // 무기 교체 실행 (곡갱이)

        }
    }


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {

        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);


        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type; // 바꾸고자 할 타입을 현재 타입에 넣어주기


        isChangeWeapon = false;



    } 

    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false; // 기존의 있던 것을 취소
                break;

            case "HAND":
                HandController.isActivate = false;
                break;

            case "AXE":
                AxeController.isActivate = false;
                break;

            case "PICKAXE":
                PickAxeController.isActivate = false;
                break;
        }
    }


    private void WeaponChange(string _type, string _name)
    {

        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        
         else if (_type == "HAND")
            theHandController.CloseWeaponChange(handDictionary[_name]);

        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);

        else if (_type == "PICKAXE")
            thePickAxeController.CloseWeaponChange(pickAxeDictionary[_name]);


    }


   public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false;

        currentWeapon.gameObject.SetActive(true);
    }




}
