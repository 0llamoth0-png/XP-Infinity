using UnityEngine;
using System;

public class PlayerHP : MonoBehaviour
{
    public float vida = 3f;

    PlayerProtect playerProtect;

    public event Action playerDamaged;

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
        Debug.Log("Player took damage. Current HP: " + vida);
        playerDamaged?.Invoke();
        if (vida == 0f)
        {
            Destroy(gameObject);
        }
        return true;
    }
}
