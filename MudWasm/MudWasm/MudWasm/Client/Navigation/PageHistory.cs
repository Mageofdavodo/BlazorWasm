namespace MudWasm.Client.Navigation
{
    public class PageHistory
    {
        public List<PageState> PreviousPages { get; set; } = new();

        public void UpdatePages(PageState pageState)
        {
            if (PreviousPages.Contains(pageState))
            {
                ResetAtIndex(PreviousPages.IndexOf(pageState));
                PreviousPages.Add(pageState);
            }
            else
            {
                PreviousPages.Add(pageState);
            }
        }

        private void ResetAtIndex(int index)
        {
            var amountToRemove = PreviousPages.Count - index;
            PreviousPages.RemoveRange(index, amountToRemove);
        }
    }
    public record PageState (string Name, string Url);

}
