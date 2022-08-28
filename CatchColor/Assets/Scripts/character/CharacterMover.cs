using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CharacterMover : NetworkBehaviour
{

    //현재 플레이어 캐릭터
    private static CharacterMover myPlayer;
    public static CharacterMover MyPlayer
    {
        get
        {
            if (myPlayer == null)
            {
                var players = FindObjectsOfType<CharacterMover>();
                foreach (var player in players)
                {
                    if (player.hasAuthority)
                    {
                        myPlayer = player;
                    }
                }
            }
            return myPlayer;
        }
    }

    public Animator anim;

    //스피드 조정 변수
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;

    protected float currentSpeed;

    [SerializeField]
    protected float jumpForce;

    //상태 변수
    private bool isWalk = false;
    protected bool isRun = false;
    protected bool isGround = true;
    protected bool isMovable = true;
    public bool isChangeColor = false;

    //움직임 체크
    protected Vector3 lastPos;

    //카메라 민감도
    [SerializeField]
    protected float lookSensitivity;

    //카메라
    [SerializeField]
    protected float cameraRatationLimit; //x축 기준 움직임 제한(상하)
    protected float currentCameraRotationX = 0; //정면

    //필요한 컴포넌트
    [SerializeField]
    public Camera cam;

    protected Rigidbody myRigid;
    protected BoxCollider myCollider;

    //색상관련
    [SerializeField]
    protected new Renderer renderer;

    [SerializeField]
    [SyncVar(hook =nameof(SetLayerIndex_Hook))]
    public int layer = 6; //Player 레이어

    public virtual void SetLayerIndex_Hook(int oldLayer,int newLayer)
    {
        Debug.Log("Parent >> Test : "+ newLayer);
    }

    [SyncVar(hook =nameof(SetPlayerColor_Hook))]
    public MyColor playerColor;
    public void SetPlayerColor_Hook(MyColor oldColor, MyColor newColor)
    {
        if (renderer == null)
        {
            renderer = gameObject.GetComponent<Renderer>();
        }
        renderer.material.color = PlayerColor.GetColor(newColor);
        SetLayer(PlayerColor.GetColorInt(newColor)+7);
    }

    [Command]
    public void CmdSetColor(MyColor color)
    {
        playerColor = color;
    }

    public virtual void SetLayer(int idx)
    {
        Debug.Log("===Parent");
    }

    public virtual void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;

        //renderer = gameObject.GetComponent<MeshRenderer>();


        if (hasAuthority)
        {
            isChangeColor = true;
            //cam = Camera.main;
            //cam.transform.SetParent(transform);
            //cam.transform.localPosition = new Vector3(0f, 1f, 0f);
            //cam.cullingMask = ~(1 << 7);
            //cam.cullingMask = ~(1<<LayerMask.NameToLayer("Runnagate_Red"));
            myCollider = GetComponent<BoxCollider>();
            myRigid = GetComponent<Rigidbody>();

            currentSpeed = walkSpeed;
        }
        else
        {
            cam.enabled = false;
        }

    }

    public virtual void Update()
    {
        if (isMovable&&hasAuthority)
        {
            IsGround();
            TryJump();
            TryRun();
            Move();
            MoveCheck();
            //CameraRotation();
            //CharacterRotation();
        }

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
        isWalk = false;
        anim.SetBool("Run", isRun);
        anim.SetBool("Walk", isWalk);

        currentSpeed = runSpeed;
    }
    //달리기 취소
    protected void RunningCancel()
    {
        isRun = false;
        isWalk = true;
        anim.SetBool("Run", isRun);
        anim.SetBool("Walk", isWalk);
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
    }
    protected void MoveCheck()
    {
        if (!isRun && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) isWalk = true;
            else isWalk = false;
            anim.SetBool("Walk", isWalk);
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


    //색상 변경
    [Command]
    protected void CmdSetPlayerCharacter(MyColor color)
    {
        playerColor = color;
    }
}
