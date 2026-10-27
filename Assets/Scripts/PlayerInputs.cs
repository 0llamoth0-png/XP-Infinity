using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-9999)]
//INPUTS DEL JUGADOR        
public class PlayerInputs : MonoBehaviour
{
    InputSystem_Actions inputs;
    public InputAction Move;
    public InputAction Attack;
    public Vector2 Direction { get => Move.ReadValue<Vector2>() ; }

    private void Awake()
    {
        //INSTANCIA DE LOS INPUTS
        inputs = new InputSystem_Actions();
        //REFERENCIAs de los mismos 
        Move = inputs.Player .Move;
        Attack = inputs.Player.Attack;
    }

    private void OnEnable ()
    {
        inputs.Enable ();
    }

    private void Start ()
    {


    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void OnDestroy()
    {
        inputs.Dispose();
    }
}