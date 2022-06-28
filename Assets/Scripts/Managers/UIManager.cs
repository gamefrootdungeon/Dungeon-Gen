using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public static bool isInMenu = true;
    public static bool isInGame = false;
    public GridManager gridManager;
    public JsonConvertManager conversionManager;
    public CanvasSampleOpenFileTextJson fileBrowser;


    public bool isTopDown = false;

    [SerializeField]private GameObject MainMenu;
    [SerializeField]private GameObject PauseMenu;
    [SerializeField]private GameObject playBtn;
    [SerializeField] private GameObject userIcon;

    public GameObject HudUIObj;

    public GameObject loadingText;
    public GameObject playLevelBtnImageCover;


    public string jsonLink;
    private string jsonData;

    public GameObject levelInfo;
    public TextMeshProUGUI titleInfoText;
    public TextMeshProUGUI InfoText;
    public TextMeshProUGUI userIDText;

    public string userID;
    public TextAsset DefaultJson;
    public void SpawnPlayer()
    {
        gridManager.SpawnPlayer();
        if (gridManager.playerSpawned)
        {
            isInGame = true;
            isInMenu = false;
            if (!isTopDown)
            {
                HideMouse();
                HudUIObj.SetActive(true);
            }
            MainMenu.SetActive(false);
            PauseMenu.SetActive(false);
            ResumeGame();
        }
    }
    private void Start()
    {
        if(StoreUserID.instance.isLoggedin == true)
        {
            userIcon.SetActive(true);
        }
        else if (StoreUserID.instance.isLoggedin == false)
        {
            userIcon.SetActive(false);
        }
        print("Storing values");
        print(StoreUserID.instance.userID);
        userID = StoreUserID.instance.userID;

        print("User ID " + userID);
        userIDText.text = userID;
    }
    private void Update()
    {
        CheckForEscapeButtonPressed();
    }

    private void CheckForEscapeButtonPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInGame)
            {
                if (isInMenu)
                {
                    ResumeGame();
                }
                else
                {
                    ToPauseMenu();
                }
            }
        }
    }
    public string jsonView;
    public void SetUpLevel(string json)
    {
        jsonView = json;
        try
        {
            gridManager.LoadJsonFile(json);
            SetUpLevelInfo();
            LevelFinshedLoading();
        }
        catch
        {
            print("Error couldn't load level!");
        }
    }

    private void SetUpLevelInfo()
    {
        titleInfoText.text = gridManager.title;
        InfoText.text = gridManager.story;
    }


    #region Load Default JSON
    public void LoadDefaultJson()
    {
        gridManager.DestroyDungeon();
        string json = DefaultJson.text;
        string name = DefaultJson.name;
        string newJson = conversionManager.ManualConversion(json);
        LoadFileFromBrowser(newJson);
    }

    #endregion

    #region Load JSON from the web
    public void LoadFromWeb()
    {
        LoadingLevel();
        StartCoroutine(Check());
    }
    private IEnumerator Check()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(jsonLink))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ProtocolError
                || request.result == UnityWebRequest.Result.ConnectionError)
            {
                print(request.error);
            }
            else
            {
                print("Successfully downloaded text!");
                print(request.downloadHandler.text);
                jsonData = request.downloadHandler.text;
                string newJsonData = conversionManager.ManualConversion(jsonData);
                SetUpLevel(newJsonData);
            }
        }
    }

    #endregion

    #region Load JSON from file browser

    public void SetJson(string json)
    {
        string newJson = conversionManager.ManualConversion(json);
        LoadFileFromBrowser(newJson);
    }
    public void LoadFileFromBrowser(string loadedJson)
    {
        LoadingLevel();
        SetUpLevel(loadedJson);
    }

    #endregion


    #region Set up level info UI
    public void ShowLevelInfo(string info1)
    {
        InfoText.text = info1;
    }
    public void ShowLevelInfo(string info1, string info2)
    {
        titleInfoText.text = info1;
        InfoText.text = info2;
        levelInfo.SetActive(true);
    }
    public void HideLevelInfo()
    {
        titleInfoText.text = "";
        InfoText.text = "";
        levelInfo.SetActive(false);
    }

    #endregion


    #region Mouse control
    public static void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion

    #region Menus

    public void LoadingLevel()
    {
        loadingText.SetActive(true);
    }
    public void LevelFinshedLoading()
    {
        loadingText.SetActive(false);
        playBtn.SetActive(true);
        playLevelBtnImageCover.SetActive(false);
    }


    public void ToMainMenu()
    {
        Time.timeScale = 1;
        gridManager.DestroyDungeon();
        playBtn.SetActive(false);
        playLevelBtnImageCover.SetActive(true);
        isInMenu = true;
        MainMenu.SetActive(true);
        PauseMenu.SetActive(false);
        ShowMouse();
        HudUIObj.SetActive(false);
        gridManager.BackToMenu();
    }

    public void ToPauseMenu()
    {
        Time.timeScale = 0;
        isInMenu = true;
        MainMenu.SetActive(false);
        PauseMenu.SetActive(true);
        ShowMouse();
        HudUIObj.SetActive(false);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        isInMenu = false;
        MainMenu.SetActive(false);
        PauseMenu.SetActive(false);
        if (!isTopDown)
        {
            HideMouse();
            HudUIObj.SetActive(true);
        }
    }

    #endregion



}



