namespace MudWasm.Client.Pages.Counter
{
    public class CounterStateContainer
    {
        private int _count;
        public int Count
        {
            get => _count; 
            set
            {
                _count = value;
                NotifyStateChanged();
            }
        }
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
