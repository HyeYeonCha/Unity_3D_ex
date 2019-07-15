using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    public string scnenName = "Game";

    // 씬이 이동되어도 데이터는 저장되도록 싱글턴화 시키기.
    public static Title instance;

    private SaveNLoad theSaveNLoad;

    private void Awake()
    {
       
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public void ClickStart()
    {
        print("Loading");
        SceneManager.LoadScene(scnenName);
    }

    public void ClickLoad()
    {
        print("Load");
       
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        //SceneManager.LoadScene(scnenName);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scnenName); // 동기화

        while(!operation.isDone) // while문 안에다가 로딩 화면 만들어줘도 좋음. (opeartion.process 이용)
        {
            yield return null;
        } // 로딩이 끝날 때까지 1프레임씩 대기하다가 로딩이 끝나면 while문 벗어나서 데이터 호출.

        // Awake에서 부르면 이미 파괴되었기때문에 여기서 찾아줘야함.
        theSaveNLoad = FindObjectOfType<SaveNLoad>();
        theSaveNLoad.LoadData();
        gameObject.SetActive(false); // DontDestroy 때문에 Destroy로는 안먹힘.
    }

    public void ClickExit()
    {
        print("Exit");
        Application.Quit();
    }

}
