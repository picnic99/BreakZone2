using StateSyncServer.LogicScripts.Enum;
using StateSyncServer.LogicScripts.VO;
using System.Collections.Generic;
using System.Net.Sockets;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Util;
using Msg;
using StateSyncServer.LogicScripts.Net.PB;
using StateSyncServer.LogicScripts.Net.PB.Enum;

namespace StateSyncServer.LogicScripts.Manager
{
    class FightActionType
    {
        /// <summary>
        /// //开始移动
        /// </summary>
        public static string START_MOVE = "START_MOVE";
        /// <summary>
        /// 停止移动
        /// </summary>
        public static string END_MOVE = "END_MOVE";
        /// <summary>
        /// 旋转
        /// </summary>
        public static string ROTATE = "ROTATE";
    }

    class ActionManager : Manager<ActionManager>
    {
        public override void AddListener()
        {
            base.AddListener();
            ActionManager.GetInstance().On(ProtocolId.CLIENT_PLAYER_LOGIN_REQ, LoginReqHandle);
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_PLAYER_LOGIN_REQ, LoginReqHandle);
        }


        /*        private void Handle_GetData(Protocol proto)
                {
                    GetGameDataReq action = (GetGameDataReq) proto;
                    var playerId = action.playerId;
                    SyncGameData(playerId);
                }
        */
        private void LoginReqHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            PlayerLoginReq req = proto.GetDataInstance<PlayerLoginReq>();
            var username = req.Username;
            var password = req.Password;
            var type = req.Type;


