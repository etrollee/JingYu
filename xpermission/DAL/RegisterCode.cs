using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(RegisterCodeMetadata))]//使用RegisterCodeMetadata对RegisterCode进行数据验证
    public partial class RegisterCode:IBaseEntity
    {
        #region 自定义属性，即有数据实体扩展的实体
        [Display(Name = "生成数量")]
        [Required]
        [Range(1,100)]
        //[RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0}只能是大于0的正整数！")]
        public int Count { get; set; }

        [Display(Name = "所属商家")]
        public string BelongMerchant { get; set; }

        [Display(Name = "使用商家")]
        public string MerchantName { get; set; }

        [Display(Name = "使用会员")]
        public string MemberName { get; set; }

        public string Name { get; set; }

        [Display(Name = "最后登录时间")]
        public DateTime LoginTime { get; set; }

        [Display(Name = "手机串号")]
        public string SerialPort { get; set; }

        #endregion
    }

    public class RegisterCodeMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name="注册码编号",Order=1)]
        public int Id { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "注册码", Order = 2)]
        public string Value { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "已使用", Order = 3)]
        public bool IsUsed { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "已分配", Order = 4)]
        public bool IsDistribution { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "有效", Order = 5)]
        public bool IsValid { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "使用会员", Order = 6)]
        public string MemberId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "使用商家", Order = 7)]
        public string MerchantId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "所属商家", Order =8)]
        public string BelongMerchantId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "创建时间", Order = 9)]
        public DateTime CreateTime { get; set; }
    }

    [MetadataType(typeof(RegisterCodeLoginMetadata))]//使用RegisterCodeMetadata对RegisterCode进行数据验证
    public partial class RegisterCodeLogin : IBaseEntity
    { 
        
    }

    public class RegisterCodeLoginMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "编号", Order = 1)]
        public int Id { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "注册码", Order = 2)]
        public string RCode { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "登录时间", Order = 3)]
        public DateTime LoginTime { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "手机串号", Order = 4)]
        public string SerialPort { get; set; }

    }
}
