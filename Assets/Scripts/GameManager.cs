using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource run;
    public AudioSource deathS;
    public AudioSource collectcoinS;
    public AudioSource correct;
    public AudioSource wrongA;
    public AudioSource magnetSound;
    public AudioSource buffSound;
    public AudioSource springSound;
    public GameObject buffParticle;
    public GameObject magnetParticle;
    public GameObject springParticle;
    public static int COIN_SCORE_AMOUNT = 5;
    public const int QUIZ_SCORE_AMOUNT = 100;

    public static GameManager Instance { set; get; }

    public bool isDead { set; get; }
    private bool isGameStarted = false;
    public static PlayerMotor motor;

    //UI and UI fields
    public Animator gameCanvasAnim;
    public GameObject Menu;
    public Text scoreText, coinText, modifierText, highscoreText;
    private float score, modifierScore;
    private int coinScore;
    private int lastScore;

    //Death Menu
    public Animator deathMenuAnim;
    public Text deadscoreText, deadcoinText;

    [Header("Question and Answer Area")]
    public string[] questionList;
    public string curQuestion;
    public string[] answerList;
    public string curAnswer;
    List<int> previousQuestions = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    public int questionNumber = 0;

    [Header("UI")]
    public GameObject quizManager;
    public TextMeshProUGUI questionTextDisp;
    public Button submitBtn;
    int ItemCount = -1;

    #region fields
    public Text inputText;
    private string inputString;
    #endregion fields

    float curTime = 0f;
    float startTime = 60f;
    public Text countdownText;
    public Image remainingTimeDial;         
    private float remainingTimeDialRate;
    public Image Bar;
    private float coinBarFillRate = 10;
    private float coinFill = 10;
    private float coinBarFill = 0f;

    public Animator plusPts, minusPts, check, wrong;
    public GameObject Plus, Wrong;
    public GameObject pauseBtn;

    public static int totalCoins;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = score.ToString("0");
        intro.Play();
        quizManager.SetActive(false);
        curTime = startTime;
        remainingTimeDialRate = 1.0f / startTime;
        coinBarFillRate = 1.0f / coinFill;
        highscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
        totalCoins = PlayerPrefs.GetInt("totalCoins", 0);
    }

    private void Update()
    {
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<SideSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvasAnim.SetTrigger("Show");
            Menu.SetActive(false);
            pauseBtn.SetActive(true);
            intro.Stop();
            run.Play();
        }

        if (isGameStarted && !isDead)
        {
            //Bump Score Up
            score += (Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }

        if (coinBarFill == 0)
        {
            
        }
        else if (coinBarFill % 10 == 0)
        {
            quiz();
        }

        Bar.fillAmount = coinBarFillRate * coinBarFill;

        if(score < 0)
        {
            score = 0;
        }
    }

    public void GetCoin()
    {
        coinScore++;
        totalCoins++;
        PlayerPrefs.SetInt("totalCoins", totalCoins);
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
        coinBarFill++;
        collectcoinS.Play();
    }

    public void Buff()
    {
        COIN_SCORE_AMOUNT = 10;
    }

    public void deBuff()
    {
        COIN_SCORE_AMOUNT = 5;
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        deathS.Stop();
    }

    public void PauseBtn()
    {
        Time.timeScale = 0.0f;
        run.Stop();
        buffSound.Pause();
        magnetSound.Pause();
        springSound.Pause();
    }

    public void restartBtn()
    {
        Time.timeScale = 1.0f;
        buffSound.Stop();
        magnetSound.Stop();
        springSound.Stop();
        buffParticle.SetActive(false);
        magnetParticle.SetActive(false);
        springParticle.SetActive(false);
        deBuff();
        Magnet.isMagnet = false;
        PlayerMotor.jumpForce = 5.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void resumeBtn()
    {
        Time.timeScale = 1.0f;
        run.Play();
        buffSound.UnPause();
        magnetSound.UnPause();
        springSound.UnPause();
    }

    public void homeBtn()
    {
        Time.timeScale = 1.0f;
        deBuff();
        Magnet.isMagnet = false;
        PlayerMotor.jumpForce = 5.0f;
        deathS.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroMenu");
    }

    public void OnDeath()
    {
        isDead = true;
        FindObjectOfType<SideSpawner>().IsScrolling = false;
        deadscoreText.text = score.ToString("0");
        deadcoinText.text = coinScore.ToString("0");
        gameCanvasAnim.SetTrigger("Hide");
        deathMenuAnim.SetTrigger("Dead");
        pauseBtn.SetActive(false);
        buffSound.Stop();
        magnetSound.Stop();
        springSound.Stop();
        buffParticle.SetActive(false);
        magnetParticle.SetActive(false);
        springParticle.SetActive(false);
        deBuff();
        Magnet.isMagnet = false;
        PlayerMotor.jumpForce = 5.0f;
        run.Stop();
        deathS.Play();

        // Check if this is a highscore
        if (score > PlayerPrefs.GetInt("Highscore"))
        {
            float s = score;
            if (s % 1 == 0)
                s++;

            PlayerPrefs.SetInt("Highscore", (int)s);
        }
    }

    public void OnAnswering()
    {
        if (inputText.text == curAnswer)
        {
            inputText.text = "";
            Debug.Log("Correct");
            score += QUIZ_SCORE_AMOUNT;
            scoreText.text = score.ToString("0");
            plusPts.SetTrigger("Plus");
            check.SetTrigger("Check");
            correct.Play();
            Time.timeScale = 1.0f;
        }
        else
        {
            inputText.text = "";
            Debug.Log("Wrong");
            score -= QUIZ_SCORE_AMOUNT;
            scoreText.text = score.ToString("0");
            minusPts.SetTrigger("Plus");
            wrong.SetTrigger("Wrong");
            wrongA.Play();
            Time.timeScale = 1.0f;
        }
        questionNumber += 1;
        ItemCount = -1;
        inputString = "";
        coinBarFill = 0;
        curTime = 60f;
        quizManager.SetActive(false);
        pauseBtn.SetActive(true);
        buffSound.UnPause();
        magnetSound.UnPause();
        springSound.UnPause();
    }

    public void clickBtn(string num)
    {
        inputString += num;
        inputText.text = inputString;
    }

    public void clickClear()
    {
        inputString = "";
        inputText.text = "";
    }

    public void quiz()
    {
        pauseBtn.SetActive(false);
        buffSound.Pause();
        magnetSound.Pause();
        springSound.Pause();
        curTime -= 1 * Time.unscaledDeltaTime;
        countdownText.text = curTime.ToString("0");
        remainingTimeDial.fillAmount = remainingTimeDialRate * curTime;
            Time.timeScale = 0.0f;
            quizManager.SetActive(true);
            if (ItemCount == -1)
            {
                ItemCount = Random.Range(0, questionList.Length);

                for (int a = 0; a < 107; a++)
                {
                    if (ItemCount != previousQuestions[a])
                    {

                    }
                    else
                    {
                        ItemCount = -1;
                    }
                }
            }

            if (ItemCount > -1)
            {
                curQuestion = questionList[ItemCount];
                curAnswer = answerList[ItemCount];

                questionTextDisp.text = curQuestion;

                previousQuestions[questionNumber] = ItemCount;

            }

            if (curTime <= 0.0f)
            {
                curTime = 60f;
                ItemCount = -1;
                questionNumber += 1;
                inputString = "";
                coinBarFill = 0;
                minusPts.SetTrigger("Plus");
                wrong.SetTrigger("Wrong");
                wrongA.Play();
                Time.timeScale = 1.0f;
                quizManager.SetActive(false);
                pauseBtn.SetActive(true);
            }
        
    }
}
