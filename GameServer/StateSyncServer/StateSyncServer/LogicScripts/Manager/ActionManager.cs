
using StateSyncServer.LogicScripts.VO;
using System.Collections.Generic;
using System.Net.Sockets;
using StateSyncServer.LogicScripts.Common;
using StateSyncServer.LogicScripts.Util;
using Msg;
using StateSyncServer.LogicScripts.Net.PB;
using StateSyncServer.LogicScripts.Net.PB.Enum;
using System.Numerics;

namespace StateSyncServer.LogicScripts.Manager
{
    class ActionManager : Manager<ActionManager>
    {
        public override void AddEventListener()
        {
            base.AddEventListener();
            ActionManager.GetInstance().On(ProtocolId.CLIENT_PLAYER_LOGIN_REQ, LoginReqHandle);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_SELECT_ROLE_REQ, SelectCrtHandle);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_ENTER_SCENE_REQ, EnterSceneHandle);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ, HandlePlayerGameOpt);
            ActionManager.GetInstance().On(ProtocolId.CLIENT_GAME_PLAYER_INPUT_REQ, HandlePlayerInput);
            ActionManager.GetInstance().On(ProtocolId.CLENT_GET_GAME_DATA_REQ, HandleGetGameDataReq);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_PLAYER_LOGIN_REQ, LoginReqHandle);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_SELECT_ROLE_REQ, SelectCrtHandle);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_ENTER_SCENE_REQ, EnterSceneHandle);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_GAME_PLAYER_OPT_REQ, HandlePlayerGameOpt);
            ActionManager.GetInstance().Off(ProtocolId.CLIENT_GAME_PLAYER_INPUT_REQ, HandlePlayerInput);
            ActionManager.GetInstance().Off(ProtocolId.CLENT_GET_GAME_DATA_REQ, HandleGetGameDataReq);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <param name="args"></param>
        public void SelectCrtHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            SelectCrtReq req = proto.GetDataInstance<SelectCrtReq>();

            var crtId = req.CrtId;
            if (crtId > 0)
            {
                CommonUtils.Logout("选择角色" + crtId);
                Player p = proto.GetPlayer();
                if (p != null)
                {
                    p.lastSelectCrtId = crtId;
                }
                //协议返回
                SelectCrtRep rep = new SelectCrtRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.CrtId = crtId;
                NetManager.GetInstance().SendProtocol(proto.client, rep);
            }
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="args"></param>
        public void EnterSceneHandle(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            EnterSceneReq req = proto.GetDataInstance<EnterSceneReq>();

            var SceneId = req.SceneId;
            if (SceneId > 0)
            {
                CommonUtils.Logout("进入场景" + SceneId);
                SceneManager.GetInstance().AddPlayerToScene(proto.GetPlayerId(), SceneId);
                //协议返回
                EnterSceneRep rep = new EnterSceneRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.SceneId = SceneId;
                NetManager.GetInstance().SendProtocol(proto.client, rep);
            }
        }


        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="args"></param>
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
                ntf.LastStayPos.X = 0;
                ntf.LastStayPos.Y = 0;
                ntf.LastStayPos.Z = 0;
                NetManager.GetInstance().SendProtocol(proto.client, ntf);
            }
            if (type == LoginTypeEnum.LOGIN_Out)
            {
                //注销
                if (playerId <= 0) return;
                var p = PlayerManager.GetInstance().FindPlayer(playerId);
                if (p == null) return;
                Global.RemovePlayerClientMap(p.playerId);
                PlayerManager.GetInstance().RemovePlayer(playerId);
                //协议返回
                PlayerLoginRep rep = new PlayerLoginRep();
                rep.Result = ProtocolResultEnum.SUCCESS;
                rep.Type = type;
                CommonUtils.Logout($"玩家{username}登出");
                NetManager.GetInstance().SendProtocol(proto.client, rep);

            }
        }

        /// <summary>
        /// 处理玩家输入操作
        /// </summary>
        /// <param name="args"></param>
        public void HandlePlayerInput(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GamePlayerInputReq req = proto.GetDataInstance<GamePlayerInputReq>();
            var InputX = req.InputX;
            var InputY = req.InputY;

            Player player = proto.GetPlayer();
            if (player == null) return;
            player.SetInput(new System.Numerics.Vector2(InputX, InputY));
            //协议返回
            GamePlayerInputNtf ntf = new GamePlayerInputNtf();
            ntf.InputX = InputX;
            ntf.InputY = InputY;
            ntf.PlayerId = player.playerId;
            List<Player> players = SceneManager.GetInstance().GetPlayersInScene(player.lastStaySceneId);
            if (players.Count > 0)
            {
                NetManager.GetInstance().SendProtoToScene(players, ntf);
            }
        }

        /// <summary>
        /// 处理玩家的操作
        /// </summary>
        /// <param name="args"></param>
        public void HandlePlayerGameOpt(object[] args)
        {
            Protocol proto = args[0] as Protocol;

            GamePlayerOptReq req = proto.GetDataInstance<GamePlayerOptReq>();

            Player player = proto.GetPlayer();
            if (player != null)
            {
                player.ApplyPlayerOpt(req);
            }

            //协议返回
            GamePlayerOptRep rep = new GamePlayerOptRep();
            rep.Result = ProtocolResultEnum.SUCCESS;
            NetManager.GetInstance().SendProtocol(proto.client, rep);

            /*            GameAnimPlayNtf NTF = new GameAnimPlayNtf();
                        NTF.AnimName = "DEFAULT_MOVE";
                        NTF.PlayerId = pId;
                        NTF.TranslateTime = 0.15f;
                        NetManager.GetInstance().SendProtocol(proto.client, NTF);*/
        }

        /// <summary>
        /// 同步数据申请
        /// </summary>
        /// <param name="args"></param>
        public void HandleGetGameDataReq(object[] args)
        {
            Protocol proto = args[0] as Protocol;
            GetGameDataRep rep = new GetGameDataRep();
            rep.Result = ProtocolResultEnum.SUCCESS;
            NetManager.GetInstance().SendProtocol(proto.client, rep);
            var p = proto.GetPlayer();
            if (p != null)
            {
                SyncGameDataToScene(p.lastStaySceneId);
            }
        }

        /// <summary>
        /// 同步游戏数据
        /// TODO 需要考虑数据量的情况
        /// 除了第一次进入场景中 需要同步场景的所有信息
        /// 其余时刻应当是谁触发了状态改变就同步谁的数据给其它玩家
        /// </summary>
        public SyncGameDataNtf GetSyncGameData(int sceneId)
        {
            //收集场景中需要同步的数据
            //读取当前场景中存在的玩家 包括AI 可能还需要考虑物体
            var ntf = new SyncGameDataNtf();
            CrtGameData crtData = new CrtGameData();
            crtData.PlayerId = 1;
            crtData.Pos.X = 0;
            crtData.Pos.Y = 0;
            crtData.Pos.Z = 0;
            ntf.CrtData.Add(crtData);
            //填充数据
            return ntf;
        }

        public void SyncGameDataToScene(int sceneId)
        {
            SyncGameDataNtf ntf = GetSyncGameData(sceneId);
            List<Player> players = SceneManager.GetInstance().GetPlayersInScene(sceneId);
            if (players.Count > 0)
            {
                NetManager.GetInstance().SendProtoToScene(players, ntf);
            }
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

        public void SendAnimPlayNtf(int playerId,int instanceId,string animName,float translateTime)
        {
            GameAnimPlayNtf ntf = new GameAnimPlayNtf();
            ntf.InstanceId = instanceId;
            ntf.AnimName = animName;
            ntf.TranslateTime = translateTime;

            List<Player> players = SceneManager.GetInstance().GetPlayerInSceneByPid(playerId);
            if (players == null || players.Count == 0) return;
            NetManager.GetInstance().SendProtoToScene(players, ntf);
        }

        public void SendAudioPlayNtf(int playerId,int instanceId,int audioId,int randomIndex)
        {
            GameAudioPlayNtf ntf = new GameAudioPlayNtf();
            ntf.InstanceId = instanceId;
            ntf.AudioId = audioId;
            ntf.RandomIndex = randomIndex;

            List<Player> players = SceneManager.GetInstance().GetPlayerInSceneByPid(playerId);
            if (players == null || players.Count == 0) return;
            NetManager.GetInstance().SendProtoToScene(players, ntf);
        }

        public void SendGameInstanceCreateNtf(int playerId, int instanceId, int prefabId, Vector3 initPos, Vector3 initScale, float initRot)
        {
            GameInstanceCreateNtf NTF = new GameInstanceCreateNtf();
            NTF.InstanceId = instanceId;
            NTF.PrefabId = prefabId;
            NTF.InitPos = new Position();
            NTF.InitPos.X = initPos.X;
            NTF.InitPos.Y = initPos.Y;
            NTF.InitPos.Z = initPos.Z;
            NTF.InitScale = new Position();
            NTF.InitScale.X = initScale.X;
            NTF.InitScale.Y = initScale.Y;
            NTF.InitScale.Z = initScale.Z;
            NTF.InitRot = initRot;
            List<Player> players = SceneManager.GetInstance().GetPlayerInSceneByPid(playerId);
            if (players == null || players.Count == 0) return;
            NetManager.GetInstance().SendProtoToScene(players, NTF);
        }

        public void SendGameInstanceTransformNtf(int playerId, int instanceId, int trans, int transType, Vector3 target, float rotTarget, float durationTime)
        {
            GameInstanceTransformNtf ntf = new GameInstanceTransformNtf();
            ntf.InstanceId = instanceId;
            ntf.Trans = trans;
            ntf.TransType = transType;
            ntf.Target = new Position();
            ntf.Target.X = target.X;
            ntf.Target.Y = target.Y;
            ntf.Target.Z = target.Z;
            ntf.RotTarget = rotTarget;
            ntf.DurationTime = durationTime;

            List<Player> players = SceneManager.GetInstance().GetPlayerInSceneByPid(playerId);
            if (players == null || players.Count == 0) return;
            NetManager.GetInstance().SendProtoToScene(players, ntf);
        }

        public void SendGameInstanceDestroyNtf(int playerId, int instanceId, float delay = 0)
        {
            GameInstanceDestroyNtf NTF = new GameInstanceDestroyNtf();
            NTF.InstanceId = instanceId;
            NTF.Delay = delay;
            List<Player> players = SceneManager.GetInstance().GetPlayerInSceneByPid(playerId);
            if (players == null || players.Count == 0) return;
            NetManager.GetInstance().SendProtoToScene(players, NTF);
        }
    }
}
