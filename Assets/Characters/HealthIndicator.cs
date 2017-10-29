using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] Image image;

    CharacterDamageable health;

    private void Awake()
    {
        health = GetComponentInParent<CharacterDamageable>();
    }

    private void OnEnable()
    {
        health.EventOnDamageTaken += Health_EventOnDamageTaken;
    }

    private void OnDisable()
    {
        health.EventOnDamageTaken -= Health_EventOnDamageTaken;
    }

    private void Health_EventOnDamageTaken(float currentHealth, float maxHealth)
    {
        float currentHealthAsPercentage = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        image.fillAmount = currentHealthAsPercentage;
    }
}
