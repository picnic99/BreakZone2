public class RecordManager : Singleton<RecordManager>,Manager
{
    public void AddRecord(string msg)
    {
        EventDispatcher.GetInstance().Event(EventDispatcher.ADD_RECORD, msg);
    }

    public void Init()
    {

    }
}