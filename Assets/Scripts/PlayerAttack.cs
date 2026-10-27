using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float alcance = 5.5f;
    [SerializeField] float anchoGolpe = 5.5f;
    [SerializeField] float altoGolpe = 2.5f;
    [SerializeField] float duracionGolpe = 0.25f;

    PlayerInputs inputs;
    Transform espada;
    Vector3 direccionMirada;
    bool atacando;

    void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
        if (inputs == null)
        {
            Debug.LogError("PlayerAttack necesita PlayerInputs en el mismo GameObject.", this);
            enabled = false;
            return;
        }

        direccionMirada = transform.forward;
        CrearEspada();
    }

    void Update()
    {
        Vector2 direccionMovimiento = inputs.Direction;
        if (direccionMovimiento.sqrMagnitude > 0.01f)
        {
            direccionMirada = new Vector3(direccionMovimiento.x, 0f, direccionMovimiento.y).normalized;
        }

        if (inputs.Attack.WasPressedThisFrame() && !atacando)
        {
            Golpear();
        }
    }

    void Golpear()
    {
        atacando = true;
        AplicarDanio();
        StartCoroutine(AnimarGolpe());
    }

    void AplicarDanio()
    {
        Quaternion orientacion = Quaternion.LookRotation(direccionMirada, Vector3.up);
        Vector3 centro = transform.position + Vector3.up * 0.25f + direccionMirada * (alcance * 0.5f);
        Vector3 semiextensiones = new Vector3(anchoGolpe * 0.5f, altoGolpe * 0.5f, alcance * 0.5f);
        Collider[] impactos = Physics.OverlapBox(centro, semiextensiones, orientacion);
        HashSet<EnemyHP> enemigosGolpeados = new HashSet<EnemyHP>();

        foreach (Collider impacto in impactos)
        {
            EnemyHP enemigo = impacto.GetComponentInParent<EnemyHP>();
            if (enemigo != null && !enemigo.transform.IsChildOf(transform) && enemigosGolpeados.Add(enemigo))
            {
                enemigo.TakeDamage(1);
            }
        }
    }

    IEnumerator AnimarGolpe()
    {
        espada.gameObject.SetActive(true);
        Vector3 direccionAtaque = direccionMirada;
        Quaternion orientacionBase = Quaternion.LookRotation(direccionAtaque, Vector3.up);
        float tiempo = 0f;

        while (tiempo < duracionGolpe)
        {
            float progreso = tiempo / duracionGolpe;
            espada.position = transform.position + direccionAtaque * 2.5f;
            espada.rotation = orientacionBase * Quaternion.Euler(0f, Mathf.Lerp(-30f, 30f, progreso), 0f);
            tiempo += Time.deltaTime;
            yield return null;
        }

        espada.position = transform.position + direccionAtaque * 2.5f;
        espada.rotation = orientacionBase * Quaternion.Euler(0f, 30f, 0f);
        espada.gameObject.SetActive(false);
        atacando = false;
    }

    void CrearEspada()
    {
        espada = new GameObject("Espada").transform;
        espada.SetParent(transform, false);
        Vector3 escalaInversa = new Vector3(
            1f / Mathf.Max(Mathf.Abs(transform.lossyScale.x), 0.0001f),
            1f / Mathf.Max(Mathf.Abs(transform.lossyScale.y), 0.0001f),
            1f / Mathf.Max(Mathf.Abs(transform.lossyScale.z), 0.0001f));
        CrearPiezaEspada("Hoja", new Vector3(0.12f, 0.08f, 2.4f), new Vector3(0f, 0f, 1.65f),
            new Color(0.75f, 0.85f, 0.9f), escalaInversa);
        CrearPiezaEspada("Guarda", new Vector3(0.45f, 0.1f, 0.12f), new Vector3(0f, 0f, 0.38f),
            new Color(0.85f, 0.65f, 0.2f), escalaInversa);
        CrearPiezaEspada("Empuñadura", new Vector3(0.14f, 0.14f, 0.55f), new Vector3(0f, 0f, 0.05f),
            new Color(0.3f, 0.16f, 0.08f), escalaInversa);

        espada.gameObject.SetActive(false);
    }

    void CrearPiezaEspada(string nombre, Vector3 tamano, Vector3 posicion, Color color, Vector3 escalaInversa)
    {
        GameObject pieza = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pieza.name = nombre;
        pieza.transform.SetParent(espada, false);
        pieza.transform.localPosition = Vector3.Scale(posicion, escalaInversa);
        pieza.transform.localScale = Vector3.Scale(tamano, escalaInversa);
        pieza.GetComponent<Collider>().enabled = false;
        pieza.GetComponent<Renderer>().material.color = color;
    }
}
