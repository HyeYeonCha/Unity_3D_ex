using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour {

    [SerializeField]
    private float viewAngle; // 시야각
    [SerializeField]
    private float viewDistanse; // 시야거리
    [SerializeField]
    private LayerMask targetMask; // 타겟 마스크

    private Pig thePig;

    private void Start()
    {
        thePig = GetComponent<Pig>();
    }

    // Update is called once per frame
    void Update () {
        View();
	}

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        // 삼각함수 이용 (Deg2Rad == 180/파이)
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistanse, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {

                Vector3 _direction = (_targetTf.position - transform.position).normalized;

                // 돼지의 시야각을 이등분 했을 때 플레이어와의 각도 
                float _angle = Vector3.Angle(_direction, transform.forward);

                if(_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hit;
                    if(Physics.Raycast(transform.position + transform.up, _direction, out _hit, viewDistanse))
                    {
                        if(_hit.transform.name == "Player")
                        {
                            print("player in view");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                    }
                }


            }
        }

    }

}
