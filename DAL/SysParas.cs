using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    [MetadataType(typeof(SysParasMetadata))]//使用SysParasMetadata对SysParas进行数据验证
    public partial class SysParas : IBaseEntity
    {

        #region 自定义属性，即由数据实体扩展的实体

        #endregion

    }
    public class SysParasMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "主键", Order = 1)]
        public object Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "欢迎词", Order = 2)]
        [StringLength(4000, ErrorMessage = "长度不可超过4000")]
        [Required(ErrorMessage = "不能为空")]
        public string WelcomeInfo { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "查看联系方式扣款", Order = 3)]
        [Required(ErrorMessage = "不能为空")]
        [Range(typeof(decimal), "0", "9999999999999999.99", ErrorMessage = "{0}需介于{1}和{2}之间")]
        public decimal DeductMoney { get; set; }
    }
}
