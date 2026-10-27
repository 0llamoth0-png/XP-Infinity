using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

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
        movement.Move(inputs.Direction);
    }



    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Apple"))
        {
            ItemGet.Invoke();
        }

    }
}