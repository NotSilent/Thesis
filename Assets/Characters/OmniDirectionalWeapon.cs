using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OmniDirectionalWeapon : Weapon
{
    [SerializeField] int numberOfBullets = 2;

    public override void Fire(Vector3 startingPosition, Vector3 target, NetworkIdentity networkIdentityToIgnore)
    {
        float angleBetweenBullets = 360f / numberOfBullets;
        for (int i = 0; i < numberOfBullets; i++)
        {
            CmdSpawnBullet(transform.position + Vector3.up, Quaternion.AngleAxis(angleBetweenBullets * i, Vector3.up) * (target - startingPosition), networkIdentityToIgnore);
        }
    }

    public void IncreaseBullets()
    {
        numberOfBullets++;
    }
}
