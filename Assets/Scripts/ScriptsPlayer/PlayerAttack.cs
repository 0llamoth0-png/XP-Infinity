using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    enum ProjectileShape
    {
        Cube,
        Sphere,
        Capsule
    }

    [SerializeField, Min(0.05f)] float attackCooldown = 0.25f;
    [SerializeField, Min(0.1f)] float projectileSpeed = 25f;
    [SerializeField, Min(0.1f)] float projectileRange = 35f;
    [SerializeField, Min(0.1f)] float projectileSize = 0.6f;
    [Header("Apariencia del proyectil")]
    [SerializeField] ProjectileShape projectileShape = ProjectileShape.Cube;
    [Tooltip("Elige el color de la bala desde el selector de color.")]
    [SerializeField] Color projectileColor = Color.cyan;
    [Header("Estela de fuego")]
    [Tooltip("0 desactiva la estela; 5 usa la intensidad maxima.")]
    [SerializeField, Range(0f, 5f)] float fireTrailIntensity = 1f;
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
        GameObject projectile = new GameObject("PlayerAttackProjectile");
        projectile.transform.position = transform.position + Vector3.up * 0.5f;
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        projectile.transform.localScale = Vector3.one * projectileSize;
        GameObject visual = GameObject.CreatePrimitive(GetPrimitiveType());
        visual.name = "ProjectileVisual";
        visual.transform.SetParent(projectile.transform, false);
        visual.transform.localScale = Vector3.one;
        if (projectileShape == ProjectileShape.Capsule)
        {
            visual.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        }

        if (source != null && shootClip != null)
        {
            source.PlayOneShot(shootClip);
        }

        visual.GetComponent<Renderer>().material.color = projectileColor;
        Destroy(visual.GetComponent<Collider>());
        ParticleSystem fireTrail = CreateFireTrail(projectile);

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
                    DestroyProjectile(projectile, fireTrail);
                    yield break;
                }
            }

            distanceTravelled += step;
            yield return new WaitForFixedUpdate();
        }

        DestroyProjectile(projectile, fireTrail);
    }

    PrimitiveType GetPrimitiveType()
    {
        switch (projectileShape)
        {
            case ProjectileShape.Sphere:
                return PrimitiveType.Sphere;
            case ProjectileShape.Capsule:
                return PrimitiveType.Capsule;
            default:
                return PrimitiveType.Cube;
        }
    }

    ParticleSystem CreateFireTrail(GameObject projectile)
    {
        if (fireTrailIntensity <= 0f)
        {
            return null;
        }

        GameObject trailObject = new GameObject("ProjectileFireTrail");
        trailObject.transform.SetParent(projectile.transform, false);
        trailObject.transform.localPosition = Vector3.back * (projectileSize * 0.5f);
        ParticleSystem trail = trailObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = trail.main;
        main.loop = true;
        main.startLifetime = 0.35f;
        main.startSpeed = 0.15f;
        main.startSize = 0.08f + fireTrailIntensity * 0.025f;
        main.startColor = new Color(1f, 0.35f, 0.04f, 0.9f);
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = trail.emission;
        emission.rateOverTime = 30f * fireTrailIntensity;

        ParticleSystem.ShapeModule shape = trail.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 12f;
        shape.radius = 0.04f;
        shape.rotation = new Vector3(0f, 180f, 0f);

        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = trail.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(1f, 0.75f, 0.05f), 0f),
                new GradientColorKey(new Color(1f, 0.12f, 0.01f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0.9f, 0f),
                new GradientAlphaKey(0f, 1f)
            });
        colorOverLifetime.color = gradient;

        trail.Play();
        return trail;
    }

    void DestroyProjectile(GameObject projectile, ParticleSystem fireTrail)
    {
        if (fireTrail != null)
        {
            fireTrail.transform.SetParent(null, true);
            fireTrail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(fireTrail.gameObject, fireTrail.main.startLifetime.constantMax);
        }

        Destroy(projectile);
    }
}