using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    OLD,
    NEW,
    DEBUG,
    OFFICIAL
}

public class GameContext
{
    public static GameMode GameMode = GameMode.NEW;

    public static bool OpenRangeShow = true;

    public static int SelectCrtId = 1;

    private static Character _curRole;
    //当前操纵的角色
    public static Character CurRole 
    {
        get { return _curRole; }
        set
        {
            _curRole = value;
            EventDispatcher.GetInstance().Event(EventDispatcher.MAIN_ROLE_CHANGE, value);
        }
    }

    //场景中全部角色
    public static List<Character> AllCharacter = new List<Character>();

    private static GameScene _curScene;
    public static GameScene LastScene;
    public static GameScene CurScene
    {
        get { return _curScene; }
        set
        {
            if(LastScene == null)
            {
                LastScene = value;
            }
            else
            {
                LastScene = _curScene;
            }
            _curScene = value;
            EventDispatcher.GetInstance().Event(EventDispatcher.SCENE_CHANGE, value);
        }
    }


    public static Character GetCharacterByObj(GameObject obj)
    {
        for (int i = 0; i < AllCharacter.Count; i++)
        {
            var character = AllCharacter[i];
            if (character.trans != null && character.trans.gameObject == obj)
            {
                return character;
            }
        }
        return null;
    }

    public static Vector3 GetPlayerInputDirect()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        return new Vector3(x, 0, z).normalized;
    }

    public static Vector3 GetDirByInput(Character character, bool defaultIsForward = true)
    {
        Vector3 input = GetPlayerInputDirect();
        Transform trans = character.trans;
        Vector3 dir = defaultIsForward ? trans.forward.normalized : Vector3.zero;
        if (input.x > 0 && input.z < 0)
        {
            dir = (-trans.forward + trans.right).normalized;
        }
        else if (input.x > 0 && input.z > 0)
        {
            dir = (trans.forward + trans.right).normalized;

        }
        else if (input.x < 0 && input.z > 0)
        {
            dir = (trans.forward - trans.right).normalized;

        }
        else if (input.x < 0 && input.z < 0)
        {
            dir = (-trans.forward - trans.right).normalized;

        }
        else if (input.x == 0 && input.z < 0)
        {
            dir = -trans.forward.normalized;

        }
        else if (input.x == 0 && input.z > 0)
        {
            dir = trans.forward.normalized;

        }
        else if (input.x > 0 && input.z == 0)
        {
            dir = trans.right.normalized;

        }
        else if (input.x < 0 && input.z == 0)
        {
            dir = -trans.right.normalized;
        }
        return dir;
    }

    /// <summary>
    /// 角色是否包含状态
    /// </summary>
    /// <param name="character"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool CharacterIncludeState(string type, bool defalutCharacterIsSelf = true, Character character = null)
    {
        if (defalutCharacterIsSelf)
        {
            character = GameContext.CurRole;
        }

        if (character == null || character.fsm == null || character.fsm.myState == null)
        {
            return false;
        }

        return character.fsm.myState.IncludeState(type);
    }


    public static int GetCharacterSkillIdByIndex(int index, Character character = null)
    {
        if (character == null) character = GameContext.CurRole;
        return character.characterData.GetSkillArr()[index];
    }

    public static AudioSource GetLoopAudio()
    {
       return  MonoBridge.GetInstance().transform.Find("LoopAudio").GetComponent<AudioSource>();
    }

    public static AudioSource GetOnceAudio()
    {
        return MonoBridge.GetInstance().transform.Find("OnceAudio").GetComponent<AudioSource>();
    }

    public static GameObject GetAudiosMain()
    {
        return MonoBridge.GetInstance().transform.Find("AudiosMain").gameObject;
    }
}