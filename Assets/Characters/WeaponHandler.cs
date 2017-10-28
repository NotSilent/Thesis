using UnityEngine;
using UnityEngine.Networking;

class WeaponHandler : NetworkBehaviour
{
    [SerializeField] Weapon currentWeapon;
    [SerializeField] GameObject projectileStartPos;
    
    public void FireCurrentWeapon(Vector3 direction, NetworkIdentity networkIdentityToIgnore)
    {
        currentWeapon.CmdSpawnBullet(projectileStartPos.transform.position, direction, networkIdentityToIgnore);
    }
}