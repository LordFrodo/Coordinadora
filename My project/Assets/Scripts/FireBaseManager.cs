using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager Instance;

    private FirebaseFirestore db;
    private ListenerRegistration scoreboardListener;

    public delegate void ScoreboardUpdated(List<UserData> users);
    public event ScoreboardUpdated OnScoreboardUpdated;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        db = FirebaseFirestore.DefaultInstance;
    }

    // 🔼 SUBIR O ACTUALIZAR SCORE
    public void UploadUserScore( UserData data)
    {
        DocumentReference docRef = db.Collection("users").Document(data.Name);

        docRef.SetAsync(data, SetOptions.MergeAll)
              .ContinueWithOnMainThread(task =>
              {
                  if (task.IsFaulted)
                      Debug.LogError("Error subiendo score");
              });
    }

    // 👂 ESCUCHAR TOP 8 EN TIEMPO REAL
    public void StartListeningScoreboard()
    {
        Query query = db.Collection("users")
                        .OrderByDescending("Score")
                        .Limit(8);

        scoreboardListener = query.Listen(snapshot =>
        {
            List<UserData> users = new List<UserData>();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    UserData user = doc.ConvertTo<UserData>();
                    users.Add(user);
                }
            }

            OnScoreboardUpdated?.Invoke(users);
        });
    }

    private void OnDestroy()
    {
        scoreboardListener?.Stop();
    }
}
