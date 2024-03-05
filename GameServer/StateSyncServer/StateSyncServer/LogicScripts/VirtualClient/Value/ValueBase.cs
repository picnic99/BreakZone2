public abstract class ValueBase<T>
{
    public T baseValue;
    public T finalValue;
    public ModifierGroup<T> modGroups = new ModifierGroup<T>();


    public ValueModifier<T> AddModifier(T value)
    {
        ValueModifier<T> mod = new ValueModifier<T>(value);
        modGroups.AddModifier(mod);
        UpdateValue();
        return mod;
    }

    public void RemoveModifier(ValueModifier<T> mod)
    {
        modGroups.RemoveModifier(mod);
        UpdateValue();
    }

    public void ModModifier(ValueModifier<T> mod, T value)
    {
        if (mod != null)
        {
            mod.value = value;
            UpdateValue();
        }
    }


    public void UpdateValue()
    {
        finalValue = baseValue;
        finalValue = GetGroupTotalValue(modGroups);
    }

    public abstract T GetGroupTotalValue(ModifierGroup<T> groups);
}