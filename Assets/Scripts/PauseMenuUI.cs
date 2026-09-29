using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; 

    void Start()
    {
    	pauseMenu.SetActive(false);     
    }

    void Update()
    {
       if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
       {
	       pauseMenu.SetActive(true);
       }
       else
       {
	       pauseMenu.SetActive(false);
       }
    }

    public void ResumeGame()
    {
	    GameManager.Instance.ResumeGame(); 
    }
}
