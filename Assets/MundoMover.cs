using UnityEngine;

public class MundoMover : MonoBehaviour
{
    public float velocidade = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.left * velocidade * Time.deltaTime;
        }
    }
}
