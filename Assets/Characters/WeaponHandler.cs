using UnityEngine;
using UnityEngine.Networking;

class WeaponHandler : NetworkBehaviour
{
    [SerializeField] Weapon currentWeapon;
    [SerializeField] GameObject projectileStartPosition;
    [SerializeField] float cooldown = 1f;

    float currentCooldwon;

    void Start()
    {
        currentCooldwon = cooldown;
    }

    private void Update()
    {
        currentCooldwon += Time.deltaTime;
    }

    public void FireCurrentWeapon(Vector3 target, NetworkIdentity networkIdentityToIgnore)
    {
        if (currentCooldwon > cooldown)
        {
            currentWeapon.Fire(projectileStartPosition.transform.position,
                target, networkIdentityToIgnore);
            currentCooldwon = 0f;
        }
    }
}