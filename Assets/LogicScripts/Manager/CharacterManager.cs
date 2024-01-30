using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Manager<CharacterManager>
{
    /// <summary>
    /// 角色 死亡
    /// </summary>
    public static string CHARACTER_DIE { get { return GetInstance().GetType().Name + "CHARACTER_DIE"; } }

    public List<Character> characters = new List<Character>();


    public override void AddEventListener()
    {
        base.AddEventListener();
        Eventer.On(CHARACTER_DIE, OnCharacterDie);
    }


    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        Eventer.Off(CHARACTER_DIE, OnCharacterDie);
    }
    public void OnCharacterDie(object[] args)
    {
        Character crt = args[0] as Character;
        if (crt != null)
        {
            crt.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, crt, StateType.Die);
        }
    }

    public Character CreateCharacter(CharacterVO vo, CharacterBaseInfo info = null)
    {
        GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
        Character c = new Character(vo, characterObj, info);
        characters.Add(c);
        return c;
    }

    public Character CreateCharacter(int id)
    {
        var vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        if (vo != null)
        {
            return CreateCharacter(vo);
        }
        return null;
    }

    public Character CreateFightCharacter(int id)
    {
        var vo = CharacterConfiger.GetInstance().GetCharacterById(id);
        if (vo != null)
        {
            GameObject characterObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
            Character c = new Character(vo, characterObj, CharacterBaseInfo.GetFightBaseInfo());
            characters.Add(c);
            return c;
        }
        return null;
    }

    public Character CreateTestCharacter()
    {
        return CreateFightCharacter(98);

    }



    public void RemoveCharacter(Character c)
    {
        c.OnDestory();
        characters.Remove(c);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        foreach (var item in characters)
        {
            item.OnUpdate();
        }
    }



}