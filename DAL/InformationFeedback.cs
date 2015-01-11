using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(InformationFeedbackMetadata))]//使用InformationsMetadata对Informations进行数据验证
    public partial class InformationFeedback
    {
        [ScaffoldColumn(true)]
        [Display(Name = "反馈模板")]
        public string FeedbackTemplateName { get; set; }
    }

    public class InformationFeedbackMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "信息反馈编号", Order = 1)]
        public int Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "信息编号", Order = 2)]
        public int InformationsId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "反馈模板", Order = 3)]
        public string FeedbackTemplateId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "反馈会员编号", Order = 4)]
        public string MemberId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "创建时间", Order = 5)]
        public DateTime CreateTime { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "已处理", Order = 6)]
        public Boolean State { get; set; }

                [ScaffoldColumn(true)]
        [Display(Name = "会员名称",Order=7)]
        public string MemberName { get; set; }
    }
}
