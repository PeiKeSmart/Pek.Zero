using NewLife.Log;

using Pek;
using Pek.Events;
using Pek.NCube.Events;
using Pek.NCubeUI.Events;
using Pek.NCubeUI.Models;
using Pek.NCubeUI.UI;

using PekMvc.Common;
using PekMvc.Components;

namespace PekMvc.Events;

public class EventConsumer :
    IConsumer<ModelPreparedEvent<BaseDHModel>>,
    IConsumer<ModelPreparedEvent<IEnumerable<BaseDHModel>>>,
    IConsumer<ModelPreparedEvent<BasePekModel>>,
    IConsumer<ModelPreparedEvent<IEnumerable<BasePekModel>>>,
    IConsumer<ModelReceivedEvent<BaseDHModel>>,
    IConsumer<ModelReceivedEvent<IEnumerable<BaseDHModel>>>,
    IConsumer<ModelReceivedEvent<BasePekModel>>,
    IConsumer<ModelReceivedEvent<IEnumerable<BasePekModel>>>,
    IConsumer<GenericRoutingEvent>,
    IConsumer<PageRenderingEvent>,
    IConsumer<WidgetEvent> {

    public int Sort { get; set; } = 0;

    public async Task HandleEventAsync(ModelPreparedEvent<BaseDHModel> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelPreparedEvent<BaseDHModel>");
    }

    public async Task HandleEventAsync(ModelPreparedEvent<IEnumerable<BaseDHModel>> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelPreparedEvent<IEnumerable<BaseDHModel>>");
    }

    public async Task HandleEventAsync(ModelPreparedEvent<BasePekModel> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelPreparedEvent<BasePekModel>");
    }

    public async Task HandleEventAsync(ModelPreparedEvent<IEnumerable<BasePekModel>> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelPreparedEvent<IEnumerable<BasePekModel>>");
    }

    public async Task HandleEventAsync(GenericRoutingEvent eventMessage)
    {
        XTrace.WriteLine($"进来GenericRoutingConsumer");

        //eventMessage.Handled = true;

        await Task.CompletedTask;
    }

    /// <summary>
    /// 处理页面渲染事件
    /// </summary>
    /// <param name="eventMessage">事件消息</param>
    /// <returns>一个代表异步操作的任务</returns>
    public async Task HandleEventAsync(PageRenderingEvent eventMessage)
    {
        var routeName = eventMessage.GetRouteName(true) ?? string.Empty;
        XTrace.WriteLine($"进来了么？PageRenderingConsumer:{routeName}");

        eventMessage.Helper.AddScriptParts(ResourceLocation.Footer, "/index.js", excludeFromBundle: true);
    }

    public async Task HandleEventAsync(WidgetEvent eventMessage)
    {
        XTrace.WriteLine($"进来了么？WidgetEvent:{eventMessage.WidgetZone}");

        var list = eventMessage.Data;
        list ??= [];

        if (eventMessage.WidgetZone == PublicWidgetZones.HeadHtmlTag)
        {
            list.Add(new RenderWidgetModel()
            {
                WidgetViewComponent = typeof(HeadHtmlTagViewComponent),
            });
        }

        eventMessage.Data = list;
    }

    public async Task HandleEventAsync(ModelReceivedEvent<BaseDHModel> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelReceivedEvent<BaseDHModel>");
    }

    public async Task HandleEventAsync(ModelReceivedEvent<BasePekModel> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelReceivedEvent<BasePekModel>");
    }

    public async Task HandleEventAsync(ModelReceivedEvent<IEnumerable<BaseDHModel>> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelReceivedEvent<IEnumerable<BaseDHModel>>");
    }

    public async Task HandleEventAsync(ModelReceivedEvent<IEnumerable<BasePekModel>> eventMessage)
    {
        XTrace.WriteLine($"进来这里了么？ModelReceivedEvent<IEnumerable<BaseDHModel>>");
    }
}