            if (type == 1)
            {
                //登录
                CommonUtils.Logout($"玩家{username}登录");
                Global.PlayerSocketMap.Add(username, proto.client);
                PlayerManager.GetInstance().GetPlayer(username, password);
                //协议返回
                PlayerLoginRep rep = new PlayerLoginRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.Type = type;
                NetManager.GetInstance().SendProtocol(proto.client, rep);
                //同步一下玩家信息
            }
            if (type == 2)
            {
                //注销
                Global.PlayerSocketMap.Remove(username);
                //协议返回
                PlayerLoginRep rep = new PlayerLoginRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.Type = type;
                CommonUtils.Logout($"玩家{username}登出");
                NetManager.GetInstance().SendProtocol(proto.client, rep);

            }
        }

        /*        private void Handle_LoginIn(TcpClient client, Protocol proto)
                {
                    LoginInReq action = (LoginInReq)proto;

                    var playerId = action.playerId;
                    CommonUtils.Logout(playerId + "");
                    Global.PlayerSocketMap.Add(playerId, client);
                    CharacterManager.GetInstance().CreatePlayer(playerId);
                    //协议返回
                    LoginInRep protocol = new LoginInRep(1, playerId);
                    //NetManager.GetInstance().SendProtocol(client, protocol);
                }

                private void Handle_LoginOut(Protocol proto)
                {
                    LoginInReq action = (LoginInReq)proto;
                    var playerId = action.playerId;
                    if (Global.PlayerSocketMap.ContainsKey(playerId))
                    {
                        Global.PlayerSocketMap.Remove(playerId);
                    }
                }

                //战斗行为处理
                public void FightHandle(PlayerOptReq action)
                {

                    switch (action.type)
                    {
                        // case FightActionEnum.IDLE:
                        //     this.Handle_Idle();
                        //     break;
                        case FightActionEnum.START_MOVE:
                            this.Handle_StartMove(action);
                            break;
                        case FightActionEnum.END_MOVE:
                            this.Handle_EndMove(action);
                            break;
                        case FightActionEnum.ROTATE:
                            this.Handle_Rotate(action);
                            break;
                        case FightActionEnum.SKILL:
                            this.Handle_Skill(action);
                            break;
                        default:
                            break;
                    }
                }

                //加入房间协议
                private void Handle_AddRoom(TcpClient client, Protocol proto)
                {
                    AddRoomReq action = (AddRoomReq)proto; 

                    var roomId = action.roomId;
                    var playerId = action.playerId;
                    var p = CharacterManager.GetInstance().GetPlayer(playerId);
                    if (p == null) return; //错误代码
                    if (roomId != 0)
                    {
                        var room = RoomManager.GetInstance().GetRoom(roomId);
                        if (room != null)
                        {
                            p.data.roomId = roomId;
                        }
                    }
                    else
                    {
                        Room r = RoomManager.GetInstance().CreateRoom();
                        p.data.roomId = r.roomId;
                    }
                    //协议返回
                    var players = RoomManager.GetInstance().GetRoomPlayerIds(p.data.roomId);

                    var AddRoomRep = new AddRoomRep(1, p.data.roomId, players.ToArray());
                    //NetManager.GetInstance().SendProtocol(client, AddRoomRep);

                    var PlayerAddNtf = new PlayerAddNtf(p.data.roomId, players.ToArray());
                    //NetManager.GetInstance().SendProtoToRoom(p.data.roomId, PlayerAddNtf);

                    this.SyncGameData(playerId);
                }

                private void Handle_StartMove(PlayerOptReq action)
                {
                    var playerId = action.playerId;
                    var p = CharacterManager.GetInstance().GetPlayer(playerId);
                    if (p == null) return;
                    this.SetPlayerState(playerId, StateEnum.MOVE);
                    this.DispatcherAction(action);
                }
                private void Handle_EndMove(PlayerOptReq action)
                {
                    var playerId = action.playerId;
                    var p = CharacterManager.GetInstance().GetPlayer(playerId);
                    if (p == null) return;
                    this.SetPlayerState(playerId, StateEnum.IDLE);
                    this.DispatcherAction(action);
                }

                private void Handle_Rotate(PlayerOptReq action)
                {
                    var playerId = action.playerId;
                    var rot = action.rot;
                    var p = CharacterManager.GetInstance().GetPlayer(playerId);
                    if (p == null) return;
                    p.Rotate(rot);
                    this.DispatcherAction(action);
                }

                private void Handle_Skill(PlayerOptReq action)
                {
                    var playerId = action.playerId;
                    var skillId = action.skillId;
                    SkillManager.GetInstance().AddSkill(new Skill(playerId, skillId));
                    CommonUtils.Logout("玩家[" + playerId + "]施放技能 " + skillId);
                    this.DispatcherAction(action);
                }

                private void SetPlayerState(int playerid, StateEnum state)
                {
                    var p = CharacterManager.GetInstance().GetPlayer(playerid);
                    if (p != null)
                    {
                        p.data.state = state;
                        CommonUtils.Logout("玩家[" + p.data.playerId + "]状态修改为 " + state);
                        SyncGameData(playerid);
                    }
                }

                private void SyncGameData(int Playerid)
                {
                    var p = CharacterManager.GetInstance().GetPlayer(Playerid);
                    var players = CharacterManager.GetInstance().GetAOIPlayer(Playerid);
                    if (players == null) return;
                    List<PlayerGameData> playerDatas = new List<PlayerGameData>();
                    foreach (var item in players)
                    {
                        playerDatas.Add(item.data);
                    }
                    var bullets = BulletManager.GetInstance().GetAllBullets();
                    List<BulletData> bulletDatas = new List<BulletData>();
                    foreach (var item in bullets)
                    {
                        bulletDatas.Add(item.data);
                    }
                    var GameDataNtf = new GameDataNtf(p.data.roomId, playerDatas, bulletDatas);
                    CommonUtils.Logout("数据同步至玩家[" + p.data.playerId + "]");
                    //NetManager.GetInstance().SendProtoToRoom(p.data.roomId, GameDataNtf);
                }

                //派发事件
                private void DispatcherAction(PlayerOptReq action)
                {
                    var players = CharacterManager.GetInstance().GetAOIPlayer(action.playerId);
                    if (players == null) return;
                    foreach (var player in players)
                    {
                        CommonUtils.Logout("操作转发至玩家[" + player.data.playerId + "]");
                        //发送数据同步协议
                        //NetManager.GetInstance().SendProtocol(Global.PlayerSocketMap[player.data.playerId], action);
                    }
                }*/
    }
}
