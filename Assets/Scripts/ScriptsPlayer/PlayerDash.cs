using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField, Min(0.01f)] float duration = 0.5f;
    [SerializeField, Min(0f)] float speed = 15f;
    [SerializeField, Min(0f)] float cooldown = 1f;

    float timeRemaining;
    float nextDashTime;

    public bool IsDashing { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Speed => speed;

    public void StartDash(Vector2 direction)
    {
        StartDash(direction, Vector2.zero);
    }

    public void StartDash(Vector2 direction, Vector2 facingDirection)
    {
        if (IsDashing || Time.time < nextDashTime)
        {
            return;
        }

        if (direction.sqrMagnitude <= 0.0001f)
        {
            direction = facingDirection;
        }

        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        Direction = direction.normalized;
        timeRemaining = Mathf.Max(duration, 0.01f);
        nextDashTime = Time.time + Mathf.Max(cooldown, 1f);
        IsDashing = true;
    }

    private void Update()
    {
        if (!IsDashing)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            IsDashing = false;
        }
    }
}
