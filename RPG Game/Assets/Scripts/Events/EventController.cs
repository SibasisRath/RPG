using System;
namespace RPG.Events
{
    public class EventController
    {
        public Action baseEvent;
        public void AddListener(Action listener) => baseEvent += listener;
        public void RemoveListener(Action listener) => baseEvent -= listener;
        public void InvokeEvent() => baseEvent?.Invoke();
    }

    public class EventController<T>
    {
        public Action<T> baseEvent;
        public void AddListener(Action<T> listener) => baseEvent += listener;
        public void RemoveListener(Action<T> listener) => baseEvent -= listener;
        public void InvokeEvent(T type) => baseEvent?.Invoke(type);
    }

}
