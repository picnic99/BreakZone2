public interface Manager
{
#if UNITY_2019_4_9
    void Init();
#else
    public abstract void Init();
#endif
}