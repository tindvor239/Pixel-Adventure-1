using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject chooseMaps;
    [SerializeField] SceneMnger sceneManager;
    [SerializeField] GameObject[] maps = new GameObject[3];
    [SerializeField] bool[] mapCompletes = new bool[3];
    [SerializeField] string[] mapInString = new string[3];
    // Start is called before the first frame update
    private void Start()
    {
        sceneManager = SceneMnger.instance;
        sceneManager.gameState = SceneMnger.GameState.Pause;
        SetBool("isMap1Complete", true);
    }

    private void Update()
    {
        // Get and set scene.
        mapCompletes[0] = GetBool(mapInString[0]);
        mapCompletes[1] = GetBool(mapInString[1]);
        mapCompletes[2] = GetBool(mapInString[2]);
        maps[0].SetActive(!mapCompletes[0]);
        maps[1].SetActive(!mapCompletes[1]);
        maps[2].SetActive(!mapCompletes[2]);

        // Check current scene is finish to unlock the next scene.
        if (sceneManager.gameState == SceneMnger.GameState.Finish)
        {
            print("I'm in game state finish");
            int currentScene = sceneManager.CurrentScene;
            mapCompletes[currentScene] = true;
            if(mapCompletes[currentScene] == true)
            {
                currentScene++;
                if(currentScene == 0)
                {
                    print("I'm in");
                    SetBool("isMap2Complete", true);
                }
                else if(currentScene == 1)
                {
                    SetBool("isMap3Complete", true);
                }
                
            }
            sceneManager.gameState = SceneMnger.GameState.Play;
        }
    }
    public void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
    bool GetBool(string key)
    {
        int value = 0;
        if (PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetInt(key);
            if(value == 1)
            {
                return true;
            }
        }
        else
        {
            print("key you input: " + key + " don't have any!.");
            SetBool(key, false);
        }
        return false;
    }
    public void OpenChooseMapMenu()
    {
        chooseMaps.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseChooseMapMenu()
    {
        chooseMaps.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadMap1()
    {
        if(GetBool(mapInString[0]))
        {
            DontDestroyOnLoad(sceneManager);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(0);
        }
    }
    public void LoadMap2()
    {
        if (GetBool(mapInString[1]))
        {
            DontDestroyOnLoad(sceneManager);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(1);
        }
    }
    public void LoadMap3()
    {
        if (GetBool(mapInString[2]))
        {
            DontDestroyOnLoad(sceneManager);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(2);
        }
    }
}
