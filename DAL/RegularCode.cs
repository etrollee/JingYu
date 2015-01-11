using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(RegularCodeMetadata))]//使用RegularCodeMetadata对RegularCode进行数据验证
    public partial class RegularCode : IBaseEntity
    {

    }

    public class RegularCodeMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "随机码编号", Order = 1)]
        public int Id { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "随机码", Order = 2)]
        public string Code { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "已使用", Order = 3)]
        public bool IsUsed { get; set; }
    }
}
