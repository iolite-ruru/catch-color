using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CharacterMover : NetworkBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;

    protected float currentSpeed;

    [SerializeField]
    protected float jumpForce;

    //���� ����
    //private bool isWalk = false;
    protected bool isRun = false;
    protected bool isGround = true;

    //������ üũ
    protected Vector3 lastPos;

    //ī�޶� �ΰ���
    [SerializeField]
    protected float lookSensitivity;

    //ī�޶�
    [SerializeField]
    protected float cameraRatationLimit; //x�� ���� ������ ����(����)
    protected float currentCameraRotationX = 0; //����

    //�ʿ��� ������Ʈ
    //[SerializeField]
    protected Camera cam;
    protected Rigidbody myRigid;
    protected CapsuleCollider myCollider;

    //�������
    new Renderer renderer;

    [SyncVar(hook =nameof(SetPlayerColor_Hook))]
    public MyColor playerColor;
    public void SetPlayerColor_Hook(MyColor oldColor, MyColor newColor)
    {
        if (renderer == null)
        {
            renderer = gameObject.GetComponent<Renderer>();
        }
        renderer.material.color = Define.GetColor(newColor);

    }




    public virtual void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = Define.GetColor(playerColor);

        if (hasAuthority)
        {
            //cam = Camera.main;
            //cam.transform.SetParent(transform);
            //cam.transform.localPosition = new Vector3(0f, 1f, 0f);
            //cam.cullingMask = ~(1 << 7);
            //cam.cullingMask = ~(1<<LayerMask.NameToLayer("Runnagate_Red"));
            myCollider = GetComponent<CapsuleCollider>();
            myRigid = GetComponent<Rigidbody>();
                                                       
            currentSpeed = walkSpeed;

            

        }

    }

    void Update()
    {
        if (hasAuthority)
        {
            IsGround();
            TryJump();
            TryRun();
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }

    }


    protected void IsGround()
    {
        //���⼭ -transfrom.up �� ���� �ȴٸ� ������ ����. ������ ���� ���� ����� ��
        //(���� ��ġ, ��ǥ ����, �̵��� �Ÿ�)
        isGround = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }
    protected void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    protected void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }
    //�޸��� �õ�
    protected void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    //�޸��� ����
    protected void Running()
    {
        isRun = true;
        //isWalk = false;
        currentSpeed = runSpeed;
    }
    //�޸��� ���
    protected void RunningCancel()
    {
        isRun = false;
        //isWalk = true;
        currentSpeed = walkSpeed;
    }

    //������ ����
    protected void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.smoothDeltaTime);
    }
    protected void MoveCheck()
    {
        if (!isRun && isGround)
        {
            //if (Vector3.Distance(lastPos, transform.position) >= 0.01f) isWalk = true;
            //else isWalk = false;
            lastPos = transform.position;

        }

    }
    protected void CameraRotation()
    {
        //���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //�ΰ��� ����
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRatationLimit, cameraRatationLimit);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    protected void CharacterRotation()
    {
        //�¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //���ο����� ���ʹ�(4����) ������ �̷����. �츮�� ���� ���ϰ� ���Ϸ�(3����) ������ ǥ����.
    }
}
