using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class CharacterSelect : MonoBehaviour
{
    [SerializeField] Color playerOneColor;
    [SerializeField] Color playerTwoColor;
    [SerializeField] float flashDelay = 0.1f;
    // Panels must be loaded into field in numerical order for this code to work
    [SerializeField] Image[] panels;
    [SerializeField] Text p1CharacterName;
    [SerializeField] Slider p1AttackPower;
    [SerializeField] Slider p1NeckSpeed;
    [SerializeField] Slider p1MoveSpeed;
    [SerializeField] Text p2CharacterName;
    [SerializeField] Slider p2AttackPower;
    [SerializeField] Slider p2NeckSpeed;
    [SerializeField] Slider p2MoveSpeed;
    [SerializeField] float maxAttack;
    [SerializeField] float maxNeckSpeed;
    [SerializeField] float maxSpeed;

    GameManager gameManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip playerHighlightSound;
    [SerializeField] AudioClip playerSelectSound;

    int playerOneSelection = 1;
    int playerTwoSelection = 5;
    const int randomCharacter = 5;
    const string player1tag = "Player1";
    const string player2tag = "Player2";

    GameObject p1Prefab;
    int p1Index = -1;
    Vector3 p1SpawnPos;
    GameObject p2Prefab;
    int p2Index = -1;
    Vector3 p2SpawnPos;


    bool isPlayerOneSelected;
    bool isPlayerTwoSelected;
    bool isLoading;
    float p1MovingTimer = 0;
    float p2MovingTimer = 0;
    [Range(0, 1), SerializeField] float moveTimer = 0.3f;
    [Range(0, 0.9f), SerializeField] float deadZone = 0.5f;

    [SerializeField] Image p1Random;
    [SerializeField] Image p2Random;

    void Start()
    {
        ResetSelection();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HighlightCharacters();

        p1SpawnPos = new Vector3(0, 0, -15);
        p2SpawnPos = new Vector3(0, 0, 15);

        p1Random.enabled = false;
        p2Random.enabled = false;
    }

    void Update()
    {
        CheckPlayersInput();

        if (isPlayerOneSelected && isPlayerTwoSelected && !isLoading)
        {

            StopAllCoroutines();
            isLoading = true;
            gameManager.PlayGame(false);
        }
        if (p1Index != playerOneSelection && gameManager.isCharacterSelect)
        {
            GameObject.Destroy(p1Prefab);
            if (playerOneSelection != 5)
            {
                p1Prefab = Instantiate(gameManager.characterOnePrefabs[playerOneSelection], p1SpawnPos, gameManager.characterOnePrefabs[playerOneSelection].transform.rotation);
                p1Prefab.transform.Find("Head").GetComponentInChildren<Billboard>().enabled = false;
                p1Random.enabled = false;
            }
            else
            {
                p1Random.enabled = true;
            }
            p1Index = playerOneSelection;
        }

        if (p2Index != playerTwoSelection && gameManager.isCharacterSelect)
        {
            GameObject.Destroy(p2Prefab);
            if (playerTwoSelection != 5)
            {
                p2Prefab = Instantiate(gameManager.characterTwoPrefabs[playerTwoSelection], p2SpawnPos, gameManager.characterTwoPrefabs[playerTwoSelection].transform.rotation);
                p2Prefab.transform.Find("Head").GetComponentInChildren<Billboard>().enabled = false;
                p2Random.enabled = false;
            }
            else
            {
                p2Random.enabled = true;
            }
            p2Index = playerTwoSelection;
        }

        //p1Prefab.GetComponent<PlayerControl>().startTimer = 1.5f;
        //p2Prefab.GetComponent<PlayerControl>().startTimer = 1.5f;
        if(p1Prefab != null)
        {
            p1Prefab.GetComponent<PlayerControl>().enabled = false;
        }

        if (p2Prefab != null)
        {
            p2Prefab.GetComponent<PlayerControl>().enabled = false;
        }


        UpdateStats("Player1", playerOneSelection);
        UpdateStats("Player2", playerTwoSelection);
    }

    private void OnGUI()
    {
        if (p1CharacterName != null)
        {
            switch (playerOneSelection)
            {
                case (0):
                    p1CharacterName.text = "Kaiser Longenecker";
                    break;
                case (1):
                    p1CharacterName.text = "Girmit";
                    break;
                case (2):
                    p1CharacterName.text = "Fun Guy Raph";
                    break;
                case (3):
                    p1CharacterName.text = "Acacia";
                    break;
                case (4):
                    p1CharacterName.text = "Wiz Kid";
                    break;
                case (5):
                    p1CharacterName.text = "Random";
                    break;
                default:
                    break;
            }
        }

        if (p2CharacterName != null)
        {
            switch (playerTwoSelection)
            {
                case (0):
                    p2CharacterName.text = "Kaiser Longenecker";
                    break;
                case (1):
                    p2CharacterName.text = "Girmit";
                    break;
                case (2):
                    p2CharacterName.text = "Fun Guy Raph";
                    break;
                case (3):
                    p2CharacterName.text = "Acacia";
                    break;
                case (4):
                    p2CharacterName.text = "Wiz Kid";
                    break;
                case (5):
                    p2CharacterName.text = "Random";
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateStats(string player, int fighterSelected)
    {
        if (player == "Player1")
        {
            if (fighterSelected < 5)
            {
                PlayerControl fighter = gameManager.characterOnePrefabs[fighterSelected].GetComponent<PlayerControl>();
                p1AttackPower.value = fighter.damageFactor / maxAttack;
                p1NeckSpeed.value = fighter.neckSpeed / maxNeckSpeed;
                p1MoveSpeed.value = fighter.speed / maxSpeed;
            }
            else
            {
                p1AttackPower.value = 0;
                p1NeckSpeed.value = 0;
                p1MoveSpeed.value = 0;
            }
        }
        else if (player == "Player2")
        {
            if (fighterSelected < 5)
            {
                PlayerControl fighter = gameManager.characterTwoPrefabs[fighterSelected].GetComponent<PlayerControl>();
                p2AttackPower.value = fighter.damageFactor / maxAttack;
                p2NeckSpeed.value = fighter.neckSpeed / maxNeckSpeed;
                p2MoveSpeed.value = fighter.speed / maxSpeed;
            }
            else
            {
                p2AttackPower.value = 0;
                p2NeckSpeed.value = 0;
                p2MoveSpeed.value = 0;
            }
        }
    }

    public void ResetSelection()
    {
        playerOneSelection = 1;
        playerTwoSelection = 5;
        isPlayerTwoSelected = false;
        isPlayerOneSelected = false;
        ResetPanelColor();
        HighlightCharacters();
    }

    private void ResetPanelColor()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].color = Color.clear;
        }
    }

    private void CheckPlayersInput()
    {
        int selectionModifier;

        if (!isPlayerOneSelected && p1MovingTimer <= 0)
        {
            if (Mathf.Abs(Input.GetAxis("LeftHorizontal1")) >= deadZone)
            {
                selectionModifier = Input.GetAxis("LeftHorizontal1") > 0 ? 2 : -2;
                playerOneSelection = (playerOneSelection + selectionModifier + 6) % 6;
                HighlightCharacters();
            }
            else if (Mathf.Abs(Input.GetAxis("LeftVertical1")) >= deadZone)
            {
                if (playerOneSelection % 2 == 0)
                {
                    playerOneSelection++;
                }
                else
                {
                    playerOneSelection--;
                }
                HighlightCharacters();
            }
            else if (Input.GetButtonDown("Submit"))
            {
                isPlayerOneSelected = true;
                HighlightCharacters();
                SelectCharacter(player1tag);
            }

            p1MovingTimer = moveTimer;
        }
        else
        {
            p1MovingTimer -= Time.deltaTime;
            if (Mathf.Abs(Input.GetAxis("LeftHorizontal1")) <= deadZone && Mathf.Abs(Input.GetAxis("LeftVertical1")) <= deadZone)
                p1MovingTimer = 0;
        }

        if (!isPlayerTwoSelected && p2MovingTimer <= 0)
        {
            if (Mathf.Abs(Input.GetAxis("LeftHorizontal2")) >= deadZone)
            {
                selectionModifier = Input.GetAxis("LeftHorizontal2") > 0 ? 2 : -2;
                playerTwoSelection = (playerTwoSelection + selectionModifier + 6) % 6;
                HighlightCharacters();
            }
            else if (Mathf.Abs(Input.GetAxis("LeftVertical2")) >= deadZone)
            {
                if (playerTwoSelection % 2 == 0)
                {
                    playerTwoSelection++;
                }
                else
                {
                    playerTwoSelection--;
                }
                HighlightCharacters();
            }
            else if (Input.GetButtonDown("Submit2"))
            {
                isPlayerTwoSelected = true;
                SelectCharacter(player2tag);
                HighlightCharacters();
            }
            p2MovingTimer = moveTimer;
        }
        else
        {
            p2MovingTimer -= Time.deltaTime;
            if (Mathf.Abs(Input.GetAxis("LeftHorizontal2")) <= deadZone && Mathf.Abs(Input.GetAxis("LeftVertical2")) <= deadZone)
                p2MovingTimer = 0;
        }
    }

    private void SelectCharacter(string playerTag)
    {
        int selectedCharacter;

        audioSource.clip = playerSelectSound;
        audioSource.Play();

        if (playerTag == player1tag)
        {
            if (playerOneSelection == randomCharacter)
            {
                selectedCharacter = Random.Range(0, gameManager.characterOnePrefabs.Length - 1);
            }
            else
            {
                selectedCharacter = playerOneSelection;
            }

            gameManager.CharacterSelected(player1tag, selectedCharacter);
        }

        if (playerTag == player2tag)
        {
            if (playerTwoSelection == randomCharacter)
            {
                selectedCharacter = Random.Range(0, gameManager.characterTwoPrefabs.Length - 1);
            }
            else
            {
                selectedCharacter = playerTwoSelection;
            }

            gameManager.CharacterSelected(player2tag, selectedCharacter);
        }
    }

    private void HighlightCharacters()
    {
        StopAllCoroutines();
        if (audioSource)
        {
            Debug.Log("Audio Source instantiated");
            audioSource.clip = playerHighlightSound;
            audioSource.Play();
        }
        else
        {
            Debug.Log("Audio Source not instantiated properly");
        }
        StartCoroutine(CharacterBorderFlash());
    }

    IEnumerator CharacterBorderFlash()
    {
        Color playerOneSelectedColor = isPlayerOneSelected == true ? playerOneColor : Color.clear;
        Color playerTwoSelectedColor = isPlayerTwoSelected == true ? playerTwoColor : Color.clear;

        ResetPanelColor();

        while (true)
        {
            if (panels[playerOneSelection] != panels[playerTwoSelection])
            {
                panels[playerOneSelection].color = playerOneColor;
                panels[playerTwoSelection].color = playerTwoSelectedColor;
            }
            else
            {
                panels[playerOneSelection].color = playerOneColor;
            }

            yield return new WaitForSeconds(flashDelay);

            if (panels[playerOneSelection] != panels[playerTwoSelection])
            {
                panels[playerOneSelection].color = playerOneSelectedColor;
                panels[playerTwoSelection].color = playerTwoColor;
            }
            else
            {
                panels[playerTwoSelection].color = playerTwoColor;
            }

            yield return new WaitForSeconds(flashDelay);
        }
    }
}
