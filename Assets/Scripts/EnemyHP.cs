using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] int vida = 3;

    public void TakeDamage(int cantidad)
    {
        if (cantidad <= 0 || vida <= 0)
        {
            return;
        }

        vida = Mathf.Max(0, vida - cantidad);
        if (vida == 0)
        {
            Destroy(gameObject);
        }
    }
}
