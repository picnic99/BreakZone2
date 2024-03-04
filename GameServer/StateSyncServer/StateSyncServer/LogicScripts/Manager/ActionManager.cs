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
            ActionManager.GetInstance().On(ProtocolId.CLIENT_SELECT_ROLE_REQ, SelectCrtHandle);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_ENTER_SCENE_REQ, EnterSceneHandle);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ, HandlePlayerGameOpt);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_GAME_PLAYER_INPUT_REQ, HandlePlayerInput);
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_PLAYER_LOGIN_REQ, LoginReqHandle);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_SELECT_ROLE_REQ, SelectCrtHandle);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_ENTER_SCENE_REQ, EnterSceneHandle);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ, HandlePlayerGameOpt);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_GAME_PLAYER_INPUT_REQ, HandlePlayerInput);
        }

        public void SelectCrtHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            SelectCrtReq req = proto.GetDataInstance<SelectCrtReq>();

            var crtId = req.CrtId;
            if (crtId > 0)
            {
                CommonUtils.Logout("选择角色" + crtId);
                //创建角色实体 Character
                //协议返回
                SelectCrtRep rep = new SelectCrtRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.CrtId = crtId;
                NetManager.GetInstance().SendProtocol(proto.client, rep);
            }
        }

        public void EnterSceneHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            EnterSceneReq req = proto.GetDataInstance<EnterSceneReq>();

            var SceneId = req.SceneId;
            if (SceneId > 0)
            {
                CommonUtils.Logout("进入场景" + SceneId);
                var p = Global.GetPlayerByClient(proto.client);
                if (p == null) return;
                p.lastStaySceneId = SceneId;
                //协议返回
                EnterSceneRep rep = new EnterSceneRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.SceneId = SceneId;
                NetManager.GetInstance().SendProtocol(proto.client, rep);
            }
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
            var playerId = req.PlayerId;
            var type = req.Type;


            if (type == LoginTypeEnum.LOGIN_IN)
            {
                //登录
                CommonUtils.Logout($"玩家{username}登录");
                var p = PlayerManager.GetInstance().GetPlayer(username, password);
                Global.AddPlayerClientMap(p.playerId, proto.client);
                //协议返回
                PlayerLoginRep rep = new PlayerLoginRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.Type = type;
                NetManager.GetInstance().SendProtocol(proto.client, rep);
                //同步一下玩家信息
                PlayerBaseInfoNtf ntf = new PlayerBaseInfoNtf();
                ntf.PlayerId = p.playerId;
                ntf.Username = username;
                //以下信息从数据库查询
                ntf.LastStaySceneId = 1;
                ntf.LastStayPosX = 0;
                ntf.LastStayPosY = 0;
                ntf.LastStayPosZ = 0;
                NetManager.GetInstance().SendProtocol(proto.client, ntf);
            }
            if (type == LoginTypeEnum.LOGIN_Out)
            {
                //注销
                if (playerId <= 0) return;
                var p = PlayerManager.GetInstance().FindPlayer(playerId);
                if (p == null) return;
                Global.RemovePlayerClientMap(p.playerId);

                //协议返回
                PlayerLoginRep rep = new PlayerLoginRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.Type = type;
                CommonUtils.Logout($"玩家{username}登出");
                NetManager.GetInstance().SendProtocol(proto.client, rep);

            }
        }

        public void HandlePlayerInput(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerInputReq req = proto.GetDataInstance<GamePlayerInputReq>();
            var InputX = req.InputX;
            var InputY = req.InputY;

            int pId = Global.GetPlayerIdByClient(proto.client);
            if (pId == 0) return;
            GamePlayerInputNtf ntf = new GamePlayerInputNtf();
            ntf.InputX = InputX;
            ntf.InputY = InputY;
            ntf.PlayerId = pId;
            NetManager.GetInstance().SendProtocol(proto.client, ntf);
        }

        public void HandlePlayerGameOpt(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            GamePlayerOptReq req = proto.GetDataInstance<GamePlayerOptReq>();

            var UpMove = req.UpMove;
            var DownMove = req.DownMove;
            var LeftMove = req.LeftMove;
            var RightMove = req.RightMove;
            var AddSpeed = req.AddSpeed;
            var Arm = req.Arm;
            var Flash = req.Flash;
            var Jump = req.Jump;
            var Atk = req.Atk;
            var Skill1 = req.Skill1;
            var Skill2 = req.Skill2;
            var Skill3 = req.Skill3;
            var Skill4 = req.Skill4;

            GamePlayerOptRep rep = new GamePlayerOptRep();
            rep.Result = ProtocolResultEnum.SUCCESS;
            NetManager.GetInstance().SendProtocol(proto.client, rep);

            if (PlayerOptEnum.IsKeyDown(UpMove)
                || PlayerOptEnum.IsKeyDown(DownMove)
                || PlayerOptEnum.IsKeyDown(LeftMove)
                || PlayerOptEnum.IsKeyDown(RightMove)
                )
            {
                //玩家移动 播放移动动画
                int pId = Global.GetPlayerIdByClient(proto.client);
                if (pId == 0) return;
                GameAnimPlayNtf NTF = new GameAnimPlayNtf();
                NTF.AnimName = "DEFAULT_MOVE";
                NTF.PlayerId = pId;
                NTF.TranslateTime = 0.15f;
                NetManager.GetInstance().SendProtocol(proto.client, NTF);
            }
        }

        public void HandleGetGameDataReq(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GetGameDataRep rep = new GetGameDataRep();
            rep.Result = ProtocolResultEnum.SUCCESS;
            NetManager.GetInstance().SendProtocol(proto.client, rep);
            var p = Global.GetPlayerByClient(proto.client);
            if (p!=null)
            {
                SyncGameDataToScene(p.lastStaySceneId,p.playerId);
            }
        }

        /// <summary>
        /// 同步游戏数据
        /// </summary>
        public SyncGameDataNtf GetSyncGameData(int sceneId)
        {
            //收集场景中需要同步的数据

            var ntf = new SyncGameDataNtf();
            //填充数据
            return ntf;
        }

        public void SyncGameDataToScene(int sceneId, int playerId)
        {
            SyncGameDataNtf ntf = GetSyncGameData(sceneId);
            var client = Global.GetClientByPlayerId(playerId);
            NetManager.GetInstance().SendProtocol(client, ntf);
        }

        /// <summary>
        /// 同步游戏数据至场景中
        /// </summary>
        /// <param name="sceneId">场景</param>
        /// <param name="playerIds">需要同步的玩家</param>
        public void SyncGameDataToScene(int sceneId, List<int> playerIds)
        {
            SyncGameDataNtf ntf = GetSyncGameData(sceneId);
            foreach (var item in playerIds)
            {

                var client = Global.GetClientByPlayerId(item);
                if (client != null)
                {
                    NetManager.GetInstance().SendProtocol(client, ntf);
                }
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
