using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ExperienceSpawner : NetworkBehaviour
{
    [SerializeField] Experience experience;
    [SerializeField] float time = 2f;
    [SerializeField] int ticks = 10;

    public void Init(float experience, Vector3 position)
    {
        transform.position = position;
        StartCoroutine(SpawnExperience(experience / time * ticks, time / ticks));
    }

    IEnumerator SpawnExperience(float experiencePerSpawn, float deltaTick)
    {
        Player[] players = FindObjectsOfType<Player>();

        var orderedPlayers = players.OrderBy(p => Vector3.Distance(transform.position, p.transform.position));
        Player player = orderedPlayers.First<Player>();

        for (int i = 0; i < ticks; i++)
        {
            GameObject newExperience = Instantiate(experience.gameObject) as GameObject;
            newExperience.transform.SetParent(null);
            newExperience.GetComponent<Experience>()?.Init(player.gameObject, transform.position);
            NetworkServer.Spawn(newExperience);
            yield return new WaitForSeconds(deltaTick);
        }
        Destroy(gameObject);
    }
}