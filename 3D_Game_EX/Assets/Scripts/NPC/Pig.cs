using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeekAnimal {


    protected override void Reset()
    {
        base.Reset();
        RandomAction();
    }

    protected void RandomAction()
    {

        RandomSound();

        // 최소값 포함 최대값 미포함.
        int _random = Random.Range(0, 4); // 대기, 풀뜯기, 두리번, 걷기. >> (0 ~ 3) 근데 마지막에 (0, 4f) 해주면 0 ~ 4 됨.!

        if (_random == 0)
        {
            Wait();
        }
        else if (_random == 1)
        {
            Eat();
        }
        else if (_random == 2)
        {
            Peek();
        }
        else if (_random == 3)
        {
            TryWalk();
        }


    }

    protected void RandomSound()
    {
        int _random = Random.Range(0, 3); // 일상 사운드 3개
        PlaySE(sound_Normal[_random]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }


}
