using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MainMenu
{
    [SerializeField] GameObject gameMenuGameObject;

    private void Start()
    {
        gameMenuGameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if(InputManager.Instance.IsESCButtonDownThisFrame())
        {
            if(gameMenuGameObject.activeSelf)
            {
                gameMenuGameObject.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                gameMenuGameObject.SetActive(true);
                Time.timeScale = 0;                
            }
            
        }
    }

    public void ReloadLevel()
    {
        InputManager.Instance.DisablePlayerInputActions();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
