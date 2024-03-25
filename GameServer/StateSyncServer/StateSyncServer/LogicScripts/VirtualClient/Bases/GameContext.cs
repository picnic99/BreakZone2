using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.Bridge;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VO;
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Bases
{
    public enum GameMode
    {
        OLD,
        NEW,
        DEBUG,
        OFFICIAL
    }

    public class GameContext
    {
        public static GameMode GameMode = GameMode.DEBUG;

        public static Vector3 GetPlayerInputDirect(Player player)
        {
            return new Vector3(player.Input.X, 0, player.Input.Y);
        }

        public static Vector3 GetDirByInput(Character character, bool defaultIsForward = true)
        {
            Vector3 input = GetPlayerInputDirect(character.Player);
            Transform trans = character.Trans;
            Vector3 dir = defaultIsForward ? trans.Forward : Vector3.Zero;
            if (input.X > 0 && input.Z < 0)
            {
                dir = (-trans.Forward + trans.Right);
            }
            else if (input.X > 0 && input.Z > 0)
            {
                dir = (trans.Forward + trans.Right);

            }
            else if (input.X < 0 && input.Z > 0)
            {
                dir = (trans.Forward - trans.Right);

            }
            else if (input.X < 0 && input.Z < 0)
            {
                dir = (-trans.Forward - trans.Right);

            }
            else if (input.X == 0 && input.Z < 0)
            {
                dir = -trans.Forward;

            }
            else if (input.X == 0 && input.Z > 0)
            {
                dir = trans.Forward;

            }
            else if (input.X > 0 && input.Z == 0)
            {
                dir = trans.Right;

            }
            else if (input.X < 0 && input.Z == 0)
            {
                dir = -trans.Right;
            }
            return dir;
        }

        /// <summary>
        /// 角色是否包含状态
        /// </summary>
        /// <param name="character"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CharacterIncludeState(string type, Character character)
        {
            if (character == null || character.fsm == null || character.fsm.myState == null)
            {
                return false;
            }

            return character.fsm.myState.IncludeState(type);
        }


        public static int GetCharacterSkillIdByIndex(int index, Character character)
        {
            if (character == null) return 0;
            return character.characterData.GetSkillArr()[index];
        }

        public static Vector3 GetInputDir(Character crt, bool defaultIsForward = true)
        {
            Vector3 input = new Vector3(crt.Player.Input.X, 0, crt.Player.Input.Y);
            Transform trans = crt.Trans;
            Vector3 dir = defaultIsForward ? Vector3.Normalize(trans.Forward): Vector3.Zero;
            if (input.X > 0 && input.Z < 0)
            {
                dir = (-trans.Forward + trans.Right).Normalize();
            }
            else if (input.X > 0 && input.Z > 0)
            {
                dir = (trans.Forward + trans.Right).Normalize();

            }
            else if (input.X < 0 && input.Z > 0)
            {
                dir = (trans.Forward - trans.Right).Normalize();

            }
            else if (input.X < 0 && input.Z < 0)
            {
                dir = (-trans.Forward - trans.Right).Normalize();

            }
            else if (input.X == 0 && input.Z < 0)
            {
                dir = -trans.Forward.Normalize();

            }
            else if (input.X == 0 && input.Z > 0)
            {
                dir = trans.Forward.Normalize();

            }
            else if (input.X > 0 && input.Z == 0)
            {
                dir = trans.Right.Normalize();

            }
            else if (input.X < 0 && input.Z == 0)
            {
                dir = -trans.Right.Normalize();
            }
            return dir;
        }
    }
}