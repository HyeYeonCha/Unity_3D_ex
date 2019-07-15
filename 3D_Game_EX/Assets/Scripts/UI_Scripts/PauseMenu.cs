using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private GameObject go_BaseUI;
    [SerializeField]
    private SaveNLoad theSaveNLoad;


	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!GameManager.isPause)
            {
                CallMenu();
            } else
            {
                CloseMenu();
            }
        }

	}

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0;

    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClickSave ()
    {
        print("save");
        theSaveNLoad.SaveData();
    }

    public void ClickLoad()
    {
        print("load");
        theSaveNLoad.LoadData();
    }

    public void ClickExit()
    {
        print("exit");
        Application.Quit();
    }
}
