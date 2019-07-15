using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] // 데이터 직렬화 >> 한 줄로 데이터들이 나열되어 저장 장치에서 읽고 쓰기가 쉬워진다.
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 PlayerRot;

    public List<int> invenArrayNumber = new List<int>();
    // Slot 자체를 기억시키지 않는 이유 >> Slot은 직렬화를 시켜도 직렬화가 안되기 때문에
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
}

public class SaveNLoad : MonoBehaviour {

    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FLIENAME = "/SaveFile.txt";

    private PlayerController thePlayer;
    private Inventory theInven;


	// Use this for initialization
	void Start () {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/"; // Path 지정

        if(Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }

	}

	public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theInven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.PlayerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theInven.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null )
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FLIENAME, json); // 실제 데이터를 물리적인 위치에 저장시키겠다.

        Debug.Log("저장 완료");
        Debug.Log(json); // json Test 

    }

    public void LoadData()
    {
        if(File.Exists(SAVE_DATA_DIRECTORY + SAVE_FLIENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FLIENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<PlayerController>();
            theInven = FindObjectOfType<Inventory>();

            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.PlayerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }
}
