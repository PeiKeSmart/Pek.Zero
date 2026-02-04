using DH.Entity;

using Microsoft.AspNetCore.Mvc;

using NewLife.Cube.ViewModels;

using Pek.NCube.BaseControllers;

namespace PekMvc.Controllers;

/// <summary>主页面</summary>
//[AllowAnonymous]
public class CubeHomeController : PekBaseControllerX {
    /// <summary>主页面</summary>
    /// <returns></returns>
    public ActionResult Index()
    {
        ViewBag.Message = "主页面";

        //DHSetting.Current.AllowDynamicRedirection = true;
        //DHSetting.Current.Save();

        var model = new SiteInfo();

        return PekView(model/*true, true*/);
    }

    /// <summary>错误</summary>
    /// <returns></returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        // 正式环境中，错误页避免返回500错误码
        HttpContext.Response.StatusCode = 200;

        var model = HttpContext.Items["Exception"] as ErrorModel;
        if (IsJsonRequest)
        {
            if (model?.Exception != null) return Json(500, null, model.Exception);
        }

        return View("Error", model);
    }
}