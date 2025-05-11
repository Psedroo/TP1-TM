using UnityEngine;
using UnityEngine.SceneManagement;

public class logica : MonoBehaviour
{
    // Método para mudar para a cena do jogo
    public void MudarCena()
    {
        Debug.Log("Clicouuu");
        SceneManager.LoadScene("Jogo");
    }

    // Método para sair do jogo
    public void SairDoJogo()
    {
        Debug.Log("Sair do jogo...");
        Application.Quit();
    }

}
