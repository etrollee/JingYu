using System;

namespace IBLL
{
    public interface ISysFieldHander
    {
        string GetMyTextsById(string id);
        global::System.Collections.Generic.List<global::DAL.SysField> GetSysField(string table, string colum);
        global::System.Collections.Generic.List<global::DAL.SysField> GetSysField(string table, string colum, string parentMyTexts);
        global::System.Collections.Generic.List<global::DAL.SysField> GetSysFieldByParent(string id, string parentid, string value);
        global::System.Collections.Generic.List<global::DAL.SysField> GetSysFieldByParentId(string id);
    }
}
