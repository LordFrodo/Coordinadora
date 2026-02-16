#if !UNITY_WEBGL || UNITY_EDITOR

using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseServiceNative : IFirebaseService
{
    private FirebaseFirestore db;
    private ListenerRegistration listener;

    private const string COLLECTION = "users";

    public FirebaseServiceNative()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void UploadScore(UserData data)
    {
        DocumentReference docRef = db.Collection(COLLECTION).Document(data.Name);

        UserDataFirestore firestoreData = new UserDataFirestore(data);
        docRef.SetAsync(firestoreData, SetOptions.MergeAll);
    }

    public void GetTopScores(int limit)
    {
        Query query = db.Collection(COLLECTION)
                        .OrderByDescending("Score")
                        .Limit(limit);

        listener = query.Listen(snapshot =>
        {
            List<UserData> users = new List<UserData>();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    UserDataFirestore firestoreData = doc.ConvertTo<UserDataFirestore>();
                    UserData user = firestoreData.ToUserData();
                    users.Add(user);
                }
            }

            FireBaseManager.Instance.InvokeScoreUpdate(users);
        });
    }

    public void StopListening()
    {
        listener?.Stop();
    }
}

#endif
        