using Microsoft.AspNetCore.Mvc;

using Pek.NCubeUI.Components;

using PekMvc.Common;

namespace PekMvc.Components;

public partial class BlogRssHeaderLinkViewComponent : PekViewComponent {
    public IViewComponentResult Invoke()
    {
        if (!PekSettings.Current.ShowHeaderRssUrl)
            return Content("");

        return View();
    }
}
