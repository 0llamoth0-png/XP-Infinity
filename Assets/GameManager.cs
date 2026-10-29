using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isGamePaused;
    public GameObject pauseMenuUI, LoseUI;
    void Start()
    {
        isGamePaused=false;
    }
   public void PauseGame()
   {
       isGamePaused = !isGamePaused;
       if(isGamePaused)
       {
           Time.timeScale = 0f;
              pauseMenuUI.SetActive(true);
       }
       else
       {
           Time.timeScale = 1f;
              pauseMenuUI.SetActive(false);
       }
   } 
   public void LoseGame()
   {
       Time.timeScale = 0f;
       LoseUI.SetActive(true);
   }
   public void InitialMenu()
   {
       Time.timeScale = 1f;
       SceneManager.LoadScene("TitleGame");
   }

   public void LoadScene(string sceneName)
    {
        Time.timeScale=1;
        SceneManager.LoadScene(sceneName);
    }
}