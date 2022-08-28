using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CharacterMover : NetworkBehaviour
{

    //���� �÷��̾� ĳ����
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

    //���ǵ� ���� ����
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;

    protected float currentSpeed;

    [SerializeField]
    protected float jumpForce;

    //���� ����
    private bool isWalk = false;
    protected bool isRun = false;
    protected bool isGround = true;
    protected bool isMovable = true;
    public bool isChangeColor = false;

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
    [SerializeField]
    public Camera cam;

    protected Rigidbody myRigid;
    protected BoxCollider myCollider;

    //�������
    [SerializeField]
    protected new Renderer renderer;

    [SerializeField]
    [SyncVar(hook =nameof(SetLayerIndex_Hook))]
    public int layer = 6; //Player ���̾�

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
        isWalk = false;
        anim.SetBool("Run", isRun);
        anim.SetBool("Walk", isWalk);

        currentSpeed = runSpeed;
    }
    //�޸��� ���
    protected void RunningCancel()
    {
        isRun = false;
        isWalk = true;
        anim.SetBool("Run", isRun);
        anim.SetBool("Walk", isWalk);
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
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) isWalk = true;
            else isWalk = false;
            anim.SetBool("Walk", isWalk);
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


    //���� ����
    [Command]
    protected void CmdSetPlayerCharacter(MyColor color)
    {
        playerColor = color;
    }
}
