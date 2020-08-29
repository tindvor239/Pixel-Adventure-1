using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject chooseMaps;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject[] mapObjects = new GameObject[3];
    [SerializeField] Map[] maps = new Map[3];
    [SerializeField] Image[] map1Star = new Image[3];
    [SerializeField] Image[] map2Star = new Image[3];
    [SerializeField] Image[] map3Star = new Image[3];
    bool isGameOver = false; // must have this boolean because don't have this gameoverUI will be alway active when player dead.

    SceneController sceneController;
    #region singleton
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
        sceneController = SceneController.instance;
        sceneController.gameState = SceneController.GameState.Pause;
        SetBool(maps[0].name, true);
    }

    private void Update()
    {
        // Get and set scene.
        GetMapInfo(0);
        GetMapInfo(1);
        GetMapInfo(2);
        GetMapStar();

        mapObjects[0].SetActive(!maps[0].isMapComplete);
        mapObjects[1].SetActive(!maps[1].isMapComplete);
        mapObjects[2].SetActive(!maps[2].isMapComplete);
        
        switch(sceneController.gameState)
        {
            // on game over set ui game over visible.
            case SceneController.GameState.GameOver:
                if (isGameOver == false)
                    gameOverUI.SetActive(true);
                else (isGameOver)
                    gameOverUI.SetActive(false);
                break;
            default:
                gameOverUI.SetActive(false);
                break;
        }
    }
    public void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
    public void SetByte(string mapName, string valueName, byte value)
    {
        PlayerPrefs.SetInt(mapName + valueName, value);
    }
    bool GetBool(string mapName)
    {
        int value = 0;
        if (PlayerPrefs.HasKey(mapName))
        {
            value = PlayerPrefs.GetInt(mapName);
            if(value == 1)
            {
                return true;
            }
        }
        else
        {
            print("key you input: " + mapName + " don't have any!.");
            SetBool(mapName, false);
        }
        return false;
    }
    byte GetByte(string mapName, string valueName)
    {
        byte value = 0;
        if(PlayerPrefs.HasKey(mapName))
        {
            value = (byte)PlayerPrefs.GetInt(mapName + valueName);
        }
        return value;
    }

    public void LoadMainMenuScene()
    {
        isGameOver = false;
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReplayCurrentMap()
    {
        sceneController.LoadScene(sceneController.CurrentScene);
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
        if(GetBool(maps[0].name))
        {
            DontDestroyOnLoad(sceneController);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            sceneController.CurrentMap = maps[0];
            sceneController.LoadScene(0);
        }
    }
    public void LoadMap2()
    {
        if (GetBool(maps[1].name))
        {
            DontDestroyOnLoad(sceneController);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            sceneController.CurrentMap = maps[1];
            sceneController.LoadScene(1);
        }
    }
    public void LoadMap3()
    {
        if (GetBool(maps[2].name))
        {
            DontDestroyOnLoad(sceneController);
            mainMenu.SetActive(false);
            chooseMaps.SetActive(false);
            DontDestroyOnLoad(gameObject);
            sceneController.CurrentMap = maps[2];
            sceneController.LoadScene(2);
        }
    }

    void GetMapInfo(byte mapIndex)
    {
        maps[mapIndex].isMapComplete = GetBool(maps[mapIndex].name);
        maps[mapIndex].star = GetByte(maps[mapIndex].name, " star");
        maps[mapIndex].highScore = GetByte(maps[mapIndex].name, " high score");
        SetByte(maps[mapIndex].name, string.Format(" starPoint {0}", mapIndex), maps[mapIndex].starPoint[0]);
        SetByte(maps[mapIndex].name, string.Format(" starPoint {0}", mapIndex), maps[mapIndex].starPoint[1]);
        SetByte(maps[mapIndex].name, string.Format(" starPoint {0}", mapIndex), maps[mapIndex].starPoint[2]);
    }

    void GetMapStar()
    {
        if(maps[0].highScore >= maps[0].starPoint[0])
        {
            map1Star[0].color = Color.yellow;
            if (maps[0].highScore >= maps[0].starPoint[1])
            {
                map1Star[1].color = Color.yellow;
                if (maps[0].highScore >= maps[0].starPoint[2])
                    map1Star[2].color = Color.yellow;
                else
                    map1Star[2].color = Color.white;
            }
            else
                map1Star[1].color = Color.white;
        }
        else
            map1Star[0].color = Color.white;

        if (maps[1].highScore >= maps[1].starPoint[0])
        {
            map2Star[0].color = Color.yellow;
            if (maps[1].highScore >= maps[1].starPoint[1])
            {
                map2Star[1].color = Color.yellow;
                if (maps[1].highScore >= maps[1].starPoint[2])
                    map2Star[2].color = Color.yellow;
                else
                    map2Star[2].color = Color.white;
            }
            else
                map2Star[1].color = Color.white;
        }
        else
            map2Star[0].color = Color.white;

        if (maps[2].highScore >= maps[2].starPoint[0])
        {
            map3Star[0].color = Color.yellow;
            if (maps[2].highScore >= maps[2].starPoint[1])
            {
                map3Star[1].color = Color.yellow;
                if (maps[2].highScore >= maps[2].starPoint[2])
                    map3Star[2].color = Color.yellow;
                else
                    map3Star[2].color = Color.white;
            }
            else
                map3Star[1].color = Color.white;
        }
        else
            map3Star[0].color = Color.white;/*
        print(string.Format("Map 1: {0}", maps[0]));
        print(string.Format("high score: {0}\n --------------------------------------------------", maps[0].highScore));
        print(string.Format("Map 2: {0}", maps[1]));
        print(string.Format("high score: {0}\n --------------------------------------------------", maps[1].highScore));
        print(string.Format("Map 3: {0}", maps[2]));
        print(string.Format("high score: {0}\n --------------------------------------------------", maps[2].highScore));*/
    }
}
