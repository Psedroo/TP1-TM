using UnityEngine;
using System.Collections;

public class MotaController : MonoBehaviour
{
    
    public float impulsoMagnitude = 11f;   // força do salto na rampa
    public float anguloMaximo = 7f;      // graus tolerados na chegada
    public float duracaoLerp = 0.20f;   
    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    /* ---------- SALTO NA RAMPA ---------- */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ramp")) return;

        Vector2 impulso = transform.right * impulsoMagnitude;   // respeita a inclinação da rampa
        rb.AddForce(impulso, ForceMode2D.Impulse);

    }

    /* -------- DETECÇÃO DE CHÃO ---------- */
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Chao")) return;


        float angActual = Mathf.DeltaAngle(0f, rb.rotation);

        // se chegar torta acima do limite, inicia correcção suave
        if (Mathf.Abs(angActual) > anguloMaximo)
            StartCoroutine(EndireitarSuavemente());
    }


    IEnumerator EndireitarSuavemente()
    {
        float t = 0f;
        float rotInicial = rb.rotation;

        // trava a rotação para não “lutar” com a física
        rb.angularVelocity = 0f;

        while (t < duracaoLerp)
        {
            t += Time.deltaTime;
            float novaRot = Mathf.LerpAngle(rotInicial, 0f, t / duracaoLerp);
            rb.MoveRotation(novaRot);
            yield return null;
        }

        rb.MoveRotation(0f);       // garante 0º exactos
    }
}
