using UnityEngine;

public class PlayerHP : MonoBehaviour
{
public float vida = 3f;
    void Update()
    {
        if (vida <= 0)
        {
            Destroy(GameObject.FindWithTag("Player"));
        }
    }
}
