using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    static public bool isActivated = true;

    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;


    // 움직임 체크 변수
    private Vector3 lastPos; // 전 프레임의 현재 위치 현재 프레임과 비교하여 같은지 다른지로 현 상태를 체크
    



    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    
    // 발 착지 여부
    private CapsuleCollider capsuleCollider;

    
    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private Crosshair theCrosshair;
    private GunController theGunController;
    private StatusController theStatusController;


	// Use this for initialization
	void Start () {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        // 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }
	
	// Update is called once per frame
	void Update () {

        if(isActivated && GameManager.canPlayerMove)
        {
            IsGround();
            TryJump();
            TryRun(); // 반드시 Move() 위에 있어야함! 움직이기 전에 뛰는지를 판단해야 하기 때문에.
            TryCrouch();
            Move();
            MoveCheck();
            if (!Inventory.inventoryActivated)
            {
                CameraRotation();
                CharacterRotation();
            }
        }

	}

    // 앉기 시도
    private void TryCrouch()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

    }

    // 앉기 (스위치 함수)
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;

        } else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);

    }

    // 부드러운 앉기 동작 실행
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y; // 자식 오브젝트이기 때문에 부모 오브젝트의 position을 이용하기위해서
        int count = 0;


        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f); // 보간 함수
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15) // 보간의 범위 지정
            {
                break; // 보간함수가 정확하지 않기 때문에.
            }

            yield return null;
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
        // while 문을 반복하다가 빠져나와서 목적지 위치로 고정해줌.

    }

    
    // 지면 체크
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.2f); // Vector3.down >> 무조건 아래방향으로 (절대적인 값, 캡슐기준이 아닌!)
        // CapsuleCollider의 영역 >> bounds , extents >> bounds 크기의 절반 (extents.y >> y크기의 절반) // 0.1f는 약간의 여유 >> 계단이나 오르막길 등 오차 범위 때문.
        // Raycast로 해당 범위까지 레이저를 발사하여 무언가 물체가 닿았을 때, true 반환 아무것도 없으면 false 반환.

        theCrosshair.JumpingAnimation(!isGround);

    }

    // 점프 시도
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    // 점프 실행
    private void Jump()
    {
        // 앉은 상태에서 점프시 앉은 상태 해제
        if (isCrouch) 
            Crouch(); // Crouch() 는 스위치 함수 이므로 만약 앉고 있을 때 뛴다면 다시 일어나게 된다.

        theStatusController.DecreaseStamins(100);
        myRigid.velocity = transform.up * jumpForce;
    }

    // 달리기 시도
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        } 
        if(Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    // 달리기 실행
    private void Running()
    {
        // 앉은 상태에서 달리면 앉기 취소 (Crouch()는 스위치 함수 이므로 !!)
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStamins(10);
        applySpeed = runSpeed;
    }
    
    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }


    // 움직임 실행 >> 매 프레임마다 실행
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);


    }


    // 
    private void MoveCheck()
    {

        if(!isRun && !isCrouch && isGround)
        {
            if(Vector3.Distance(lastPos, transform.position) >= 0.01f)
            // if (lastPos != transform.position) >> 경사로에서 미끄러져도 프레임에 따라 위치값이 달라지기 때문에 여유값필요. // 처음 호출 되었을 때의 값과 비교
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalikingAnimation(isWalk);
            lastPos = transform.position; // 이전 프레임의 위치 (조건문 들어가기 전에 이미 값 대입되므로)

        }


    }



    // 위아래 카메라 회전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX; // 마우스 반전 때문에 -
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // 구한 오일러 쿼터니언 값을 유니티 내장 함수에 넣어주기
    }
}
