using System;
using UnityEngine.SceneManagement;


/// <summary>
/// ×´Ì¬¹ÜÀíÆ÷
/// </summary>
public class SceneManager : Singleton<SceneManager>,Manager
{
    public void ChangeScene(string sceneName,Action call)
    {
        UIManager.GetInstance().ClearAllUI();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged+=(a,b)=>call.Invoke();
    }

    public void Init()
    {

    }
}