using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject levelCompleteUI;

    void Awake()
    {
        Instance = this;
    }

    public void LevelComplete()
    {
        levelCompleteUI.SetActive(true);  
        PlayerController player = FindObjectOfType<PlayerController>();
        if(player != null){
            player.SetInputEnabled(false);
        }
    }
    public void RestartLevel()
{
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}