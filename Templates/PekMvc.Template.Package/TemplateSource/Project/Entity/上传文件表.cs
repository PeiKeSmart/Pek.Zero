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

/// <summary>上传文件表</summary>
[Serializable]
[DataObject]
[Description("上传文件表")]
[BindIndex("IX_DH_Upload_ItemId_FileType", false, "ItemId,FileType")]
[BindTable("DH_Upload", Description = "上传文件表", ConnName = "__DB_CONN_NAME__", DbType = DatabaseType.None)]
public partial class UploadInfo : IUploadInfo, IEntity<IUploadInfo>
{
    #region 属性
    private Int32 _Id;
    [DisplayName("编号")]
    [Description("编号")]
    [DataObjectField(true, true, false, 0)]
    [BindColumn("Id", "编号", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

    private String? _OriginFileName;
    [DisplayName("上传实际文件名")]
    [Description("上传实际文件名")]
    [DataObjectField(false, false, true, 200)]
    [BindColumn("OriginFileName", "上传实际文件名", "")]
    public String? OriginFileName { get => _OriginFileName; set { if (OnPropertyChanging("OriginFileName", value)) { _OriginFileName = value; OnPropertyChanged("OriginFileName"); } } }

    private String? _FileName;
    [DisplayName("上传文件名")]
    [Description("上传文件名")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("FileName", "上传文件名", "")]
    public String? FileName { get => _FileName; set { if (OnPropertyChanging("FileName", value)) { _FileName = value; OnPropertyChanged("FileName"); } } }

    private Int64 _FileSize;
    [DisplayName("上传文件大小")]
    [Description("上传文件大小")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("FileSize", "上传文件大小", "")]
    public Int64 FileSize { get => _FileSize; set { if (OnPropertyChanging("FileSize", value)) { _FileSize = value; OnPropertyChanged("FileSize"); } } }

    private Int32 _FileType;
    [DisplayName("上传文件类别0")]
    [Description("上传文件类别 0:无 1:文章 2:帮助内容 3:店铺幻灯片 4:会员协议 5:积分礼品切换 6:积分礼品内容 7:可视化编辑模块 8:单页文章 9:广告 50:源码")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("FileType", "上传文件类别 0:无 1:文章 2:帮助内容 3:店铺幻灯片 4:会员协议 5:积分礼品切换 6:积分礼品内容 7:可视化编辑模块 8:单页文章 9:广告 50:源码", "")]
    public Int32 FileType { get => _FileType; set { if (OnPropertyChanging("FileType", value)) { _FileType = value; OnPropertyChanged("FileType"); } } }

    private Boolean _IsImg;
    [DisplayName("文件类型true为图片")]
    [Description("文件类型 true为图片")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("IsImg", "文件类型 true为图片", "")]
    public Boolean IsImg { get => _IsImg; set { if (OnPropertyChanging("IsImg", value)) { _IsImg = value; OnPropertyChanged("IsImg"); } } }

    private String? _FileUrl;
    [DisplayName("文件Url")]
    [Description("文件Url")]
    [DataObjectField(false, false, true, 100)]
    [BindColumn("FileUrl", "文件Url", "")]
    public String? FileUrl { get => _FileUrl; set { if (OnPropertyChanging("FileUrl", value)) { _FileUrl = value; OnPropertyChanged("FileUrl"); } } }

    private Int32 _ItemId;
    [DisplayName("文件信息ID")]
    [Description("文件信息ID")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("ItemId", "文件信息ID", "")]
    public Int32 ItemId { get => _ItemId; set { if (OnPropertyChanging("ItemId", value)) { _ItemId = value; OnPropertyChanged("ItemId"); } } }

    private String? _CreateUser;
    [DisplayName("创建者")]
    [Description("创建者")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("CreateUser", "创建者", "")]
    public String? CreateUser { get => _CreateUser; set { if (OnPropertyChanging("CreateUser", value)) { _CreateUser = value; OnPropertyChanged("CreateUser"); } } }

    private Int32 _CreateUserID;
    [DisplayName("创建者")]
    [Description("创建者")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("CreateUserID", "创建者", "")]
    public Int32 CreateUserID { get => _CreateUserID; set { if (OnPropertyChanging("CreateUserID", value)) { _CreateUserID = value; OnPropertyChanged("CreateUserID"); } } }

    private DateTime _CreateTime;
    [DisplayName("创建时间")]
    [Description("创建时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("CreateTime", "创建时间", "")]
    public DateTime CreateTime { get => _CreateTime; set { if (OnPropertyChanging("CreateTime", value)) { _CreateTime = value; OnPropertyChanged("CreateTime"); } } }

    private String? _CreateIP;
    [DisplayName("创建地址")]
    [Description("创建地址")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("CreateIP", "创建地址", "")]
    public String? CreateIP { get => _CreateIP; set { if (OnPropertyChanging("CreateIP", value)) { _CreateIP = value; OnPropertyChanged("CreateIP"); } } }

    private String? _UpdateUser;
    [DisplayName("更新者")]
    [Description("更新者")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("UpdateUser", "更新者", "")]
    public String? UpdateUser { get => _UpdateUser; set { if (OnPropertyChanging("UpdateUser", value)) { _UpdateUser = value; OnPropertyChanged("UpdateUser"); } } }

    private Int32 _UpdateUserID;
    [DisplayName("更新者")]
    [Description("更新者")]
    [DataObjectField(false, false, false, 0)]
    [BindColumn("UpdateUserID", "更新者", "")]
    public Int32 UpdateUserID { get => _UpdateUserID; set { if (OnPropertyChanging("UpdateUserID", value)) { _UpdateUserID = value; OnPropertyChanged("UpdateUserID"); } } }

    private DateTime _UpdateTime;
    [DisplayName("更新时间")]
    [Description("更新时间")]
    [DataObjectField(false, false, true, 0)]
    [BindColumn("UpdateTime", "更新时间", "")]
    public DateTime UpdateTime { get => _UpdateTime; set { if (OnPropertyChanging("UpdateTime", value)) { _UpdateTime = value; OnPropertyChanged("UpdateTime"); } } }

    private String? _UpdateIP;
    [DisplayName("更新地址")]
    [Description("更新地址")]
    [DataObjectField(false, false, true, 50)]
    [BindColumn("UpdateIP", "更新地址", "")]
    public String? UpdateIP { get => _UpdateIP; set { if (OnPropertyChanging("UpdateIP", value)) { _UpdateIP = value; OnPropertyChanged("UpdateIP"); } } }
    #endregion

    #region 拷贝
    public void Copy(IUploadInfo model)
    {
        Id = model.Id;
        OriginFileName = model.OriginFileName;
        FileName = model.FileName;
        FileSize = model.FileSize;
        FileType = model.FileType;
        IsImg = model.IsImg;
        FileUrl = model.FileUrl;
        ItemId = model.ItemId;
        CreateUser = model.CreateUser;
        CreateUserID = model.CreateUserID;
        CreateTime = model.CreateTime;
        CreateIP = model.CreateIP;
        UpdateUser = model.UpdateUser;
        UpdateUserID = model.UpdateUserID;
        UpdateTime = model.UpdateTime;
        UpdateIP = model.UpdateIP;
    }
    #endregion

    #region 获取/设置 字段值
    public override Object? this[String name]
    {
        get => name switch
        {
            "Id" => _Id,
            "OriginFileName" => _OriginFileName,
            "FileName" => _FileName,
            "FileSize" => _FileSize,
            "FileType" => _FileType,
            "IsImg" => _IsImg,
            "FileUrl" => _FileUrl,
            "ItemId" => _ItemId,
            "CreateUser" => _CreateUser,
            "CreateUserID" => _CreateUserID,
            "CreateTime" => _CreateTime,
            "CreateIP" => _CreateIP,
            "UpdateUser" => _UpdateUser,
            "UpdateUserID" => _UpdateUserID,
            "UpdateTime" => _UpdateTime,
            "UpdateIP" => _UpdateIP,
            _ => base[name]
        };
        set
        {
            switch (name)
            {
                case "Id": _Id = value.ToInt(); break;
                case "OriginFileName": _OriginFileName = Convert.ToString(value); break;
                case "FileName": _FileName = Convert.ToString(value); break;
                case "FileSize": _FileSize = value.ToLong(); break;
                case "FileType": _FileType = value.ToInt(); break;
                case "IsImg": _IsImg = value.ToBoolean(); break;
                case "FileUrl": _FileUrl = Convert.ToString(value); break;
                case "ItemId": _ItemId = value.ToInt(); break;
                case "CreateUser": _CreateUser = Convert.ToString(value); break;
                case "CreateUserID": _CreateUserID = value.ToInt(); break;
                case "CreateTime": _CreateTime = value.ToDateTime(); break;
                case "CreateIP": _CreateIP = Convert.ToString(value); break;
                case "UpdateUser": _UpdateUser = Convert.ToString(value); break;
                case "UpdateUserID": _UpdateUserID = value.ToInt(); break;
                case "UpdateTime": _UpdateTime = value.ToDateTime(); break;
                case "UpdateIP": _UpdateIP = Convert.ToString(value); break;
                default: base[name] = value; break;
            }
        }
    }
    #endregion

    #region 扩展查询
    public static UploadInfo? FindById(Int32 id)
    {
        if (id < 0) return null;
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);
        return Meta.SingleCache[id];
    }

    public static IList<UploadInfo> FindAllByItemIdAndFileType(Int32 itemId, Int32 fileType)
    {
        if (itemId < 0) return [];
        if (fileType < 0) return [];
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.ItemId == itemId && e.FileType == fileType);
        return FindAll(_.ItemId == itemId & _.FileType == fileType);
    }
    #endregion

    #region 字段名
    public partial class _
    {
        public static readonly Field Id = FindByName("Id");
        public static readonly Field OriginFileName = FindByName("OriginFileName");
        public static readonly Field FileName = FindByName("FileName");
        public static readonly Field FileSize = FindByName("FileSize");
        public static readonly Field FileType = FindByName("FileType");
        public static readonly Field IsImg = FindByName("IsImg");
        public static readonly Field FileUrl = FindByName("FileUrl");
        public static readonly Field ItemId = FindByName("ItemId");
        public static readonly Field CreateUser = FindByName("CreateUser");
        public static readonly Field CreateUserID = FindByName("CreateUserID");
        public static readonly Field CreateTime = FindByName("CreateTime");
        public static readonly Field CreateIP = FindByName("CreateIP");
        public static readonly Field UpdateUser = FindByName("UpdateUser");
        public static readonly Field UpdateUserID = FindByName("UpdateUserID");
        public static readonly Field UpdateTime = FindByName("UpdateTime");
        public static readonly Field UpdateIP = FindByName("UpdateIP");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    public partial class __
    {
        public const String Id = "Id";
        public const String OriginFileName = "OriginFileName";
        public const String FileName = "FileName";
        public const String FileSize = "FileSize";
        public const String FileType = "FileType";
        public const String IsImg = "IsImg";
        public const String FileUrl = "FileUrl";
        public const String ItemId = "ItemId";
        public const String CreateUser = "CreateUser";
        public const String CreateUserID = "CreateUserID";
        public const String CreateTime = "CreateTime";
        public const String CreateIP = "CreateIP";
        public const String UpdateUser = "UpdateUser";
        public const String UpdateUserID = "UpdateUserID";
        public const String UpdateTime = "UpdateTime";
        public const String UpdateIP = "UpdateIP";
    }
    #endregion
}