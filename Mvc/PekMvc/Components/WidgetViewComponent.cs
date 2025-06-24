using Microsoft.AspNetCore.Mvc;

using NewLife.Model;

using Pek.Events;
using Pek.Infrastructure;
using Pek.NCubeUI.Components;
using Pek.NCubeUI.Events;

namespace PekMvc.Components;

public partial class WidgetViewComponent : PekViewComponent {
    public async Task<IViewComponentResult> InvokeAsync(String WidgetZone, object? AdditionalData = null)
    {
        var eventPublisher = ObjectContainer.Provider.GetPekService<IEventPublisher>();

        var widgetEvent = new WidgetEvent(WidgetZone);
        await eventPublisher?.PublishAsync(widgetEvent)!;

        if (widgetEvent.Data == null || !widgetEvent.Data.Any()) return Content("");

        return View(widgetEvent.Data);
    }
}
