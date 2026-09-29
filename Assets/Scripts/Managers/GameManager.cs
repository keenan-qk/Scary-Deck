using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; } 

    public enum GameState
    {
	    Playing,
	    Paused,
	    GameOver
    }
    
    public GameState CurrentState {get; private set; } = GameState.Playing;

    private void Awake()
    {
	    if (Instance != null && Instance != this)
	    {
		    Destroy(gameObject); 
		    return;
	    }

	    Instance = this; 
    }

    public void PauseGame()
    {
	    CurrentState = GameState.Paused; 
	    Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
	    CurrentState = GameState.Playing; 
	    Time.timeScale = 1f; 
    }

    public void GameOver()
    {
	    CurrentState = GameState.GameOver; 
	    Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
	    Time.timeScale = 1f; 
	    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
