using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(MerchantMetadata))]//使用MerchantMetadata对Merchant进行数据验证
    public partial class Merchant : IBaseEntity
    {
        [Display(Name = "用户")]
        public string SysPersonId { get; set; }
        [Display(Name = "用户")]
        public string SysPersonIdOld { get; set; }

        [Display(Name = "注册码")]
        public string OldRegisterCode { get; set; }
    }

    public class MerchantMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "商家编号", Order = 1)]
        [DataMember]
        public string Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "商家名称", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        [DataMember(IsRequired=true)]
        public string Name { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "商家Logo", Order = 3)]
        public byte[] Logo { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "简介", Order = 4)]
        public string Description { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "固定电话", Order = 5)]
        [RegularExpression("^[0-9]{4,4}-?[0-9]{7,7}|1[0-9]{10}$", ErrorMessage = "格式不正确，请输入座机或手机号")]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string Telephone { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "联系人", Order = 6)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string Contacts { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "移动电话", Order = 7)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string Cellphone { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "网站地址", Order = 8)]
        [RegularExpression(@"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", ErrorMessage = "{0}的格式不正确")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string SiteUrl { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "地址", Order = 9)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Address { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "综合星级", Order = 10)]
        [Range(0, 3, ErrorMessage = "只能是0~3的整数")]
        public int ComprehensiveStar { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "商家类别", Order = 11)]
        [Required(ErrorMessage = "不能为空")]
        [DataMember(IsRequired = true)]
        public int MerchantTypeId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "注册码", Order = 12)]
        [StringLength(50, ErrorMessage = "长度不可超过50")]
        public string RegisterCode { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "业务QQ", Order = 13)]
        //[StringLength(16, ErrorMessage = "{0}最多{1}位数")]
        [RegularExpression(@"^[1-9]\d{0,15}$", ErrorMessage = "{0}需介于1和9999999999999999之间")]
        public ulong QQ { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "上级商家Id", Order = 14)]
        public string  ParentId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "余额", Order = 15)]
        [Required(ErrorMessage = "不能为空")] 
        //[StringLength(17,ErrorMessage="{0}最多{1}位数")]
        [Range(typeof(decimal),"0","9999999999999999.99",ErrorMessage="{0}需介于{1}和{2}之间") ]
        public decimal Balance { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "上级商家可见", Order = 16)]
        [DataMember(IsRequired = false)]
        public int IsVisible { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "更新标志", Order = 17)]
        public Guid UpdateFlag { get; set; }
    }

}
