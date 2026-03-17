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

/// <summary>用户扩展表</summary>
[Serializable]
[DataObject]
[Description("用户扩展表")]
[BindTable("DH_UserEx", Description = "用户扩展表", ConnName = "__DB_CONN_NAME__", DbType = DatabaseType.None)]
public partial class UserEx : IUserEx, IEntity<IUserEx>
{
    #region 属性
    private Int32 _Id;
    [DisplayName("用户Id")]
    [Description("用户Id")]
    [DataObjectField(true, false, false, 0)]
    [BindColumn("Id", "用户Id", "")]
    public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

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
    public void Copy(IUserEx model)
    {
        Id = model.Id;
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
    public static UserEx? FindById(Int32 id)
    {
        if (id < 0) return null;
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);
        return Meta.SingleCache[id];
    }
    #endregion

    #region 字段名
    public partial class _
    {
        public static readonly Field Id = FindByName("Id");
        public static readonly Field UpdateUser = FindByName("UpdateUser");
        public static readonly Field UpdateUserID = FindByName("UpdateUserID");
        public static readonly Field UpdateTime = FindByName("UpdateTime");
        public static readonly Field UpdateIP = FindByName("UpdateIP");

        static Field FindByName(String name) => Meta.Table.FindByName(name)!;
    }

    public partial class __
    {
        public const String Id = "Id";
        public const String UpdateUser = "UpdateUser";
        public const String UpdateUserID = "UpdateUserID";
        public const String UpdateTime = "UpdateTime";
        public const String UpdateIP = "UpdateIP";
    }
    #endregion
}