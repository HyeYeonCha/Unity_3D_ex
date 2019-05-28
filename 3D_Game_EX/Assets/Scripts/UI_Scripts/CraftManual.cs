using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    public string craftName; // 이름
    public GameObject go_Prefab; // 실제 설치될 프리팹.
    public GameObject go_PreviewPrefab; // 미리보기 프리팹.
}

public class CraftManual : MonoBehaviour {

    // 상태변수
    private bool isActivated = false;
    private bool isPreViewActivated = false;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; // 모닥불용 탭

    private GameObject go_Preview; // 미리보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수

    [SerializeField]
    private Transform tf_Player; // 플레이어의 Transform

    // Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range; 

    public void SlotClick(int _slotNumber)
    {
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreViewActivated = true;
        go_BaseUI.SetActive(false);
    }


	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreViewActivated)
        {
            Window();
        }

        if(isPreViewActivated)
        {
            PreviewPositionUpdate();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }


	}

    private void Build()
    {
        if(isPreViewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreViewActivated = false;
            go_Preview = null;
            go_Prefab = null;
            
        }
    }



    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                // 레이저를 쏴서 맞은 객체의 실제좌표 반환
                Vector3 location = hitInfo.point;
                go_Preview.transform.position = location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreViewActivated)
            Destroy(go_Preview);

        // 초기화
        isActivated = false;
        isPreViewActivated = false;
        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }


    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
            CloseWindow();
    }


    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }


    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }


}
