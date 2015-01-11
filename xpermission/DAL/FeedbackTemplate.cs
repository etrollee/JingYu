using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(FeedbackTemplateMetadata))]//使用FeedbackTemplateMetadata对FeedbackTemplate进行数据验证
    public partial class FeedbackTemplate
    {
        [Display(Name = "信息")]
        public string InformationId { get; set; }
        [Display(Name = "信息")]
        public string InformationIdOld { get; set; }
    }

    public class FeedbackTemplateMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "反馈模板编号", Order = 1)]
        public string Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "模板内容", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Name { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "描述", Order = 3)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Description { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "默认模板", Order = 4)]
        public bool IsSys { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "创建人", Order = 5)]
        public string CreatePersonId { get; set; }
    }
}
