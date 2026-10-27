using UnityEngine;
public class MovimientoVagon : MonoBehaviour
{
    [SerializeField] float speed;
    public void Update()
    {
        transform.Translate (new Vector3(-speed,0,0)*Time.deltaTime);

        if (transform.position.x <=-160)
        {
            transform.position = new Vector3 (83, 0, 0);
        }
    }
}
