using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("LIMITES DE MOVIMIENTO")]
    public float limiteXMax;
    public float limiteXMin ;
    public float limiteZMax ;  
    public float limiteZMin ;
    //ATRIBUTOS
   [SerializeField] float speed;
   [SerializeField] Rigidbody rigid;

   public void Rotate (Vector3 direction)
   {
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000f * Time.deltaTime);
   }
   public void Move (Vector2 direction)
   {
        SetVelocity(direction, speed);
    }

   public void MoveDash (Vector2 direction, float dashSpeed)
   {
        SetVelocity(direction, dashSpeed);
   }

   void SetVelocity (Vector2 direction, float movementSpeed)
   {
        if (rigid == null)
        {
            return;
        }

        Vector3 velocity = new Vector3(direction.x, 0f, direction.y) * movementSpeed;
        float fixedDeltaTime = Time.fixedDeltaTime;

        velocity.x = PuntoMaximoDeVelocidad(transform.position.x, velocity.x, limiteXMin, limiteXMax, fixedDeltaTime);
        velocity.z = PuntoMaximoDeVelocidad(transform.position.z, velocity.z, limiteZMin, limiteZMax, fixedDeltaTime);
        rigid.linearVelocity = velocity;
    //la llave son atributos // y la caja son funciones 
   }
    float PuntoMaximoDeVelocidad(float position, float velocity, float minimum, float maximum, float fixedDeltaTime)
    {
        if ((position <= minimum && velocity < 0) || (position >= maximum && velocity > 0))
        {
            return 0;
        }

        float nextPosition = position + velocity * fixedDeltaTime;
        if (velocity > 0 && nextPosition > maximum)
        {
            return (maximum - position) / fixedDeltaTime;
        }

        if (velocity < 0 && nextPosition < minimum)
        {
            return (minimum - position) / fixedDeltaTime;
        }

        return velocity;
    }

}