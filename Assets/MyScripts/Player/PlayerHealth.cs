using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int MaxHP = 3;
    public int CurrentHP;
    public GameOverManager GameOverManager;
    public DamageVignette DamageVignette;
    public PlayerSound PlayerSound;

    [Header("HP Heal Settings")]
    public float healInterval = 5.0f;
    public int healAmount = 1;
    public float healDelay = 10.0f;

    private float healTimer = 0f;
    private float damageTimer = 0f;
    private bool canHeal = false;

    void Start()
    {
        CurrentHP = MaxHP;
    }

    void Update()
    {
        if(!canHeal)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer >= healDelay)
            {
                canHeal = true;
            }
        }
        else
        {
            healTimer += Time.deltaTime;
            if(healTimer >= healInterval && CurrentHP < MaxHP)
            {
                healTimer = 0f;
                CurrentHP = Mathf.Min(CurrentHP + healAmount, MaxHP);

                DamageVignette.OnDamage(CurrentHP, MaxHP);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Player HP: " + CurrentHP);

        CurrentHP -= amount;
        DamageVignette.OnDamage(CurrentHP, MaxHP);
        PlayerSound.PlayGrowl();

        canHeal = false;
        damageTimer = 0f;
        healTimer = 0f;

        if(CurrentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Dead");

        PlayerSound.PlayDeath();
        GameOverManager.AmbientSoundManager.StopAllBGM();
        
        var fpsController = GetComponent<FpsHorrorKit.FpsController>();
        if(fpsController != null)
        {
            fpsController.enabled = false;
        }

        var controller = GetComponent<CharacterController>();
        if(controller != null)
        {
            controller.enabled = false;
        }   
        
        var playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if(playerInput != null)
        {
            playerInput.enabled = false;
        }

        GameOverManager.TriggerGameOver();
    }
}
