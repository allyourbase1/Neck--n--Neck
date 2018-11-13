using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] int characterSelectScene;
    [SerializeField] int fightScene;
    [SerializeField] int victoryScene;
    int activeScene;
    [Header("Fight Management")]
    [SerializeField] int totalRounds = 3;
    int roundsToWin = 0;
    int pOneWins = 0;
    int pTwoWins = 0;
    public GameObject[] characterTwoPrefabs;
    public GameObject[] characterOnePrefabs;
    public GameObject playerTwoCharacter;
    public GameObject playerOneCharacter;
    [Header("Game Canvas")]
    [SerializeField] CanvasGroup mainMenuGUI;
    [SerializeField] CanvasGroup characterSelectGUI;
    [SerializeField] CanvasGroup fightGUI;
    [SerializeField] CanvasGroup victoryGUI;
    [SerializeField] Image countdownText;
    [SerializeField] Sprite countThree; //CK
    [SerializeField] Sprite countTwo;   //CK
    [SerializeField] Sprite countOne;   //CK
    [SerializeField] Sprite countFight; //CK
    [SerializeField] CanvasGroup fadeOutImage;
    [SerializeField] float screenFadeSpeed = 3;
    [SerializeField] Text playerWinsText;
    float fadeStep = 0;
    [Header("Game Audio")]
    [SerializeField] AudioClip bellRoundStart;
    [SerializeField] AudioClip bellRoundEnd;
    [SerializeField] AudioClip bellMatch;
    [SerializeField] AudioSource audioSource;
    MusicController musicController;
    [Header("Debug Config")]
    [SerializeField] bool testing = false;
    [SerializeField] float countdownTimer = 1f;
    [SerializeField] GameObject musicControllerPrefab;
    Image pOneTokenFill; //CK
    Image pTwoTokenFill; //CK
    PlayerSpawner playerSpawner;

    [SerializeField] GameObject fightersCamera;
    [SerializeField] GameObject fightersImage;
    public bool isCharacterSelect = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainMenuGUI = GameObject.Find("MainMenuGUI").GetComponent<CanvasGroup>();
        characterSelectGUI = GameObject.Find("CharacterSelectGUI").GetComponent<CanvasGroup>();
        fightGUI = GameObject.Find("FightGUI").GetComponent<CanvasGroup>();
        victoryGUI = GameObject.Find("VictoryGUI").GetComponent<CanvasGroup>();
        SceneChangeGUIToggle(true, false, false, false);

        pOneTokenFill = GameObject.Find("PlayerOneTokenFill").GetComponent<Image>(); //CK
        pTwoTokenFill = GameObject.Find("PlayerTwoTokenFill").GetComponent<Image>(); //CK

        fadeOutImage = GameObject.Find("FadeOutImage").GetComponent<CanvasGroup>();
        fadeOutImage.alpha = 0;
        fadeOutImage.interactable = false;
        fadeOutImage.blocksRaycasts = false;

        //playerWinsText = GameObject.Find("Winner").GetComponent<Text>();
        countdownText.sprite = null;
        countdownText.enabled = false;

        // Determine if odd number of rounds. If not, add one.
        if (totalRounds % 2 == 0)
        {
            totalRounds++;
        }

        roundsToWin = (totalRounds / 2) + 1;

        fadeStep = screenFadeSpeed * Time.deltaTime;

        if (testing)
        {
            GameObject music = Instantiate(musicControllerPrefab);
            music.name = "MusicController";
        }

        // Get the music controller and set the music fade time equal to the screen fade time
        musicController = GameObject.Find("MusicController").GetComponent<MusicController>();
        musicController.SetFade(fadeStep);
        musicController.PlayTitleMusic();

        if (testing)
        {
            activeScene = fightScene;
            countdownTimer = .01f;
            StartCoroutine(SceneFadeOut());
        }
    }

    public void PlayGame(bool gotoPlayerSelect)
    {
        pOneWins = 0;
        pOneTokenFill.fillAmount = 0f;
        pTwoWins = 0;
        pTwoTokenFill.fillAmount = 0f;

        if (gotoPlayerSelect)
        {
            activeScene = characterSelectScene;
            characterSelectGUI.GetComponentInChildren<CharacterSelect>().ResetSelection();
            StartCoroutine(SceneFadeOut());
        }
        else
        {
            activeScene = fightScene;
            StartCoroutine(SceneFadeOut());
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ToggleGUI(CanvasGroup cGroup, bool show)
    {
        if (cGroup == fightGUI && show)
        {
            cGroup.alpha = show ? 1 : 0;
        }
        else
        {
            if (show)
            {
                cGroup.alpha = 1;
                cGroup.interactable = true;
                cGroup.blocksRaycasts = true;
                if(cGroup == characterSelectGUI)
                {
                    Debug.Log("Fighter select");
                    fightersCamera.SetActive(true);
                    fightersImage.SetActive(true);
                    isCharacterSelect = true;
                }
                else
                {
                    Debug.Log("Not fighter select");
                    fightersCamera.SetActive(false);
                    fightersImage.SetActive(false);
                    isCharacterSelect = false;
                }
            }
            else
            {
                cGroup.alpha = 0;
                cGroup.interactable = false;
                cGroup.blocksRaycasts = false;
            }
        }
    }

    // This function is called by the defeated player, sending their number to this script to determine who gets the point.
    public void AddWin(string playerTag)
    {
        Debug.Log("Howdy");

        switch (playerTag)
        {
            case "Player1":
                pTwoWins++;
                pTwoTokenFill.fillAmount = (float)pTwoWins / (float)roundsToWin;
                break;
            case "Player2":
                pOneWins++;
                pOneTokenFill.fillAmount = (float)pOneWins / (float)roundsToWin;
                break;
        }

        if (pOneWins >= roundsToWin || pTwoWins >= roundsToWin)
        {
            audioSource.clip = bellMatch;
            playerWinsText.text = pOneWins == roundsToWin ? "Player 1 Wins!" : "Player 2 Wins!";
            activeScene = victoryScene;
        }
        else
        {
            audioSource.clip = bellRoundEnd;
            activeScene = fightScene;
        }

        audioSource.Play();

        StartCoroutine(SceneFadeOut());
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene(activeScene);

        StopAllCoroutines();

        if (activeScene == victoryScene)
        {
            SceneChangeGUIToggle(false, false, false, true);
            musicController.PlayVictoryMusic();
        }
        else if (activeScene == fightScene)
        {
            SceneChangeGUIToggle(false, false, true, false);
            musicController.PlayCombatMusic();
        }
        else if (activeScene == characterSelectScene)
        {
            SceneChangeGUIToggle(false, true, false, false);
            audioSource.clip = null;
            musicController.PlayCharacterSelectMusic();
        }

        StartCoroutine(SceneFadeIn());
    }

    private void FindAndEnablePlayers()
    {
        if (GameObject.Find("PlayerOne") != null)
        {
            GameObject.Find("PlayerOne").GetComponent<PlayerControl>().enabled = true;
        }

        if (GameObject.Find("PlayerTwo") != null)
        {
            GameObject.Find("PlayerTwo").GetComponent<PlayerControl>().enabled = true;
        }

    }

    public void SceneChangeGUIToggle(bool mainMenuVisible, bool characterSelectVisible, bool fightVisible, bool victoryVisible)
    {
        ToggleGUI(mainMenuGUI, mainMenuVisible);
        ToggleGUI(characterSelectGUI, characterSelectVisible);
        ToggleGUI(fightGUI, fightVisible);
        ToggleGUI(victoryGUI, victoryVisible);
    }

    public void CharacterSelected(string playerTag, int characterIndex)
    {
        switch (playerTag)
        {
            case ("Player1"):
                playerOneCharacter = characterOnePrefabs[characterIndex];
                break;
            case ("Player2"):
                playerTwoCharacter = characterTwoPrefabs[characterIndex];
                break;
            default:
                Debug.LogError("GameManager.cs: Player tag not found!");
                break;
        }
    }

    IEnumerator SceneFadeOut()
    {
        musicController.MusicFadeOut();

        while (fadeOutImage.alpha < 1)
        {
            fadeOutImage.alpha += fadeStep;
            yield return new WaitForSeconds(fadeStep);
        }

        SceneChangeGUIToggle(false, false, false, false);

        LoadNewScene();
    }

    IEnumerator SceneFadeIn()
    {
        while (fadeOutImage.alpha > 0)
        {
            fadeOutImage.alpha -= fadeStep;
            yield return new WaitForSeconds(fadeStep);
        }

        if (activeScene == fightScene)
        {
            StartCoroutine(RoundStartCountDown());
        }
    }

    IEnumerator RoundStartCountDown()
    {
        countdownText.enabled = true;
        countdownText.sprite = countThree;
        countdownText.SetNativeSize();
        yield return new WaitForSecondsRealtime(countdownTimer);
        countdownText.sprite = countTwo;
        countdownText.SetNativeSize();
        yield return new WaitForSecondsRealtime(countdownTimer);
        countdownText.sprite = countOne;
        countdownText.SetNativeSize();
        yield return new WaitForSecondsRealtime(countdownTimer);
        countdownText.sprite = countFight;
        countdownText.SetNativeSize();
        audioSource.clip = bellRoundStart;
        audioSource.Play();
        FindAndEnablePlayers();
        yield return new WaitForSecondsRealtime(countdownTimer);
        countdownText.sprite = null;
        countdownText.enabled = false;
    }

    public void FighterSpawnerID(PlayerSpawner pSpawner)
    {
        playerSpawner = pSpawner;
        playerSpawner.InputFighterData(playerOneCharacter, "Player1");
        playerSpawner.InputFighterData(playerTwoCharacter, "Player2");
        playerSpawner.SpawnFighters();
    }
}
