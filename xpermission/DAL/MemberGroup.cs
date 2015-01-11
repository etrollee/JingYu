using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(MemberGroupMetadata))]//使用MemberGroupMetadata对MemberGroup进行数据验证
    public partial class MemberGroup
    {
        [Display(Name = "会员")]
        public string MemberId { get; set; }
        [Display(Name = "会员")]
        public string MemberIdOld { get; set; }
    }

    public class MemberGroupMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "会员分组编号", Order = 1)]
        public string Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "分组名称", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过500")]
        public string Name { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "创建人", Order =3)]
        public string CreatePersonId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "描述", Order = 4)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public object Description { get; set; }
    }
}
