using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace PekMvc.Entity;

/// <summary>单页文章翻译</summary>
[Serializable]
[DataObject]
[Description("单页文章翻译")]
[BindIndex("IU_DH_SingleArticleLan_SId_LId", true, "SId,LId")]
[BindTable("DH_SingleArticleLan", Description = "单页文章翻译", ConnName = "__DB_CONN_NAME__", DbType = DatabaseType.None)]
public partial class SingleArticleLan : ISingleArticleLan, IEntity<ISingleArticleLan>
{
    #region 属性
    private Int32 _Id;
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private Int32 _SId;
    [DisplayName("单页文章Id")]
    [Description("单页文章Id")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("SId", "单页文章Id", "")]
    public Int32 SId { get => _SId; set { if (OnPropertyChanging("SId", value)) { _SId = value; OnPropertyChanged("SId"); } } }

    private Int32 _LId;
    [DisplayName("所属语言Id")]
    [Description("所属语言Id")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("LId", "所属语言Id", "")]
    public Int32 LId { get => _LId; set { if (OnPropertyChanging("LId", value)) { _LId = value; OnPropertyChanged("LId"); } } }

    private String? _Name;
    [DisplayName("文章标题")]
    [Description("文章标题")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("Name", "文章标题", "", Master = true)]
    public String? Name { get => _Name; set { if (OnPropertyChanging("Name", value)) { _Name = value; OnPropertyChanged("Name"); } } }

    private String? _Content;
    [DisplayName("内容")]
    [Description("内容")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("Content", "内容", "text")]
    public String? Content { get => _Content; set { if (OnPropertyChanging("Content", value)) { _Content = value; OnPropertyChanged("Content"); } } }

    private String? _MobileContent;
    [DisplayName("内容(移动端)")]
    [Description("内容(移动端)")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("MobileContent", "内容(移动端)", "text")]
    public String? MobileContent { get => _MobileContent; set { if (OnPropertyChanging("MobileContent", value)) { _MobileContent = value; OnPropertyChanged("MobileContent"); } } }

    private String? _Summary;
    [DisplayName("简介")]
    [Description("简介")]
    [DataObjectField(false, false, true, 512)]
    [BindColumn("Summary", "简介", "")]
    public String? Summary { get => _Summary; set { if (OnPropertyChanging("Summary", value)) { _Summary = value; OnPropertyChanged("Summary"); } } }

    private String? _Pic;
    [DisplayName("文章主图")]
    [Description("文章主图")]
    [DataObjectField(false, false, true, 255)]
    [BindColumn("Pic", "文章主图", "")]
    public String? Pic { get => _Pic; set { if (OnPropertyChanging("Pic", value)) { _Pic = value; OnPropertyChanged("Pic"); } } }

    private String? _SeoTitle;
    [DisplayName("SEO标题")]
    [Description("SEO标题")]
    [DataObjectField(false, false, true, 255)]
    [BindColumn("SeoTitle", "SEO标题", "")]
    public String? SeoTitle { get => _SeoTitle; set { if (OnPropertyChanging("SeoTitle", value)) { _SeoTitle = value; OnPropertyChanged("SeoTitle"); } } }

    private String? _Keys;
    [DisplayName("SEO关键词")]
    [Description("SEO关键词")]
    [DataObjectField(false, false, true, 255)]
    [BindColumn("Keys", "SEO关键词", "")]
    public String? Keys { get => _Keys; set { if (OnPropertyChanging("Keys", value)) { _Keys = value; OnPropertyChanged("Keys"); } } }

    private String? _Description;
    [DisplayName("SEO描述")]
    [Description("SEO描述")]
    [DataObjectField(false, false, true, 255)]
    [BindColumn("Description", "SEO描述", "")]
    public String? Description { get => _Description; set { if (OnPropertyChanging("Description", value)) { _Description = value; OnPropertyChanged("Description"); } } }
    #endregion

    #region 拷贝
    public void Copy(ISingleArticleLan model)
    {
        Id = model.Id;
        SId = model.SId;
        LId = model.LId;
        Name = model.Name;
        Content = model.Content;
        MobileContent = model.MobileContent;
        Summary = model.Summary;
        Pic = model.Pic;
        SeoTitle = model.SeoTitle;
        Keys = model.Keys;
        Description = model.Description;
    }
    #endregion

    #region 获取/设置 字段值
    public override Object? this[String name]
    {
        get => name switch
        {
            "Id" => _Id,
            "SId" => _SId,
            "LId" => _LId,
            "Name" => _Name,
            "Content" => _Content,
            "MobileContent" => _MobileContent,
            "Summary" => _Summary,
            "Pic" => _Pic,
            "SeoTitle" => _SeoTitle,
            "Keys" => _Keys,
            "Description" => _Description,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "SId": _SId = value.ToInt(); break;
                case "LId": _LId = value.ToInt(); break;
                case "Name": _Name = Convert.ToString(value); break;
                case "Content": _Content = Convert.ToString(value); break;
                case "MobileContent": _MobileContent = Convert.ToString(value); break;
                case "Summary": _Summary = Convert.ToString(value); break;
                case "Pic": _Pic = Convert.ToString(value); break;
                case "SeoTitle": _SeoTitle = Convert.ToString(value); break;
                case "Keys": _Keys = Convert.ToString(value); break;
                case "Description": _Description = Convert.ToString(value); break;
                default: base[name] = value; break;
            }
        }
    }
    #endregion

    #region 扩展查询
    public static SingleArticleLan? FindById(Int32 id)
    {
        if (id < 0) return null;
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);
        return Meta.SingleCache[id];
    }

    public static SingleArticleLan? FindBySIdAndLId(Int32 sId, Int32 lId)
    {
        if (sId < 0) return null;
        if (lId < 0) return null;
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.SId == sId && e.LId == lId);
        return Find(_.SId == sId & _.LId == lId);
    }

    public static IList<SingleArticleLan> FindAllBySId(Int32 sId)
    {
        if (sId < 0) return [];
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.SId == sId);
        return FindAll(_.SId == sId);
    }
    #endregion

    #region 字段名
    public partial class _
    {
        public static readonly Field Id = FindByName("Id");
        public static readonly Field SId = FindByName("SId");
        public static readonly Field LId = FindByName("LId");
        public static readonly Field Name = FindByName("Name");
        public static readonly Field Content = FindByName("Content");
        public static readonly Field MobileContent = FindByName("MobileContent");
        public static readonly Field Summary = FindByName("Summary");
        public static readonly Field Pic = FindByName("Pic");
        public static readonly Field SeoTitle = FindByName("SeoTitle");
        public static readonly Field Keys = FindByName("Keys");
        public static readonly Field Description = FindByName("Description");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    public partial class __
    {
        public const String Id = "Id";
        public const String SId = "SId";
        public const String LId = "LId";
        public const String Name = "Name";
        public const String Content = "Content";
        public const String MobileContent = "MobileContent";
        public const String Summary = "Summary";
        public const String Pic = "Pic";
        public const String SeoTitle = "SeoTitle";
        public const String Keys = "Keys";
        public const String Description = "Description";
    }
    #endregion
}