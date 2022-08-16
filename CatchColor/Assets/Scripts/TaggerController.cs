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

    private void TryAttack()
    {

    }

    private void Attack()
    {

    }
}
