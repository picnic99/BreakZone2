/// <summary>
/// 单例工具类
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T: Manager,new() 
{
    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = new T();
            _instance.Init();
        }
        return _instance;
    }
}