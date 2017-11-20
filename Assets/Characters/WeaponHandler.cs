using UnityEngine;
using UnityEngine.Networking;

class WeaponHandler : NetworkBehaviour
{
    [SerializeField] Weapon currentWeapon;
    [SerializeField] GameObject projectileStartPosition;
    
    public void FireCurrentWeapon(Vector3 target, NetworkIdentity networkIdentityToIgnore)
    {
        currentWeapon.Fire(projectileStartPosition.transform.position,
            target, networkIdentityToIgnore);
    }
}