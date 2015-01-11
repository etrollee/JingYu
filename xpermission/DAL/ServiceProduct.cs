using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(ServiceProductMetadata))]//使用ServiceProductMetadata对ServiceProduct进行数据验证
    public partial class ServiceProduct : IBaseEntity
    {

    }

    public class ServiceProductMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "服务产品编号", Order = 1)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "商家", Order = 2)]
        public string MerchantId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "产品名称", Order = 3)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public object Name { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "特点介绍", Order = 4)]
        public object Description { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "星级", Order = 5)]
        [Range(0,3,ErrorMessage = "只能是0~3的整数")]
        public int Star { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "预约价格", Order = 6)]
        [Required(ErrorMessage = "不能为空")]
        [Range(typeof(decimal), "0", "9999999999999999.99", ErrorMessage = "{0}需介于{1}和{2}之间")]
        public decimal Worth { get; set; }
    }
}
