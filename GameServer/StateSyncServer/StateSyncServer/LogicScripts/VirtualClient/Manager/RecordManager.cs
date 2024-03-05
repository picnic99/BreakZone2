public class RecordManager : Manager<RecordManager> 
{
    public void AddRecord(string msg)
    {
        EventDispatcher.GetInstance().Event(EventDispatcher.ADD_RECORD, msg);
    }
}