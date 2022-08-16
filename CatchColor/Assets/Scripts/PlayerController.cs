using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    //플레이어 기본 정보
    public int id;
    public new string name;

    //색깔 관련 변수
    public MyColor myColor;
    public Color color;

    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    private float currentSpeed;

    [SerializeField]
    private float jumpForce;

    //상태 변수
    //private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //움직임 체크
    private Vector3 lastPos;

    //카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라
    [SerializeField]
    private float cameraRatationLimit; //x축 기준 움직임 제한(상하)
    private float currentCameraRotationX = 0; //정면

    //필요한 컴포넌트
    //[SerializeField]
    protected Camera cam;
    protected Rigidbody myRigid;
    protected CapsuleCollider myCollider;
    protected MeshRenderer[] myMesh = new MeshRenderer[2];

    //UI 컴포넌트
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
            myMesh[1] = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>(); //오브젝트 계층 구조 변경 전
                                                                                        //myMesh[1] = transform.GetChild(1).GetComponent<MeshRenderer>(); //변경 후

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
        //myMesh[0].materials[0].color = color; //플레이어 모델 색상 변경
        //myMesh[1].materials[0].color = color;
        textColor.text = myColor.ToString(); //UI 출력
    }

    public virtual void ChangeColor(int layerIndex)
    {
        Debug.Log("===parent");
        //cam.cullingMask = ~(1 << LayerMask.NameToLayer("Runnagate_Red"));
        cam.cullingMask = ~(1 << layerIndex);
    }

    protected void IsGround()
    {
        //여기서 -transfrom.up 을 쓰게 된다면 문제가 생김. 고정된 값인 벡터 사용할 것
        //(현재 위치, 목표 방향, 이동할 거리)
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
    //달리기 시도
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
    //달리기 실행
    protected void Running()
    {
        isRun = true;
        //isWalk = false;
        currentSpeed = runSpeed;
    }
    //달리기 취소
    protected void RunningCancel()
    {
        isRun = false;
        //isWalk = true;
        currentSpeed = walkSpeed;
    }

    //움직임 실행
    protected void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * currentSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.smoothDeltaTime);
        //Time.deltaTime(약 0.016)
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
        //상하 카메라 회전
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity; //민감도 조절
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRatationLimit, cameraRatationLimit);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    protected void CharacterRotation()
    {
        //좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //내부에서는 쿼터늄(4원소) 값으로 이루어짐. 우리가 보기 편하게 오일러(3원소) 값으로 표기함.
    }
}
