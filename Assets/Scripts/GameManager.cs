using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int WINNING_POINTS = 5;
    // Singleton pattern
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameManager>();

            return _instance;
        }
    }

    public static bool hasInstance { get { return instance != null; } }

    public GameObject menuCanvas;
    public GameObject instructionsPanel;
    public TextMeshProUGUI menuText;

    private Dialogue dialogue;
    private Player player;
    private Universe universe;
    private DetectorUI detectorUI;

    private bool firstTime = true;
    private bool isShowingMenu = false;
    private bool isPlaying = false;
    private int nextTargetPoint = 0;

    public void SetNextTargetPoint()
    {
        Debug.Log("next target");
        nextTargetPoint += 1;
    }

    public Transform GetPlayerTransform()
    {
        return player.transform;
    }

    public void ReachTarget()
    {
        dialogue.PlayDialogue(nextTargetPoint);
        SetNextTargetPoint();

        if (nextTargetPoint == 5)
        {
            WinGame();
        }
    }

    public Vector3 GetNextTargetPoint()
    {
        return universe.GetNextTargetPoint(nextTargetPoint);
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    void ShowStartGameMenu()
    {
        isShowingMenu = true;
        isPlaying = false;
        menuCanvas.SetActive(true);
        menuText.text = "STARCHASER";
        instructionsPanel.SetActive(true);

        // TODO show instructions
    }

    void WinGame()
    {
        isShowingMenu = true;
        isPlaying = false;
        instructionsPanel.SetActive(false);
        detectorUI.gameObject.SetActive(false);
        menuCanvas.SetActive(true);

        menuText.text = "And as the last bytes of data flowed from the ship a singularity instantly burst from it's hull.\nThe ship was gone.";
        Debug.Log("you win");
    }

    public void GameOver(string causeMessage)
    {
        isPlaying = false;
        instructionsPanel.SetActive(false);
        detectorUI.gameObject.SetActive(false);
        menuCanvas.SetActive(true);
        isShowingMenu = true;
        menuText.text = causeMessage;
        Debug.Log(causeMessage);
    }

    public void PlayDialogue(int id)
    {
        dialogue.PlayDialogue(id);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
            Destroy(gameObject);

        dialogue = GetComponent<Dialogue>();
        player = FindObjectOfType<Player>();
        detectorUI = FindObjectOfType<DetectorUI>();
        universe = FindObjectOfType<Universe>();

    }

    void Start()
    {
        ShowStartGameMenu();
    }

    void StartGame()
    {
        universe.StartGeneration((int)System.DateTime.Now.Millisecond * 1000);
        player.Init();
        menuCanvas.SetActive(false);
        isShowingMenu = false;
        isPlaying = true;
        firstTime = false;
    }

    private bool hasPlayedFinalDialogue = false;

    // Update is called once per frame
    void Update()
    {
        if (isShowingMenu)
        {
            if (Input.GetKeyDown("space"))
            {
                if (!firstTime)
                {
                    SceneManager.LoadScene("Main", LoadSceneMode.Single);
                    return;
                }

                StartGame();
            }
        }

        // gross hack to make final dialogue work
        if (!hasPlayedFinalDialogue && nextTargetPoint == 4 && Vector3.Distance(player.transform.position, universe.GetTargetPoints()[nextTargetPoint]) <= 5000f)
        {
            hasPlayedFinalDialogue = true;
            dialogue.PlayDialogue(nextTargetPoint);
        }
    }
}
