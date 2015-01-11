using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(MerchantRegisterCodeMetadata))]
    public partial class MerchantRegisterCode : IBaseEntity
    {

        [Display(Name = "注册码")]
        public string RegisterCode { get; set; }

        [Display(Name = "已使用")]
        public bool IsUsed { get; set; }

        [Display(Name = "已分配")]
        public bool IsDistribution { get; set; }

        [Display(Name = "有效")]
        public bool IsValid { get; set; }

        [Display(Name = "会员")]
        public string MemberId { get; set; }
    }

    /// <summary>
    /// 商家注册码
    /// </summary>
    public class MerchantRegisterCodeMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "编号", Order = 1)]
        public int Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "注册码", Order = 2)]
        public int RegisterCodeId { get; set; }


        [ScaffoldColumn(true)]
        [Display(Name = "商家", Order = 2)]
        public string MerchantId { get; set; }

    }
}
