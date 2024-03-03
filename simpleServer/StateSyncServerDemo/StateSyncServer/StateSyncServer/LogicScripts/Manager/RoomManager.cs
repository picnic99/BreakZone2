using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class RoomManager : Manager<RoomManager>
    {
        //最后一个房间的Id
        private int LastRoomId = 0;
        //房间列表
        private Dictionary<int, Room> RoomMap = new Dictionary<int, Room>();
        public RoomManager()
        {

        }
        //实例化房间
        public Room CreateRoom()
        {
            var room = new Room();
            room.roomId = ++this.LastRoomId;
            room.sceneId = 1;
            this.RoomMap.Add(room.roomId, room);
            return room;
        }
        //移除房间实例
        public void DelRoom(int roomId)
        {
            if (RoomMap.ContainsKey(roomId))
            {
                this.RoomMap.Remove(roomId);

            }
        }
        //查找房间
        public Room GetRoom(int roomId)
        {
            if (RoomMap.ContainsKey(roomId))
            {
                return RoomMap[roomId];

            }
            return null;
        }
        //获取房间里面的所有玩家
        public List<Character> GetRoomPlayers(int roomId)
        {
            List<Character> result = new List<Character>();
            var list = CharacterManager.GetInstance().GetAllPlayers();

            foreach (var p in list)
            {
                if (p.data.roomId == roomId)
                {
                    result.Add(p);
                }
            }
            return result;
        }

        //获取房间里面的所有玩家id
        public List<int> GetRoomPlayerIds(int roomId)
        {
            List<int> result = new List<int>();
            var list = CharacterManager.GetInstance().GetAllPlayers();
            foreach (var p in list)
            {
                if (p.data.roomId == roomId)
                {
                    result.Add(p.data.playerId);
                }
            }
            return result;
        }

        public void Tick()
        {
            //处理玩家输入的
        }
    }
}
