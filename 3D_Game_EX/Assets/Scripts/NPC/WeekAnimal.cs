using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekAnimal : Animal {


    public void Run(Vector3 _targetPos)
    {
        // 플레이어의 반대 방향을 보게하는 부분.
        // eulerAngles 없으면 그냥 Quaternion
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized; 
        // 정규화해줬기 때문에 합이 1밖에 안됨.
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        nav.speed = runSpeed;
        anim.SetBool("Running", isRunning);

    }

    public override void Demage(int _dmg, Vector3 _targetPos)
    {
        base.Demage(_dmg, _targetPos);
        if(!isDead)
        {
            Run(_targetPos);
        }
    }


}
