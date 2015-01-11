using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(MerchantTypeMetadata))]//使用MerchantTypeMetadata对MerchantType进行数据验证
    public partial class MerchantType : IBaseEntity
    {

    }

    public class MerchantTypeMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "商家类别编号", Order = 1)]
        public int Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "类型名称", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Name { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "描述", Order = 3)]
        public string Description { get; set; }
    }
}
