using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 데이터 직렬화
// 새로 클래스 만들어 주기 >> 이거는 MonoBehaviour를 상속받지 않기 때문에 따로 컴포넌트 추가 X
public class Sound {

    public string name; // 곡의 이름
    public AudioClip clip; //곡

}


// 싱글턴화 시키기
public class SoundManager : MonoBehaviour {

    static public SoundManager instance;

    #region singleton
    void Awake() // 객체 생성시 최초 실행 그 다음 OnEnable() -> 근데 코루틴 실행 X
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject); // 기존에 있던 것을 살리고 새로운 것을 파괴.

    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    // 새로 생성한 클래스 사용방법 >> 변수처럼 선언해서 사용
    // public Sound a;

    public string[] playSoundName;

    public Sound[] effectSound;
    public Sound[] bgmSound;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSound.Length; i++)
        {
            if(_name == effectSound[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSound[i].name; // 재생중인 AudioSource의 이름 일치시켜주기
                        audioSourceEffects[j].clip = effectSound[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 AudioSouce가 사용중.");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");

    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {

            if(playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
            
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }





    // 매번 활성화된 실행.
    // Use this for initialization
    //void Start () {

    //}

    // Strat() 다음
    //Update is called once per frame
    //void Update()
    //{

    //}
}
