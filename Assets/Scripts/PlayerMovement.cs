using System.IO.Compression;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
   public void Move (Vector2 direction)
   {
        Vector3 velocity = new Vector3(direction.x, 0, direction.y) * speed;
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