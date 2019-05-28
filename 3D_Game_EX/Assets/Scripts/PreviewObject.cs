using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour {

    // 충돌한 오브젝트의 콜라이더.
    private List<Collider> colliderLIst = new List<Collider>();

    [SerializeField]
    private int LayerGround; // 지상 레이어.
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ChangeColor();
	}

    private void ChangeColor()
    {
        if(colliderLIst.Count > 0 )
        {
            SetColor(red);
        }
        else
        {
            SetColor(green);
        }
    }

    private void SetColor(Material mat)
    {
        foreach(Transform tf_Child in this.transform)
        {
            var newMaterials = new Material[tf_Child.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tf_Child.GetComponent<Renderer>().materials = newMaterials;

        }
    }



    // 부딪히면 추가되고 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            colliderLIst.Add(other);
        }
       
    }

    // 부딪힌 객체로부터 나오면 사라진다.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderLIst.Remove(other);
    }


    public bool isBuildable()
    {
        return colliderLIst.Count == 0;
    }


}
