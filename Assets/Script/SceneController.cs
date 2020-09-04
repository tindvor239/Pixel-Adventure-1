using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] MenuController menuController;
    [SerializeField] BossController boss;
    sbyte currentScene = 0;

    // Audio Section.
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip gameOverSound;
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

    public sbyte CurrentScene
    {
        get { return currentScene; }
    }

    public sbyte SceneCount
    {
        get { return (sbyte)SceneManager.sceneCountInBuildSettings; }
    }
    #endregion
    private void Start()
    {
        menuController = MenuController.Instance;
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level < SceneCount) // not count main menu
        {
            gameState = GameState.Play;
            currentScene = (sbyte)SceneManager.GetActiveScene().buildIndex;
            print(currentScene);
            if(menuController.Maps[currentScene].type == Map.Type.Boss)
            {
                boss = GameObject.FindGameObjectWithTag("Enemy").GetComponent<BossController>();
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (player == null && GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        switch (gameState)
        {
            case GameState.Play:
                if(player.Stats.HP == 0)
                {
                    gameState = GameState.GameOver;
                }

                if(menuController.Maps[currentScene].type == Map.Type.Boss && boss.Stats.HP == 0)
                {
                    gameState = GameState.Finish;
                }
                break;
            case GameState.Finish:
                audioSource.PlayOneShot(finishSound);
                break;
            case GameState.GameOver:
                audioSource.PlayOneShot(gameOverSound);
                break;
        }
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
