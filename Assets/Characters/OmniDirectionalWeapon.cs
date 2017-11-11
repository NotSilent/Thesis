using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OmniDirectionalWeapon : Weapon
{
    [SerializeField] int numberOfBullets = 2;

    public override void Fire(Vector3 startingPosition, Vector3 direction, NetworkIdentity networkIdentityToIgnore)
    {
        float angleBetweenBullets = 360f / numberOfBullets;
        //float angleToDirection = Vector3.Angle(transform.forward, direction);
        float angleToDirection = SignedAngle(transform.forward, direction);
        for (int i = 0; i < numberOfBullets; i++)
        {
            CmdSpawnBullet(transform.position + Vector3.up, Quaternion.AngleAxis(angleBetweenBullets * i + angleToDirection, Vector3.up) * transform.forward, networkIdentityToIgnore);
        }
    }

    float SignedAngle(Vector3 from, Vector3 to)
    {
        float angle = Vector3.Angle(from, to);
        float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(from, to)));
        
        float signedAngle = angle * sign;

        return signedAngle;
    }

    public void IncreaseBullets()
    {
        numberOfBullets++;
    }
}
