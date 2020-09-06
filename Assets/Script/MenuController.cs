using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region UI Objects
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject chooseMaps;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject mapInfoUI;
    #endregion

    #region Map Objects.
    static sbyte limitMapRange = 3;
    // Scriptable Object Maps.
    [SerializeField] Map[] maps = new Map[limitMapRange];
    // Map UI.
    /**** Stars Images ****/
    [SerializeField] Image[] map1Star = new Image[limitMapRange];
    [SerializeField] Image[] map2Star = new Image[limitMapRange];
    [SerializeField] Image[] map3Star = new Image[limitMapRange];
    /**** Lock ****/
    [SerializeField] GameObject[] mapLocks = new GameObject[limitMapRange];

    List<Image[]> listImages = new List<Image[]>();
    MapUI[] mapUIs = new MapUI[limitMapRange];

    // Map Info.
    [SerializeField] Image[] mapInfoStars = new Image[limitMapRange];
    [SerializeField] GameObject newScore;
    [SerializeField] Text highScore;
    [SerializeField] Text score;
    [SerializeField] Text scoreMore;
    [SerializeField] GameObject mainMenuButton;
    [SerializeField] GameObject nextMapButton;

    float time = 0;

    MapInfo mapInfo = new MapInfo();
    #endregion

    #region Boss Map UI
    [SerializeField] Text timer;
    #endregion

    //
    [SerializeField] bool isGameOver = false; // must have this boolean because don't have this gameoverUI will be alway active when player dead.
    SceneController sceneController;

    #region Singleton
    public static MenuController Instance;
    void Awake()
    {
        Instance = this;
    }
    #endregion
    #region Properties
    public Map[] Maps
    {
        get { return maps; }
    }
    #endregion
    // Start is called before the first frame update
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        // Map Declaration.
        listImages.Add(map1Star);
        listImages.Add(map2Star);
        listImages.Add(map3Star);

        PlayerPrefs.SetInt(string.Format("is{0}Complete", Maps[0].name), 1); // for map1 alway unlock.
        
        for (int index = 0; index < mapUIs.Length; index++)
        {
            mapUIs[index] = new MapUI(mapLocks[index], maps[index]);
        }

        mapInfo = new MapInfo(mapInfoStars, newScore, highScore, score, scoreMore);
        // Scence Controller Declaration.
        sceneController = SceneController.instance;
        sceneController.gameState = SceneController.GameState.Pause;

    }

    private void Update()
    {
        timer.gameObject.SetActive(false);
        switch (sceneController.gameState)
        {
            case SceneController.GameState.Play:
                switch (maps[sceneController.CurrentScene].type)
                {
                    case Map.Type.Boss:
                        time += Time.deltaTime;
                        timer.gameObject.SetActive(true);
                        timer.text = time.ToString("0.0");
                        break;
                    default:
                        time = 0;
                        break;
                }
                isGameOver = false;
                mapInfoUI.SetActive(false);
                gameOverUI.SetActive(false);
                break;
            // on game over set ui game over visible.
            case SceneController.GameState.GameOver:
                if (isGameOver == false)
                    gameOverUI.SetActive(true);
                else
                    gameOverUI.SetActive(false);
                mapInfoUI.SetActive(false);
                break;
            case SceneController.GameState.Finish:
                uint score;
                int timerScore = 0;

                mapInfoUI.SetActive(true);
                // next scene is NOT out of range.
                if (sceneController.CurrentScene + 2 < sceneController.SceneCount) // NOT COUNT main menu scene.
                {
                    nextMapButton.SetActive(true);
                    mainMenuButton.SetActive(false);

                    // Unlock next map.
                    if (maps[sceneController.CurrentScene + 1] != null && maps[sceneController.CurrentScene + 1].IsMapComplete == false)
                    {
                        if (maps[sceneController.CurrentScene].IsMapComplete)
                        {
                            maps[sceneController.CurrentScene + 1].IsMapComplete = true;
                        }
                    }
                }
                else
                {
                    nextMapButton.SetActive(false);
                    mainMenuButton.SetActive(true);
                }

                // if in boss map.
                if (maps[sceneController.CurrentScene].type == Map.Type.Boss)
                {
                    timerScore = (int)(maps[sceneController.CurrentScene].time - time);

                }
                score = (uint)(timerScore + sceneController.Player.Score + sceneController.Player.Stats.HP);
                mapInfo.SetMapInfo(maps[sceneController.CurrentScene], score);
                break;
            default:
                isGameOver = false;
                mapInfoUI.SetActive(false);
                gameOverUI.SetActive(false);
                break;
        }
    }

    #region Button Behaviors
    public void LoadMainMenuScene()
    {
        isGameOver = true;
        sceneController.gameState = SceneController.GameState.Pause;
        mainMenu.SetActive(true);
        mapInfoUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReplayCurrentMap()
    {
        sceneController.LoadScene(sceneController.CurrentScene + 1);
    }

    public void PlayNextMap()
    {
        sceneController.LoadScene(sceneController.CurrentScene + 2);
    }

    public void CloseUI(GameObject closeUI)
    {
        closeUI.SetActive(false);
    }
    public void OpenUI(GameObject openUI)
    {
        openUI.SetActive(true);
    }

    public void OpenUIChooseMap()
    {
        for (int index = 0; index < maps.Length; index++)
        {
            maps[index] = new Map(listImages[index], maps[index].starPoint, maps[index].name, maps[index].type, maps[index].time);
        }
        mainMenu.SetActive(false);
        chooseMaps.SetActive(true);
    }
    public void CloseUIChooseMap()
    {
        mainMenu.SetActive(true);
        chooseMaps.SetActive(false);
    }
    public void LoadMap(int mapIndex)
    {
        if(maps[mapIndex - 1].IsMapComplete)
        {
            DontDestroyOnLoad(sceneController);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            sceneController.LoadScene(mapIndex);
        }
    }
    #endregion
}

struct MapUI
{
    private Map map;
    private GameObject lockImage;
    public MapUI(GameObject lockImage, Map map)
    {
        this.lockImage = lockImage;
        this.map = map;
        lockImage.SetActive(!map.IsMapComplete);
    }
}
struct MapInfo
{
    public Image[] starImages;
    public GameObject newHighScore;
    public Text highScore;
    public Text score;
    public Text scoreMore;

    public MapInfo(Image[] starImages, GameObject newHighScore, Text highScore, Text score, Text scoreMore)
    {
        this.starImages = starImages;
        this.newHighScore = newHighScore;
        this.highScore = highScore;
        this.score = score;
        this.scoreMore = scoreMore;
    }
    public void SetMapInfo(Map currentMap, uint score)
    {
        string scoreMoreString;
        
        if(score >= currentMap.HighScore)
        {
            PlayerPrefs.SetInt(string.Format("{0} highScore", currentMap.name), (int)score);
            newHighScore.SetActive(true);
            scoreMore.color = Color.yellow;
            scoreMoreString = "Congrats! you have beat the last highScore.";
        }
        else
        {
            newHighScore.SetActive(false);
            scoreMore.color = Color.red;
            scoreMoreString = string.Format("{0} score more. C'mon, you can do it!.", (currentMap.HighScore - score).ToString());
        }
        highScore.text = string.Format("High Score: {0}", currentMap.HighScore.ToString());
        this.score.text = string.Format("Your Score: {0}", score.ToString());
        scoreMore.text = scoreMoreString;
        currentMap.SetStar(starImages, score);
    }
}