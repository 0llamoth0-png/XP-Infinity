using UnityEngine;
using UnityEngine.SceneManagement;
public class SalidaJuego : MonoBehaviour
{
 public void SalirJuego ()
    {
        Application.Quit();
    }

    public void IniciarJuego ()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
