using UnityEngine;

public class PhysicsCollisionDetector : MonoBehaviour
{
    [SerializeField] Transform pivot;
    [SerializeField] Vector2 size;
    [SerializeField] LayerMask layers;

    public Collider[] GetCollision()
    {
       return Physics.OverlapBox(pivot.position,size,Quaternion.identity, layers);
    }
    private void OnDrawGizmos()
    {
        Gizmos . DrawWireCube (pivot.position,size);
    }


}