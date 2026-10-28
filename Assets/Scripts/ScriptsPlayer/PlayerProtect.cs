using UnityEngine;

public class PlayerProtect : MonoBehaviour
{
    [SerializeField] GameObject shield;

    PlayerInputs inputs;

    public bool IsProtecting => inputs != null && inputs.Protect != null && inputs.Protect.IsPressed();

    void Awake()
    {
        inputs = GetComponent<PlayerInputs>();

        if (shield == null)
        {
            Transform shieldTransform = transform.Find("Shield");
            if (shieldTransform != null)
            {
                shield = shieldTransform.gameObject;
            }
        }

        if (inputs == null)
        {
            Debug.LogError("PlayerProtect requires a PlayerInputs component.", this);
        }

        if (shield == null)
        {
            Debug.LogError("PlayerProtect requires a Shield object on the player.", this);
            return;
        }

        shield.SetActive(false);
    }

    void Update()
    {
        if (shield != null)
        {
            shield.SetActive(IsProtecting);
        }
    }
}