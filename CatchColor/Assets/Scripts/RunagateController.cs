using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunagateController : PlayerController
{
    void Start()
    {
        if (hasAuthority)
        {
            cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 1f, 0f);
            myCollider = GetComponent<CapsuleCollider>();
            myRigid = GetComponent<Rigidbody>();
            myMesh[0] = GetComponent<MeshRenderer>();
            //myMesh[1] = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>(); //오브젝트 계층 구조 변경 전
            //myMesh[1] = transform.GetChild(1).GetComponent<MeshRenderer>(); //변경 후

            currentSpeed = walkSpeed;
            //myColor = (MyColor)(Random.Range(0, 3));
            //color = Color.white; //시작 랜덤 배정 필요
            //SetTextColor();
            //ChangeColor();
        }

    }

    public void SetColor(MyColor myColor, Color color)
    {
        this.myColor = myColor;
        myMesh[0].materials[0].color = color; //플레이어 모델 색상 변경
        //myMesh[1].materials[0].color = color;
        SetTextColor();
    }

    public void SetLayer(int layerIndex)
    {
        this.gameObject.layer = layerIndex;
    }

}
