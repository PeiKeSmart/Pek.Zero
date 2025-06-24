﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using NewLife.Caching;
using NewLife.Caching.Services;
using NewLife.Cube;
using NewLife.Cube.WebMiddleware;
using NewLife.Log;
using NewLife.Model;

using Pek;
using Pek.Configs;
using Pek.Helpers;
using Pek.Infrastructure;
using Pek.NCubeUI;
using Pek.NCubeUI.Common;

XTrace.UseConsole();

ApplicationHelper.SetEnvironment(args);

var builder = WebApplication.CreateBuilder(args);

// 清除默认日志提供程序并设置最低日志级别
//builder.Logging.ClearProviders();
//builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddXLog(); // 添加自定义日志提供程序

ConfigFileHelper.SetConfig(builder.Configuration); //可在Settings文件夹中放入多个配置文件
builder.Configuration.AddEnvironmentVariables();

builder.Host.UseDefaultServiceProvider(options =>
{
    // 我们不验证范围，因为在应用程序启动和初始配置时，我们需要通过根容器解析某些服务（注册为 “scoped”）
    options.ValidateScopes = false;
    options.ValidateOnBuild = true;
});

var services = builder.Services;

// 引入星尘，设置监控中间件
var star = services.AddStardust(null!);
TracerMiddleware.Tracer = star?.Tracer;
star?.SetWatchdog(120);

// 分布式服务，使用配置中心RedisCache配置
ObjectContainer.Current.AddSingleton<ICacheProvider, RedisCacheProvider>();

// 修改上传的文件大小限制
DHSetting.Current.MaxSize = 200;
DHSetting.Current.CurrentVersion = "1.0.0"; // 设置当前版本
DHSetting.Current.Save();

// 修改上传的文件大小限制
if (!ApplicationHelper.IsIIS)
{
    // 配置上传文件大小限制（详细信息：FormOptions） IIS不在这里配置
    services.Configure<FormOptions>(options =>
    {
        options.BufferBodyLengthLimit = DHSetting.Current.MaxSize * 1024 * 1024;
        options.KeyLengthLimit = DHSetting.Current.MaxSize * 1024 * 1024;
        options.MultipartBodyLengthLimit = DHSetting.Current.MaxSize * 1024 * 1024;
        options.MultipartBoundaryLengthLimit = DHSetting.Current.MaxSize * 1024 * 1024;
        options.ValueCountLimit = DHSetting.Current.MaxSize * 1024 * 1024;
        options.ValueLengthLimit = DHSetting.Current.MaxSize * 1024 * 1024;
    });

    services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = DHSetting.Current.MaxSize * 1024 * 1024;
        options.Limits.MaxRequestBufferSize = DHSetting.Current.MaxSize * 1024 * 1024;
        options.Limits.MaxRequestLineSize = DHSetting.Current.MaxSize * 1024 * 1024;

        // 默认最小速率为 240 字节/秒，宽限期为 5 秒 时期。更改为最小速率为 80 字节/秒，宽限期为20秒
        options.Limits.MinRequestBodyDataRate =
   new MinDataRate(bytesPerSecond: 80, gracePeriod: TimeSpan.FromSeconds(20));
        options.Limits.MinResponseDataRate =
            new MinDataRate(bytesPerSecond: 80, gracePeriod: TimeSpan.FromSeconds(20));
    });
}

services.Configure<ForwardedHeadersOptions>(options =>   // X-Forwarded-For
{
    options.ForwardLimit = null;
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.ForwardedForHeaderName = SecuritySetting.Current.TrueClientIPHeader;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // 禁用驼峰命名，保留原始属性名称
    })
#if DEBUG
    .AddRazorRuntimeCompilation()  // 用于动态编译 Razor 页面
#endif
    ;

//// 注册 WebSocket 客户端
//services.AddSingleton<MyWebSocketClient>();

//// 后台服务
//services.AddHostedService<MyHostedService>();
//services.AddHostedService<WebSocketHostedService>();

services.AddCube(builder.Configuration, builder.Environment);

services.AddAllSingletons(); // 注册所有单例服务

var app = builder.Build();

//app.UseDefaultFiles();
//app.MapStaticAssets();

//app.UseHttpsRedirection();
app.UseCube(builder.Environment);
//app.UseCubeHome();   # 路由已通过其他方式注册，不需要再注册

app.UseAuthentication();  // 认证中间件 用于Jwt检验

app.UseRouting(); // 路由中间件

app.UseAuthorization(); // 授权中间件

//app.MapControllerRoute(name: "default", pattern: "{controller=Index}/{action=Index}/{id?}");
//app.MapControllerRoute(name: "default2", pattern: "{area=Admin}/{controller=Index}/{action=Index}/{id?}");

app.UsePekEndpoints(); // 注册 Pek 的路由端点

//app.UseCubeHome();

app.RegisterService("PekMvc", null, builder.Environment.EnvironmentName, "/pek/info");

app.Run();