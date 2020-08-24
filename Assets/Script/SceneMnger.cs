using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMnger : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform finishPos;
    [SerializeField] GameObject player;
    byte currentScene = 0;

    // Audio Section.
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip gameOverSound;
    AudioSource audioSource;

    public GameState gameState;
    public enum GameState { Play, Finish, Pause, GameOver };
    #region Singleton
    public static SceneMnger instance;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Properties
    public GameObject Player
    {
        get { return player; }
    }

    public byte CurrentScene
    {
        get { return currentScene; }
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.Play:
                if(startPos == null && GameObject.FindGameObjectWithTag("Respawn"))
                {
                    startPos = GameObject.FindGameObjectWithTag("Respawn").transform;
                }
                if(finishPos == null && GameObject.FindGameObjectWithTag("Finish"))
                {
                    finishPos = GameObject.FindGameObjectWithTag("Finish").transform;
                }
                if (player == null && GameObject.FindGameObjectWithTag("Player"))
                {
                    player = GameObject.FindGameObjectWithTag("Player");
                }
                break;
            case GameState.Finish:
                // get currentScene.
                currentScene = (byte)SceneManager.GetActiveScene().buildIndex;
                DontDestroyOnLoad(gameObject);
                audioSource.PlayOneShot(finishSound);

                // check if currentScene is out of range.
                if(currentScene++ < SceneManager.sceneCountInBuildSettings)
                    SceneManager.LoadScene(currentScene++);
                break;
            case GameState.GameOver:
                audioSource.PlayOneShot(gameOverSound);
                break;
            case GameState.Pause:
                break;
        }

        // if player don't exist in the scene => gameover.
        if(player == null && gameState == GameState.Play)
        {
            gameState = GameState.GameOver;
        }

        // if in menu (pause) state find game object with tag if NOT null switch state to play.
        // to make it not switch to play when load map.
        if(gameState == GameState.Pause && GameObject.FindGameObjectWithTag("Player") != null)
        {
            gameState = GameState.Play;
        }
    }
}
