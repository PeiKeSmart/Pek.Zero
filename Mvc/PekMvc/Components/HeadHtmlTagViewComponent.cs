using Microsoft.AspNetCore.Mvc;

using Pek.NCubeUI.Components;

namespace PekMvc.Components;

public class HeadHtmlTagViewComponent : PekViewComponent {
    public async Task<IViewComponentResult> InvokeAsync(String WidgetZone, object? AdditionalData = null)
    {
        await Task.CompletedTask;

        return View();
    }
}
