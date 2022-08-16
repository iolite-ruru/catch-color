using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    //�÷��̾� �⺻ ����
    public int id;
    public new string name;

    //���� ���� ����
    public MyColor myColor;
    public Color color;

    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float currentSpeed;

    [SerializeField]
    private float jumpForce;

    //���� ����
    //private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //������ üũ
    private Vector3 lastPos;

    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶�
    [SerializeField]
    private float cameraRatationLimit; //x�� ���� ������ ����(����)
    private float currentCameraRotationX = 0; //����

    //�ʿ��� ������Ʈ
    //[SerializeField]
    protected Camera cam;
    protected Rigidbody myRigid;
    protected CapsuleCollider myCollider;
    protected MeshRenderer[] myMesh = new MeshRenderer[2];

    //UI ������Ʈ
    [SerializeField]
    private Text textColor;

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
            myMesh[1] = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>(); //������Ʈ ���� ���� ���� ��
                                                                                        //myMesh[1] = transform.GetChild(1).GetComponent<MeshRenderer>(); //���� ��

            currentSpeed = walkSpeed;
            color = Color.white;
            SetTextColor();

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

    public void SetTextColor()
    {
        //myMesh[0].materials[0].color = color; //�÷��̾� �� ���� ����
        //myMesh[1].materials[0].color = color;
        textColor.text = myColor.ToString(); //UI ���
    }

    public virtual void ChangeColor(int layerIndex)
    {
        Debug.Log("===parent");
        //cam.cullingMask = ~(1 << LayerMask.NameToLayer("Runnagate_Red"));
        cam.cullingMask = ~(1 << layerIndex);
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
        //Time.deltaTime(�� 0.016)
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
