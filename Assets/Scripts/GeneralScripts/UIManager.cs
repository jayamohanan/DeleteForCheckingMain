using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Start Screen")]
    public GameObject startScreen;

    [Header("In Game Screen")]
    public GameObject inGameScreen;
    public TextMeshProUGUI levelNoText;
    public TextMeshProUGUI goodWordText;
    public GameObject dragTut;

    [Header("Win Screen")]
    public GameObject winScreen;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI multiplierText;
    public GameObject[] NAConfettiArray;
    public GameObject scoreGainedPanel;
    public TextMeshProUGUI scoreGainedText;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public GameObject nextButtonObj;

    [Header("Lose Screen")]
    public GameObject loseScreen;

    private Camera mainCam;
    private GameManager gameManager;
    private ScoreManager scoreManager;
    private int uiLayer;

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        mainCam = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        uiLayer = LayerMask.NameToLayer("UI");
    }
    private void OnEnable()
    {
        //gameManager = FindObjectOfType<GameManager>();
        gameManager.LevelSetEvent += OnLevelSet;
        gameManager.LevelOverEvent += OnLevelOver;
        gameManager.UserInputStarted += OnFirstUserInputReceived;

    }
    private void OnDisable()//
    {
        gameManager.LevelSetEvent -= OnLevelSet;
        gameManager.LevelOverEvent -= OnLevelOver;
        gameManager.UserInputStarted -= OnFirstUserInputReceived;

    }

    private void OnLevelSet(int levelNumber)
    {
        SetLevelStartUI(levelNumber);
        //if(levelNumber == 1)
        //{
        //    dragTut.SetActive(true);
        //}
    }
    private void OnLevelOver(bool win, int multiplier)
    {
        if(win)
            SetWinScreen(true, multiplier);
        else
            SetLoseScreen(true);
    }
    //private void OnLevelWasLoaded(int level)
    //{
    //    gameManager = FindObjectOfType<GameManager>();
    //    gameManager.LevelSetEvent += OnLevelSet;
    //    gameManager.LevelOverEvent += OnLevelOver;
    //    mainCam = Camera.main;
    //}

    public void SetStartScreen(bool active)
    {
        scorePanel.gameObject.SetActive(false);
        StopAllCoroutines();//To stop game win confetti if playing
        if(confParent!=null)
        Destroy(confParent.gameObject);
        startScreen.SetActive(active);
    }
    public void SetInGameScreen(bool active, int levelNumber)
    {
        if (active)
            levelNoText.text = "Level "+levelNumber.ToString();
        inGameScreen.SetActive(active);
        if (levelNumber == 1)
        {
            dragTut.SetActive(true);
        }
    }
    public void SetWinScreen(bool active, int multiplier = 1, string status = "Level\nCompleted!")
    {
        if (active)
        {
            //ShowGoodWordText();
            statusText.text = status;
            //if(multiplier!=-1)
            //multiplierText.text = multiplier.ToString() + "%";
            //StartCoroutine(PlayConfetti(multiplier));
            //scoreGainedPanel.SetActive(true);
            Invoke("ShowNextButton",2);
            if(multiplier == 1)
            {
                coinCount = 3;
                scoreGainedText.text = "+3";
            }
            else if(multiplier == 2)
            {
                coinCount = 5;
                scoreGainedText.text = "+5";
            }
            else if(multiplier == 3)
            {
                coinCount = 10;
                scoreGainedText.text = "+10";
            }
            else if(multiplier == 4)
            {
                coinCount = 25;
                scoreGainedText.text = "+25";
                Invoke("ShowCoinAnim", 0.5f);//
            }
        }
        winScreen.SetActive(active);
    }
    int coinCount;
    private void ShowCoinAnim()
    {
        scoreManager.AddScore(coinCount);
    }
    private void ShowNextButton()
    {
        nextButtonObj.SetActive(true);
    }
    public void SetLoseScreen(bool active)
    {
        if (active)
        {
            Invoke("LoseScreenTimer", 1);
            //StartCoroutine("LoseScreenTimer");
        }
        else
            loseScreen.SetActive(false);

    }
    private /*IEnumerator*/void LoseScreenTimer() 
    { 
        //yield return new WaitForSeconds(1);
        loseScreen.SetActive(true);
    }

    public void SetLevelStartUI(int levelNumber)
    {
        SetStartScreen(true);
        if(levelNumber == 1)
        {

        }
        SetInGameScreen(true, levelNumber);
        SetWinScreen(false);
        SetLoseScreen(false);
    }
    public void Retry()
    {
        SetLoseScreen(false);
        gameManager.Retry();
    }
    public void NextLevel()
    {
        if (confParent != null)
            Destroy(confParent.gameObject);
        SetWinScreen(false);
       gameManager.NextLevel();//Should come first
    }
    private Transform confParent;
    private IEnumerator PlayConfetti(int count)
    {
        yield return new WaitForSeconds(0.1f);
        confParent = new GameObject("ConfParent").transform;
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .65f, 8));
        AudioManager.Instance.PlayWowSound();
        GameObject obj;

        WaitForSeconds wfs = new WaitForSeconds(0.3f);
        for (int i = 0; i < count; i++)
        {
            pos.z = 1;
            obj = Instantiate(NAConfettiArray[0], pos, Quaternion.Euler(Vector3.zero), confParent);
            obj.GetComponentInChildren<ParticleSystemRenderer>().sortingOrder = 150;
            yield return wfs;
            Destroy(obj, 8);
        }
    }
    public void PlayConfettiAtPosition(Vector3 position)
    {
        GameObject confetti = Instantiate(NAConfettiArray[0], position, Quaternion.identity);
        Destroy(confetti, 3);
    }
    public void SetScoreText(int s)
    {
        scorePanel.gameObject.SetActive(true);
        scoreText.text = s.ToString();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnFirstUserInputReceived()
    {
        SetStartScreen(false);
    }
    private int goodTextIndex = -1;
    public void ShowGoodWordText()
    {
        goodWordText.gameObject.SetActive(false);
        goodWordText.gameObject.SetActive(true);
        goodTextIndex = PlayerPrefs.GetInt("GoodTextIndex");
        if (goodTextIndex >= goodWords.Count)
            goodTextIndex = 0;
        goodWordText.text = goodWords[goodTextIndex];
        PlayConfettiAtPosition(goodWordText.transform.position);
        goodTextIndex++;
        PlayerPrefs.SetInt("GoodTextIndex", goodTextIndex);
    }
    public void HideGoodWordText()
    {
        goodWordText.gameObject.SetActive(false);
    }
    private List<string> goodWords = new List<string>()
    {
        "WOW", "GOOD","PERFECT", "AMAZING", "INCREDIBLE","BRILLIANT","FANTASTIC",
        "MARVELLOUS","SUPER","MASTER","NAILED IT","SPEECHLESS","BEAUTY","PUZZLER","KEEP IT UP"
    };
}
