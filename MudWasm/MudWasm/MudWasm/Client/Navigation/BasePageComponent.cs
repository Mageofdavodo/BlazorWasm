using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudWasm.Client.Navigation
{
    [Authorize]
    public class BasePageComponent : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public PageHistory PageHistory { get; set; }

        public string Title { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            
            var newUri = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "");
            PageHistory.UpdatePages(new PageState(Title, newUri));
        }

        public List<BreadcrumbItem> GetBreadcrumbItems()
        {
            return PageHistory.PreviousPages.Select((page, index) => new BreadcrumbItem(page.Name, page.Url, index+1 == PageHistory.PreviousPages.Count ? true : false)).ToList();
        }
    }
}
