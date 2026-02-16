using System.Collections.Generic;

public interface IFirebaseService
{
    void UploadScore(UserData data);
    void GetTopScores(int limit);
}
    