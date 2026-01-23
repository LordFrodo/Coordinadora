using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

public class SlideAnimation:MonoBehaviour
{

    [Header("UI")]
    [Tooltip("Slider o Image con Fill")]
    public Slider securitySlider;

    [Header("Configuración")]
    [Range(0f, 1f)]
    public float initialValue = 1f;

    [Tooltip("Cuánto se recupera al responder bien (0 = no recupera)")]
    public float rewardMultiplier = 1f;

    [Header("Eventos")]
    public UnityEvent OnBarEmpty;
    public FloatEvent OnPercentageChanged;

    [Header("GameManager")]
    [SerializeField] private QuizGameManager quizGameManager;

    private float currentValue;
    private float stepPerQuestion;


    void OnEnable() => quizGameManager.loader.OnDataLoaded += Initialize;

    void OnDisable() => quizGameManager.loader.OnDataLoaded -= Initialize;
    /// <summary>
    /// Inicializa la barra usando el total de preguntas del juego
    /// </summary>
    public void Initialize()
    {
        int totalQuestions = 0;
        foreach (var item in quizGameManager.loader.gameData.levels)
        {
            totalQuestions += item.questions.Count;
        }
        
        currentValue = Mathf.Clamp01(initialValue);
        stepPerQuestion = 1f / Mathf.Max(1, totalQuestions);

        UpdateUI();
    }

    /// <summary>
    /// Llamar cuando el jugador responde correctamente
    /// </summary>
    public void CorrectAnswer()
    {
        ModifyBar(stepPerQuestion * rewardMultiplier);
    }

    /// <summary>
    /// Llamar cuando el jugador falla o se acaba el tiempo
    /// </summary>
    public void WrongAnswer()
    {
        ModifyBar(-stepPerQuestion);
    }

    private void ModifyBar(float amount)
    {
        currentValue = Mathf.Clamp01(currentValue + amount);
        UpdateUI();

        OnPercentageChanged?.Invoke(currentValue * 100f);

        if (currentValue <= 0f)
        {
            OnBarEmpty?.Invoke();
        }
    }

    private void UpdateUI()
    {
        if (securitySlider != null)
        {
            securitySlider.value = currentValue;
        }
    }

    /// <summary>
    /// Permite consultar el porcentaje actual (0–100)
    /// </summary>
    public float GetPercentage()
    {
        return currentValue * 100f;
    }
}
