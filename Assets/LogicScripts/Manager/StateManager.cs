using System;


/// <summary>
/// ×´Ì¬¹ÜÀíÆ÷
/// </summary>
public class StateManager : Manager<StateManager>
{
    internal void AddState(_Character from, _Character[] targets, params object[] args)
    {
        if (targets == null || targets.Length <= 0) return;
        foreach (var target in targets)
        {
            target.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, args);
        }
    }
}