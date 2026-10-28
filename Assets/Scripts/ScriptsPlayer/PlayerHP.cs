using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float vida = 3f;

    PlayerProtect playerProtect;

    void Awake()
    {
        playerProtect = GetComponent<PlayerProtect>();
    }

    public bool TakeDamage(int cantidad)
    {
        if (cantidad <= 0 || vida <= 0 || (playerProtect != null && playerProtect.IsProtecting))
        {
            return false;
        }

        vida = Mathf.Max(0f, vida - cantidad);
        if (vida == 0f)
        {
            Destroy(gameObject);
        }

        return true;
    }
}
