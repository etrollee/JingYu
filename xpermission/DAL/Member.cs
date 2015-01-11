using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(MemberMetadata))]//使用MemberMetadata对Member进行数据验证
    public partial class Member
    {

        [Display(Name = "会员分组")]
        public string MemberGroupId { get; set; }
        [Display(Name = "会员分组")]
        public string MemberGroupIdOld { get; set; }

        [Display(Name="注册码")]
        public string OldRegisterCode { get; set; }
    }

    public class MemberMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "会员编号", Order = 1)]
        public string Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "会员名称", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Name { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "代号", Order = 3)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Code { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "创建人", Order = 4)]
        public string CreatePersonId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "注册码", Order = 5)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string RegisterCode { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "地区", Order = 6)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string Area { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "成立时间", Order = 7)]
        [DataType(DataType.Date, ErrorMessage = "时间格式不正确,请输入如2013-10-01的日期格式")]
        public DateTime Regtime { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "注册资金", Order = 8)]
        public decimal RegisteredCapital { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "联系人", Order =9)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string  Contacts { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "联系电话", Order = 10)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public object Phone { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "注册地址", Order = 11)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string RegisteredAddress { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "法定代表", Order = 12)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string LegalRepresentative { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "注册手机号", Order = 13)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string RegisteredCellPhone { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "自定义1", Order = 14)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public object SelfDefineOne { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "自定义2", Order = 15)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public object SelfDefineTwo { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "备注", Order = 16)]
        public object Remark { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "VIP会员", Order = 17)]
        public bool VIP { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "有效", Order = 18)]
        public bool IsValid { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "可见性", Order = 19)]
        public bool IsVisible { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "登录次数", Order = 20)]
        public int LogOnTimes { get; set; }
    }
}
