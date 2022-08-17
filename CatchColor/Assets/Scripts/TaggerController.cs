using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerController : PlayerController
{
    [SerializeField]
    private AttackController attackController;

    private bool isAttack = false;
    private bool isSwing = false;

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

        yield return new WaitForSeconds(attackController.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(attackController.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(attackController.attackDelay - attackController.attackDelayA - attackController.attackDelayB);
        
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (ChecekObject())
            {
                isSwing = false;
                Debug.Log("==충돌: " + hitInfo.transform.name);
            }
            yield return null;
        }
    }
    
    private bool ChecekObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, attackController.range)){
            return true;
        }
        return false;
    }

    private void Attack()
    {

    }
}
