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
            //myMesh[1] = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>(); //������Ʈ ���� ���� ���� ��
            //myMesh[1] = transform.GetChild(1).GetComponent<MeshRenderer>(); //���� ��

            currentSpeed = walkSpeed;
            //myColor = (MyColor)(Random.Range(0, 3));
            //color = Color.white; //���� ���� ���� �ʿ�
            //SetTextColor();
            //ChangeColor();
        }

    }

    public void SetColor(MyColor myColor, Color color)
    {
        this.myColor = myColor;
        myMesh[0].materials[0].color = color; //�÷��̾� �� ���� ����
        //myMesh[1].materials[0].color = color;
        SetTextColor();
    }

    public void SetLayer(int layerIndex)
    {
        this.gameObject.layer = layerIndex;
    }

}
