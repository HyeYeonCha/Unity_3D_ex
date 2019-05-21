using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour {

    [SerializeField]
    protected string animalName; // 동물의 이름
    [SerializeField]
    protected int hp;  // 동물의 체력

    [SerializeField]
    protected float walkSpeed; // 걷기 스피드
    [SerializeField]
    protected float runSpeed; // 뛰기 스피드
    [SerializeField]
    // protected float turningSpeed; // 회전 스피드
    // protected float applySpeed;

    protected Vector3 destination; // 방향 >> 목적지로 수정


    // 상태변수
    protected bool isAction; // 행동중인지 아닌지 판별
    protected bool isWalking; // 걷는지 안 걷는지 판별
    protected bool isRunning; // 뛰는지 판별
    protected bool isDead; // 죽었는지 판별

    [SerializeField]
    protected float walkTime; // 걷기시간
    [SerializeField]
    protected float waitTime; // 대기시간
    [SerializeField]
    protected float runTime; // 뛰기시간
    protected float currentTime;


    // 필요한 컴포넌트
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource theAudio;
    protected NavMeshAgent nav;

    [SerializeField]
    protected AudioClip[] sound_Normal;
    [SerializeField]
    protected AudioClip sound_Hurt;
    [SerializeField]
    protected AudioClip sound_Dead;


    // Use this for initialization
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
             // Rotation();
            ElapseTime();
        }

    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            // rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            nav.SetDestination(transform.position + destination * 5f); // >> 정규화해주었기때문에 합이 1밖에 안되어서 5f 정도를 곱해주었다.

        }
    }


    //private void Rotation()
    //{
    //    if (isWalking || isRunning)
    //    {
    //        Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
    //        rigid.MoveRotation(Quaternion.Euler(_rotation));
    //    }
    //}


    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                // 다음 랜덤 행동 개시.
                Reset();
            }
        }
    }

    // Reset() >> 스크립트를 컴포넌트에 추가하면 실행되는 함수 -> 스크립트를 넣다 뺄 경우가 잦을 때 사용하면 좋다.

    protected virtual void Reset()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        nav.speed = walkSpeed; // walkSpeed가 기본 스피드이므로.
        // Reset함수가 실행되면서 navMash와 걸려서 초기화 해줘야함. >> 목적지까지 이동시키는데 Reset함수와 걸리니 목적지를 제거해줌.
        nav.ResetPath();
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
        // RandomAction();
    }

    protected void Dead()
    {
        PlaySE(sound_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }
    

    protected void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }

    protected void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }

    protected void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("걷기");
    }

    

    public virtual void Demage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySE(sound_Hurt);
            anim.SetTrigger("Hurt");
            // Run(_targetPos);

        }

    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }



}
