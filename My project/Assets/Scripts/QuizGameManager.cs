using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class QuizGameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;
    public TextMeshProUGUI timerText;
    public CanvasGroup questionPanel;

    [Header("Data")]
    public QuestionLoader loader;
    [Header("Slider")]
    public SlideAnimation slider;

    private Level currentLevel;
    private int questionIndex = 0, level = 0, score = 0;
    private float timeRemaining;
    private bool isAnswering = false, isGameRunning;

    [Header("Complete Game")]
    public GameObject completeScreen; 
    public GameObject hackedPanel; 
    public GameObject gameScreen; 
    public TextMeshProUGUI titleCompleteGame;
    public TextMeshProUGUI bodyCompleteGame;


    public void LoadLevel(int levelIndex)
    {
        if(isGameRunning && slider.securitySlider.value > 0)
        {
            if(levelIndex < loader.gameData.levels.Count)
            {
                isAnswering = true;
                currentLevel = loader.gameData.levels[levelIndex];
                questionIndex = 0;
                LoadQuestion();
            }
            else
            {
                isAnswering= false;
                currentLevel = null;
                questionIndex = 0;
                GameComplete();
            }
        }
    }

    public void StartGame()
    {
        isGameRunning = true;
        level = 0;
        slider.securitySlider.value = 1;
        slider.currentValue = 1;
        LoadLevel(0);
    }

    public void GameComplete()
    {
        isGameRunning = false;
        isAnswering = false;
        currentLevel = null;
        gameScreen.SetActive(false);
        completeScreen.SetActive(true);
        hackedPanel.SetActive(slider.securitySlider.value < 0.45f);

    }

    void LoadQuestion()
    {
        if (!isGameRunning) return;
        if (questionIndex >= currentLevel.questions.Count)
        {
            Debug.Log("Nivel completado: "+level);
            level++;
            LoadLevel(level);
            return;
        }

        StartCoroutine(TransitionOutIn());

        Question q = currentLevel.questions[questionIndex];
        questionText.text = q.question;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            if (i < q.options.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionTexts[i].text = q.options[i];
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => Answer(index));
            }
            else optionButtons[i].gameObject.SetActive(false);
        }

        timeRemaining = currentLevel.timePerQuestion;
        isAnswering = true;
    }

    void Update()
    {
        if (!isAnswering) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            isAnswering = false;
            slider.WrongAnswer();
            NextQuestion();
        }
    }

    void Answer(int index)
    {
        isAnswering = false;
        int correct = currentLevel.questions[questionIndex].correctIndex;

        if (index == correct)
        {
            Debug.Log("Respuesta correcta");
            score += 20 * (int)slider.rewardMultiplier * (int)slider.stepPerQuestion;
        }
        else
        {
            Debug.Log("Respuesta incorrecta");
            slider.WrongAnswer();
        }

        NextQuestion();
    }

    void NextQuestion()
    {
        questionIndex++;
        Invoke(nameof(LoadQuestion), 0.5f);
    }

    IEnumerator TransitionOutIn()
    {
        yield return Fade(1, 0);
        yield return Fade(0, 1);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            questionPanel.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }
}
