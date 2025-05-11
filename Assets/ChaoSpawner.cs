using UnityEngine;
using System.Collections.Generic;

public class ChaoSpawner : MonoBehaviour
{
    public GameObject blocoChao;
    public GameObject[] obstaculosDisponiveis;
    public GameObject motaPrefab;   

    public float larguraBloco = 4f;
    public float alturaObstaculo = 1f;

    public Transform paiDoChao;
    public Camera cameraPrincipal;

    GameObject ultimoBloco;
    readonly List<GameObject> blocosInstanciados = new();
    int totalBlocosInstanciados = 1;

    void Start()
    {
        /* ------- cria o primeiro bloco de chão ------- */
        Vector3 posicaoInicial = new(-12f, -4f, 0f);
        ultimoBloco = Instantiate(blocoChao, posicaoInicial, Quaternion.identity, paiDoChao);
        blocosInstanciados.Add(ultimoBloco);

        /* ------- instancia a mota -------- */
        if (motaPrefab != null)
        {
            Vector3 posMota = posicaoInicial + new Vector3(5f, 1f, 0f);
            Instantiate(motaPrefab, posMota, Quaternion.identity);
        }
    }

    void Update()
    {
        if (ultimoBloco == null) return;

        float limiteDireito = cameraPrincipal.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
        float fimDoBloco = ultimoBloco.transform.position.x + larguraBloco;

        if (fimDoBloco < limiteDireito + 3f)
        {
            GameObject novoBloco = Instantiate(blocoChao, Vector3.zero, Quaternion.identity, paiDoChao);

            float y = novoBloco.CompareTag("Tile") ? 1f :
                      novoBloco.CompareTag("Tile_alto") ? 2f : -4f;

            Vector3 novaPos = new(fimDoBloco, y, 0f);
            novoBloco.transform.position = novaPos;

            ultimoBloco = novoBloco;
            blocosInstanciados.Add(novoBloco);
            totalBlocosInstanciados++;

            if (totalBlocosInstanciados % 10 == 0 && obstaculosDisponiveis.Length > 0)
            {
                int idx = Random.Range(0, obstaculosDisponiveis.Length);
                GameObject ob = Instantiate(obstaculosDisponiveis[idx]);

                float offsetX = Random.Range(0.5f, larguraBloco - 0.5f);
                float hY = ob.CompareTag("Tile") ? 3.4f :
                                ob.CompareTag("Tile_alto") ? 2.4f : 0.5f;

                ob.transform.position = novaPos + new Vector3(offsetX, hY, 0f);
                ob.transform.parent = paiDoChao;
                blocosInstanciados.Add(ob);
            }
        }

        /* ------- limpa blocos/obstáculos fora do ecrã ------- */
        float limiteEsquerdo = cameraPrincipal.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
        const float margemExtra = 10f;

        for (int i = blocosInstanciados.Count - 1; i >= 0; i--)
        {
            GameObject b = blocosInstanciados[i];
            float fimX = b.transform.position.x + larguraBloco;

            if (fimX < limiteEsquerdo - margemExtra)
            {
                Destroy(b);
                blocosInstanciados.RemoveAt(i);
            }
        }
    }
}
