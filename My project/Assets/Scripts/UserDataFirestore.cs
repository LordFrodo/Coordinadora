#if !UNITY_WEBGL || UNITY_EDITOR

using Firebase.Firestore;

[FirestoreData]
public class UserDataFirestore
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Mail { get; set; }

    [FirestoreProperty]
    public int Score { get; set; }

    public UserDataFirestore() { }

    public UserDataFirestore(UserData data)
    {
        Name = data.Name;
        Mail = data.Mail;
        Score = data.Score;
    }

    public UserData ToUserData()
    {
        return new UserData
        {
            Name = Name,
            Mail = Mail,
            Score = Score
        };
    }
}

#endif
