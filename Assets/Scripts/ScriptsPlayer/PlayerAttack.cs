using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField, Min(0.05f)] float attackCooldown = 0.25f;
    [SerializeField, Min(0.1f)] float projectileSpeed = 25f;
    [SerializeField, Min(0.1f)] float projectileRange = 35f;
    [SerializeField, Min(0.1f)] float projectileSize = 0.6f;
    [SerializeField] LayerMask enemyLayer = ~0;

    [SerializeField] AudioClip shootClip;
      [SerializeField] AudioSource source;

    PlayerProtect playerProtect;
    float nextAttackTime;

    void Awake()
    {
        playerProtect = GetComponent<PlayerProtect>();
    }

    public bool TryAttack()
    {
        return TryAttack(transform.position + transform.forward * 20f);
    }

    public bool Attack()
    {
        return TryAttack();
    }

    public bool TryAttack(Vector3 aimPoint)
    {
        if ((playerProtect != null && playerProtect.IsProtecting) || Time.time < nextAttackTime)
        {
            return false;
        }

        Vector3 direction = aimPoint - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude <= 0.0001f)
        {
            direction = transform.forward;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude <= 0.0001f)
        {
            return false;
        }

        nextAttackTime = Time.time + Mathf.Max(attackCooldown, 0.05f);
        StartCoroutine(ShootBox(direction.normalized));
        return true;
    }

    IEnumerator ShootBox(Vector3 direction)
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        projectile.name = "PlayerAttackProjectile";
        projectile.transform.position = transform.position + Vector3.up * 0.5f;
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        projectile.transform.localScale = Vector3.one * projectileSize;
        source.PlayOneShot(shootClip);
        projectile.GetComponent<Renderer>().material.color = Color.cyan;
        Destroy(projectile.GetComponent<Collider>());

        Vector3 halfExtents = Vector3.one * (projectileSize * 0.5f);
        float distanceTravelled = 0f;
        while (distanceTravelled < projectileRange)
        {
            float step = Mathf.Min(projectileSpeed * Time.fixedDeltaTime, projectileRange - distanceTravelled);
            projectile.transform.position += direction * step;
            Collider[] hits = Physics.OverlapBox(
                projectile.transform.position,
                halfExtents,
                projectile.transform.rotation,
                enemyLayer.value,
                QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                EnemyHP enemyHP = hit.GetComponentInParent<EnemyHP>();
                if (enemyHP != null)
                {
                    enemyHP.TakeDamage(1);
                    Destroy(projectile);
                    yield break;
                }
            }

            distanceTravelled += step;
            yield return new WaitForFixedUpdate();
        }

        Destroy(projectile);
    }
}