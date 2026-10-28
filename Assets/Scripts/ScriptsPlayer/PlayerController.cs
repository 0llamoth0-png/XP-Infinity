using UnityEngine;
using UnityEngine.Events;

//CONECTA TODOS LOS SERVICIOS DE EL JUGADOR 
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputs inputs;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerDash playerDash;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] PlayerHP playerHP;

    public UnityEvent ItemGet;

    public void FixedUpdate()
    {
        if (movement == null)
        {
            return;
        }

        Vector2 direction = inputs == null ? Vector2.zero : inputs.Direction;
        if (playerDash != null && playerDash.IsDashing)
        {
            movement.MoveDash(playerDash.Direction, playerDash.Speed);
            return;
        }

        movement.Move(direction);
    }

    public void Update()
    {
        if (inputs == null)
        {
            return;
        }

        Vector3 aimPoint = GetAimPoint();
        if (movement != null)
        {
            Vector3 aimDirection = aimPoint - transform.position;
            aimDirection.y = 0f;
            movement.Rotate(aimDirection);
        }

        if (playerDash != null && inputs.Dash != null && inputs.Dash.WasPressedThisFrame())
        {
            Vector2 facingDirection = new Vector2(transform.forward.x, transform.forward.z);
            playerDash.StartDash(inputs.Direction, facingDirection);
        }

        if (playerAttack != null && inputs.Attack != null && inputs.Attack.WasPressedThisFrame())
        {
            playerAttack.TryAttack(aimPoint);
        }
    }

    Vector3 GetAimPoint()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Ray aimRay = mainCamera.ScreenPointToRay(inputs.MousePosition);
            Plane aimPlane = new Plane(Vector3.up, transform.position);
            if (aimPlane.Raycast(aimRay, out float distance))
            {
                return aimRay.GetPoint(distance);
            }
        }

        return transform.position + transform.forward * 20f;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other != null && other.CompareTag("Apple"))
        {
            ItemGet?.Invoke();
        }
    }
}