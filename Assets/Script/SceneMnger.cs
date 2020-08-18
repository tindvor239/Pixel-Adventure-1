using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMnger : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform finishPos;
    [SerializeField] GameObject player;
    [SerializeField] int sceneCount = 0;
    public GameState gameState;
    public enum GameState {Play, Finish, Pause };
    #region Singleton
    public static SceneMnger instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject Player
    {
        get { return player; }
    }

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
                sceneCount++;
                SceneMnger.DontDestroyOnLoad(this.gameObject);
                gameState = GameState.Play;
                SceneManager.LoadScene(sceneCount);
                break;
        }
    }
}
