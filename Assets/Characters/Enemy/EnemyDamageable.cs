﻿using UnityEngine;
using UnityEngine.Networking;

public class EnemyDamageable : CharacterDamageable
{
    [SerializeField] ExperienceSpawner experienceSpawner;
    [SerializeField] float experience;

    protected override void OnDied()
    {
        SpawnExperience();
        base.OnDied();
    }

    private void SpawnExperience()
    {
        GameObject newExperienceSpawner = Instantiate(experienceSpawner.gameObject) as GameObject;
        newExperienceSpawner.transform.SetParent(null);
        newExperienceSpawner.GetComponent<ExperienceSpawner>().Init(experience, transform.position);
        NetworkServer.Spawn(newExperienceSpawner);
    }
}
