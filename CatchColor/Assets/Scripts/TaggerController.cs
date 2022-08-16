using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggerController : PlayerController
{

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

    public override void ChangeColor(int layerIndex)
    {
        //   cam.cullingMask = ~(1 << layerIndex);
        //cam.cullingMask = 0;
        cam.cullingMask = ~(1 << LayerMask.NameToLayer("Runnagate_Red"));
    }

    private void TryAttack()
    {

    }

    private void Attack()
    {

    }
}
