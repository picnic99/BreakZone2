public class RecordManager : Manager<RecordManager> 
{
    public void AddRecord(string msg)
    {
        _EventDispatcher.GetInstance().Event(_EventDispatcher.ADD_RECORD, msg);
    }
}