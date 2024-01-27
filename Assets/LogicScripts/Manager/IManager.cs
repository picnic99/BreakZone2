public interface IManager
{
#if UNITY_2019_4_9
    void Init();
#else
    void Init();
    void OnUpdate();
      void Clear();
#endif
}