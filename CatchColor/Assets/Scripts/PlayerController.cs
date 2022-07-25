using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public MyColor myColor;
    public Color color;

    //[SerializeField]
    //private Material material;

    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float currentSpeed;

    [SerializeField]
    private float jumpForce;

    //���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //������ üũ
    private Vector3 lastPos;

    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� ���� ����
    [SerializeField]
    private float cameraRatationLimit; //����
    private float currentCameraRotationX = 0; //����

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera cam;
    private Rigidbody myRigid;
    private CapsuleCollider myCollider;
    private MeshRenderer[] myMesh = new MeshRenderer[2];

    //UI ������Ʈ
    [SerializeField]
    private Text textColor;

    void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        myMesh[0] = GetComponent<MeshRenderer>();
        myMesh[1] = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();

        currentSpeed = walkSpeed;
        color = Color.white;
        SetTextColor();

    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        Move();
        MoveCheck();
        CameraRotation();
        CharacterRotation();
    }

    public void SetTextColor()
    {
        myMesh[0].materials[0].color = color; //�÷��̾� �� ���� ����
        myMesh[1].materials[0].color = color;
        textColor.text = myColor.ToString(); //UI ���
    }

    private void IsGround()
    {
        //���⼭ -transfrom.up �� ���� �ȴٸ� ������ ����. ������ ���� ���� ����� ��
        //(���� ��ġ, ��ǥ ����, �̵��� �Ÿ�)
        isGround = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }
    //�޸��� �õ�
    private void TryRun()
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
    private void Running()
    {
        isRun = true;
        isWalk = false;
        currentSpeed = runSpeed;
    }
    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        isWalk = true;
        currentSpeed = walkSpeed;
    }

    //������ ����
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.smoothDeltaTime);
        //Time.deltaTime(�� 0.016)
    }
    private void MoveCheck()
    {
        if (!isRun && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) isWalk = true;
            else isWalk = false;
            lastPos = transform.position;

        }

    }
    private void CameraRotation()
    {
        //���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //�ΰ��� ����
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRatationLimit, cameraRatationLimit);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    private void CharacterRotation()
    {
        //�¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //���ο����� ���ʹ�(4����) ������ �̷����. �츮�� ���� ���ϰ� ���Ϸ�(3����) ������ ǥ����.
    }
}
