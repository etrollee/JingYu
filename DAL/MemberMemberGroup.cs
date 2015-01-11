using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(MemberMemberGroupMetadata))]
    public partial class MemberMemberGroup
    {

    }

    public class MemberMemberGroupMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "会员与分组关系编号", Order = 1)]
        public int Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "会员编号", Order = 2)]
        public int MemberId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "会员分组编号", Order = 3)]
        public int MemberGroupId { get; set; }
    }
}
