using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(InformationsMetadata))]//使用InformationsMetadata对Informations进行数据验证
    public partial class Informations
    {
        [Display(Name = "反馈模板")]
        public string FeedbackTemplateId { get; set; }
        [Display(Name = "反馈模板")]
        public string FeedbackTemplateIdOld { get; set; }

        [Display(Name = "会员")]
        public string MemberId { get; set; }
        [Display(Name = "会员")]
        public string MemberIdOld { get; set; }
        [Display(Name="商家")]
        public string MerchantId { get; set; }
        [Display(Name="发布人名称")]
        public string PersonName { get; set; }

        [Display(Name = "是否已阅读")]
        public bool IsRead { get; set; }

        [Display(Name="发送信息反馈模板")]
        private List<FeedbackTemplate> _feedbackTemplates = new List<FeedbackTemplate>();
        [Display(Name = "发送信息反馈模板")]
        public List<FeedbackTemplate> FeedbackTemplates 
        {
            get { return _feedbackTemplates; }
            set { _feedbackTemplates = value; }
        }

        [Display(Name="会员信息反馈")]
        private InformationFeedback _informationFeedbacks = new InformationFeedback();
        [Display(Name = "会员信息反馈")]
        public InformationFeedback InformationFeedbacks
        {
            get { return _informationFeedbacks; }
            set { _informationFeedbacks = value; }
        }

    }

    public class InformationsMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "信息编号", Order = 1)]
        public string Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "标题", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Title { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "内容", Order = 3)]
        [Required(ErrorMessage = "不能为空")]
        //[StringLength(4000, ErrorMessage = "长度不可超过4000")]
        public string Content { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "时限", Order = 4)]
        public float TimeLimit { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "发布人", Order = 5)]
        public string CreatePersonId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "发布时间", Order = 6)]
        public DateTime CreateTime { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "已发送", Order = 7)]
        public Boolean IsSend { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name="信息类型",Order=8)]
        [Required(ErrorMessage = "不能为空")]
        public int Type { get; set; }
    }
}
