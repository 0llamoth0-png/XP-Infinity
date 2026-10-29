using UnityEngine;
using System;

public class PlayerHP : MonoBehaviour
{
    public float vida = 3f;

    [SerializeField] Sprite[] heartSprites = new Sprite[3];

    PlayerProtect playerProtect;

    public event Action playerDamaged;

    void Awake()
    {
        playerProtect = GetComponent<PlayerProtect>();
    }
    

    public bool TakeDamage(float cantidad)
    {
        if (cantidad <= 0 || vida <= 0 || (playerProtect != null && playerProtect.IsProtecting))
        {
            return false;
        }

        vida = Mathf.Max(0f, vida - cantidad);
        Debug.Log("Player took damage. Current HP: " + vida);
        playerDamaged?.Invoke();
        if (vida == 0f)
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.LoseGame();
            }
            else
            {
                Debug.LogWarning("GameManager not found in the scene.");
            }
            Destroy(gameObject, 1.5f);
        }
        return true;
    }

    void OnGUI()
    {
        if (heartSprites == null || Event.current.type != EventType.Repaint)
        {
            return;
        }

        Color originalColor = GUI.color;
        int visibleHearts = Mathf.Min(Mathf.CeilToInt(vida), heartSprites.Length);
        const float heartSize = 42f;
        const float heartSpacing = 8f;
        const float left = 20f;
        const float top = 20f;

        for (int i = 0; i < visibleHearts; i++)
        {
            Sprite heart = heartSprites[i];
            if (heart == null)
            {
                continue;
            }

            Rect spriteRect = heart.textureRect;
            Rect uvRect = new Rect(
                spriteRect.x / heart.texture.width,
                spriteRect.y / heart.texture.height,
                spriteRect.width / heart.texture.width,
                spriteRect.height / heart.texture.height);
            Rect screenRect = new Rect(left + i * (heartSize + heartSpacing), top, heartSize, heartSize);
            GUI.DrawTextureWithTexCoords(screenRect, heart.texture, uvRect);
        }

        GUI.color = originalColor;
    }
}
