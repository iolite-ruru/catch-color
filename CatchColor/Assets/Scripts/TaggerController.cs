using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerController : PlayerController
{
    //[SerializeField]
    //private AttackController attackController;

    private bool isAttack = false;
    private bool isSwing = false;

    public float range;
    public float damage;
    public float attackDelay;
    public float attackDelayA;
    public float attackDelayB;

    private RaycastHit hitInfo;

    void Update()
    {
        if (hasAuthority)
        {
            TryAttack();
            IsGround();
            TryJump();
            TryRun();
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }

    }

    public void ChangeColor(int layerIndex)
    {
        cam.cullingMask = ~(1 << layerIndex);
        Debug.Log("===child(Tagger)");
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        yield return new WaitForSeconds(attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(attackDelay - attackDelayA - attackDelayB);
        
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log("==충돌: " + hitInfo.transform.name);
            }
            yield return null;
        }
    }
    
    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, range)){
            return true;
        }
        return false;
    }

}
