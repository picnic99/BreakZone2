using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class RecordManager : Manager<RecordManager>
    {
        public void AddRecord(string msg)
        {
            EventDispatcher.GetInstance().Event(EventDispatcher.ADD_RECORD, msg);
        }
    }
}