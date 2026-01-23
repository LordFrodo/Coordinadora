using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Question
{
    public string question;
    public List<string> options;
    public int correctIndex;
}

[System.Serializable]
public class Level
{
    public int id;
    public string name;
    public int timePerQuestion;
    public List<Question> questions;
}

[System.Serializable]
public class GameData
{
    public List<Level> levels;
}

public class QuestionLoader : MonoBehaviour
{
    public System.Action OnDataLoaded;

    public TextAsset questionsJson;
    public GameData gameData;

    void Awake()
    {
        gameData = JsonUtility.FromJson<GameData>(questionsJson.text);
        StartCoroutine("DataLoaded", 1.5f);
    }
    
    public void DataLoaded()
    {
        OnDataLoaded?.Invoke();
    }
}
