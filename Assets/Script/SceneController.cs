using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform finishPos;
    [SerializeField] PlayerController player;
    [SerializeField] Map currentMap;
    byte currentScene = 0;

    // Audio Section.
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip gameOverSound;
    MenuController menuController;
    AudioSource audioSource;

    public GameState gameState;
    public enum GameState { Play, Finish, Pause, GameOver };
    #region Singleton
    public static SceneController instance;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    #endregion
    #region Properties
    public PlayerController Player
    {
        get { return player; }
    }

    public byte CurrentScene
    {
        get { return currentScene; }
    }

    public Map CurrentMap
    {
        get { return currentMap; }
        set { currentMap = value; }
    }
    #endregion
    private void Start()
    {
        menuController = MenuController.Instance;
    }
    // Update is called once per frame
    private void Update()
    {
        switch(gameState)
        {
            case GameState.Play:
                currentScene = (byte)SceneManager.GetActiveScene().buildIndex;
                if (startPos == null && GameObject.FindGameObjectWithTag("Respawn"))
                {
                    startPos = GameObject.FindGameObjectWithTag("Respawn").transform;
                }
                if(finishPos == null && GameObject.FindGameObjectWithTag("Finish"))
                {
                    finishPos = GameObject.FindGameObjectWithTag("Finish").transform;
                }
                if (player == null && GameObject.FindGameObjectWithTag("Player"))
                {
                    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                }
                break;
            case GameState.Finish:
                // get currentScene.
                DontDestroyOnLoad(gameObject);
                audioSource.PlayOneShot(finishSound);

                // check if currentScene is out of range.
                if(currentScene++ < SceneManager.sceneCountInBuildSettings - 1) // becuz the last scene is MainMenu so it must -1
                    SceneManager.LoadScene(currentScene++);
                // check if this scene finish than unlock the next scene for menu controller.
                UnlockMap();
                gameState = GameState.Play;
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

    void UnlockMap()
    {
        byte currentScene;
        byte nextScene;

        // get current active scene.
        currentScene = (byte)SceneManager.GetActiveScene().buildIndex;
        nextScene = (byte)(currentScene + 1);

        // check if the map is complete
        if(menuController.Maps[currentScene] && nextScene < SceneManager.sceneCountInBuildSettings)
        {
            //unlock the next map.
            menuController.SetBool(menuController.Maps[nextScene].name, true);
        }
        print("Unlocking Map");
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
