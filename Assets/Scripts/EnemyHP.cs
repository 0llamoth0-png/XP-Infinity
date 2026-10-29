using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField, Min(1)] int vida = 1;

    int baseVida;
    int vidaMaxima;
    bool isDead;
    Renderer enemyRenderer;
    Collider enemyCollider;
    Camera viewCamera;

    public int CurrentHealth => vida;
    public int MaxHealth => vidaMaxima;

    void Awake()
    {
        baseVida = Mathf.Max(1, vida);
        vida = baseVida;
        vidaMaxima = baseVida;
        enemyRenderer = GetComponentInChildren<Renderer>();
        enemyCollider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        KillCounter.KillCountChanged += UpdateHealthForKillCount;
    }

    void OnDisable()
    {
        KillCounter.KillCountChanged -= UpdateHealthForKillCount;
    }

    void Start()
    {
        UpdateHealthForKillCount(KillCounter.Kills);
    }

    public void TakeDamage(int cantidad)
    {
        if (cantidad <= 0 || isDead)
        {
            return;
        }

        vida = Mathf.Max(0, vida - cantidad);
        if (vida == 0)
        {
            isDead = true;
            if (KillCounter.Instance != null)
            {
                KillCounter.Instance.RegisterKill();
            }
            else
            {
                Debug.LogError("EnemyHP requires an active KillCounter to register kills.", this);
            }

            Destroy(gameObject);
        }
    }

    void UpdateHealthForKillCount(int killCount)
    {
        if (isDead)
        {
            return;
        }

        int healthTier = killCount / 10;
        int newMaximumHealth = Mathf.CeilToInt(baseVida * (1f + healthTier * 0.5f));
        int healthIncrease = newMaximumHealth - vidaMaxima;
        vida = Mathf.Min(newMaximumHealth, vida + healthIncrease);
        vidaMaxima = newMaximumHealth;
    }

    void OnGUI()
    {
        if (isDead || Event.current.type != EventType.Repaint)
        {
            return;
        }

        if (viewCamera == null)
        {
            viewCamera = Camera.main;
        }

        if (viewCamera == null)
        {
            return;
        }

        float topY = transform.position.y;
        if (enemyRenderer != null)
        {
            topY = enemyRenderer.bounds.max.y;
        }
        else if (enemyCollider != null)
        {
            topY = enemyCollider.bounds.max.y;
        }

        Vector3 screenPosition = viewCamera.WorldToScreenPoint(new Vector3(
            transform.position.x,
            topY + 0.25f,
            transform.position.z));
        if (screenPosition.z <= 0f)
        {
            return;
        }

        const float barWidth = 80f;
        const float barHeight = 10f;
        float screenY = Screen.height - screenPosition.y;
        Rect barRect = new Rect(screenPosition.x - barWidth * 0.5f, screenY, barWidth, barHeight);
        Rect healthRect = new Rect(barRect.x, barRect.y, barWidth * ((float)vida / vidaMaxima), barHeight);

        Color originalColor = GUI.color;
        GUI.color = Color.black;
        GUI.DrawTexture(barRect, Texture2D.whiteTexture);
        GUI.color = vida * 2 <= vidaMaxima ? Color.red : Color.green;
        GUI.DrawTexture(healthRect, Texture2D.whiteTexture);
        GUI.color = originalColor;

        Rect labelRect = new Rect(barRect.x - 10f, barRect.y - 20f, barWidth + 20f, 20f);
        GUI.Label(labelRect, $"{vida}/{vidaMaxima}");
    }
}
