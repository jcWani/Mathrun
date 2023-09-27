using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quizManager : MonoBehaviour
{
    [Header("Question and Answer Area")]
    public string[] questionList;
    public string curQuestion;
    public string[] answerList;
    public string curAnswer;
    List<int> previousQuestions = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    public int questionNumber = 0;

    [Header("UI")]
    public Text questionTextDisp;
    public Button submitBtn;
    int ItemCount = -1;

    #region fields
    public Text inputText;
    private string inputString;
    #endregion fields

    float curTime = 0f;
    float startTime = 10f;
    public Text countdownText;


    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        curTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemCount == -1)
        {
            ItemCount = Random.Range(0, questionList.Length);

            for(int a = 0; a < 10; a++)
            {
                if(ItemCount != previousQuestions[a])
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

        curTime -= 1 * Time.deltaTime;
        countdownText.text = curTime.ToString("0");

        if(curTime <= 0.0f)
        {
            curTime = 10f;
            ItemCount = -1;
        }
    }
    
    public void OnAnswering()
    {
        if(inputText.text == curAnswer)
        {
            inputText.text = "";
            Debug.Log("Correct");
        }
        else
        {
            inputText.text = "";
            Debug.Log("Wrong");

        }
        questionNumber += 1;
        ItemCount = -1;
        inputString = "";
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
    #endregion Methods

    #region Events
    #endregion Events
}
