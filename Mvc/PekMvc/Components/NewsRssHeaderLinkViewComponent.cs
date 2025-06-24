using DH.Entity;

using Microsoft.AspNetCore.Mvc;

using Pek;
using Pek.NCubeUI.Components;

namespace PekMvc.Components;

public partial class NewsRssHeaderLinkViewComponent : PekViewComponent {
    public IViewComponentResult Invoke()
    {
        if (!DHSetting.Current.ShowNewsHeaderRssUrl)
            return Content("");

        return View();
    }
}
