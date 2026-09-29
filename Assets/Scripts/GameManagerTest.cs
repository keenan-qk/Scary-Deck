using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerTest : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
	    if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
	    {
	            GameManager.Instance.PauseGame();
        	    Debug.Log("Game State: " + GameManager.Instance.CurrentState);
	    }
	    else if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
	    {
		    GameManager.Instance.ResumeGame(); 
        	    Debug.Log("Game State: " + GameManager.Instance.CurrentState);
	    }
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            GameManager.Instance.GameOver();
            Debug.Log("Game State: " + GameManager.Instance.CurrentState);
        }

	if (Keyboard.current.tKey.wasPressedThisFrame)
	{
	    Debug.Log("Restarting Level...");  
	    GameManager.Instance.RestartLevel(); 
	}
    }
}
