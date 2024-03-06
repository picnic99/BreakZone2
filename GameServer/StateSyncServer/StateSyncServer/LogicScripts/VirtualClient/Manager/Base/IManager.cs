namespace StateSyncServer.LogicScripts.VirtualClient.Manager.Base
{
    public interface IManager
    {

        void Init();
        void OnUpdate();
        void Clear();
    }
}