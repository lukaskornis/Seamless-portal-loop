using UnityEngine.Events;

public class ChangeEvent<T>
{
    T value;
    UnityEvent<T> onChange = new UnityEvent<T>();

    public void Invoke(T value)
    {
        if (this.value.Equals(value)) return;
        this.value = value;
        onChange.Invoke(value);
    }

    public void AddListener(UnityAction<T> action)
    {
        onChange.AddListener(action);
    }

    public void RemoveListener(UnityAction<T> action)
    {
        onChange.RemoveListener(action);
    }
}

public static class EventExtensions
{
}