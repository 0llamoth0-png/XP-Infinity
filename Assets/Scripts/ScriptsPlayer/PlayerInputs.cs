using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-9999)]
//INPUTS DEL JUGADOR        
public class PlayerInputs : MonoBehaviour
{
    InputSystem_Actions inputs;
    public InputAction Move;
    public InputAction Attack;
    public InputAction Dash;
    public InputAction Protect;
    public InputAction Aim;

    public Vector2 Direction => Move != null ? Move.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 MousePosition => Aim != null ? Aim.ReadValue<Vector2>() : Vector2.zero;

    private void Awake()
    {
        //INSTANCIA DE LOS INPUTS
        inputs = new InputSystem_Actions();
        //REFERENCIAs de los mismos 
        Move = inputs.Player .Move;
        Attack = inputs.Player.Attack;
        Dash = inputs.Player.Dash;
        Protect = inputs.Player.Protect;
        Aim = inputs.Player.Aim;
    }

    private void OnEnable ()
    {
        if (inputs != null)
        {
            inputs.Enable();
        }
    }

    private void Start ()
    {
        if (inputs == null)
        {
            Awake();
        }
    }

    private void OnDisable()
    {
        if (inputs != null)
        {
            inputs.Disable();
        }
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Dispose();
            inputs = null;
        }
    }
}