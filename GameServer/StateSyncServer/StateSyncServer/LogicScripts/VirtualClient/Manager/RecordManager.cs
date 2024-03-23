using StateSyncServer.LogicScripts.VirtualClient.Manager.Base;

namespace StateSyncServer.LogicScripts.VirtualClient.Manager
{
    public class RecordManager : Manager<RecordManager>
    {
        public void AddRecord(string msg)
        {
            _EventDispatcher.GetInstance().Event(_EventDispatcher.ADD_RECORD, msg);
        }
    }
}