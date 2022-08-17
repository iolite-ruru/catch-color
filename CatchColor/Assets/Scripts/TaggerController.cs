using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerController : PlayerController
{

    void Update()
    {
        if (hasAuthority)
        {
            //TryAttack();
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

    }

    private void Attack()
    {

    }
}
