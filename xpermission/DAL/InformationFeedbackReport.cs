using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class InformationFeedbackReport
    {
        /// <summary>
        /// 信息编号
        /// </summary>
        public string InformationId 
        { 
            get;
            set; 
        }

        /// <summary>
        /// 信息标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 信息发送时间
        /// </summary>
        public DateTime SendTime
        {
            get;
            set;
        }


        /// <summary>
        /// 发送信息会员数量
        /// </summary>
        public int ReceiveMemberAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 发送信息商家数量
        /// </summary>
        public int ReceiveMerchantAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈会员数量
        /// </summary>
        public int FeedbackMemberAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈商家数量
        /// </summary>
        public int FeedbackMerchantAmount
        {
            get;
            set;
        }

        private List<FeedbackTemlateMembers> _feedbackTemlateMembersList = new List<FeedbackTemlateMembers>();

        /// <summary>
        /// 反馈模板会员反馈信息集合
        /// </summary>
        public List<FeedbackTemlateMembers> FeedbackTemlateMembersList
        {
            get 
            { 
                return _feedbackTemlateMembersList;
            }
            set 
            { 
                _feedbackTemlateMembersList = value; 
            }
        }

        private List<FeedbackTemlateMembers> _feedbackTemlateMerchantsList = new List<FeedbackTemlateMembers>();

        /// <summary>
        /// 反馈模板商家反馈信息集合
        /// </summary>
        public List<FeedbackTemlateMembers> FeedbackTemlateMerchantsList
        {
            get
            {
                return _feedbackTemlateMerchantsList;
            }
            set
            {
                _feedbackTemlateMerchantsList = value;
            }
        }

        private List<FeedbackTemlateMembers> _unFeedbackTemlateMembersList = new List<FeedbackTemlateMembers>();

        /// <summary>
        /// 反馈模板未反馈会员信息集合
        /// </summary>
        public List<FeedbackTemlateMembers> UnFeedbackTemlateMembersList
        {
            get
            {
                return _unFeedbackTemlateMembersList;
            }
            set
            {
                _unFeedbackTemlateMembersList = value;
            }
        }

        private List<FeedbackTemlateMembers> _unFeedbackTemlateMerchantsList = new List<FeedbackTemlateMembers>();

        /// <summary>
        /// 反馈模板未反馈商家信息集合
        /// </summary>
        public List<FeedbackTemlateMembers> UnFeedbackTemlateMerchantsList
        {
            get
            {
                return _unFeedbackTemlateMerchantsList;
            }
            set
            {
                _unFeedbackTemlateMerchantsList = value;
            }
        }

        private List<FeedbackTemplateStatistics> _feedbackTemplateStatisticsList = new List<FeedbackTemplateStatistics>();

        /// <summary>
        /// 反馈模块会员反馈统计集合
        /// </summary>
        public List<FeedbackTemplateStatistics> FeedbackTemplateStatisticsList
        {
            get 
            {
                return _feedbackTemplateStatisticsList; 
            }
            set 
            {
                _feedbackTemplateStatisticsList = value; 
            }
        }

        private List<FeedbackTemplateStatistics> _feedbackTemplateMerchantStatisticsList = new List<FeedbackTemplateStatistics>();

        /// <summary>
        /// 反馈模块商家反馈统计集合
        /// </summary>
        public List<FeedbackTemplateStatistics> FeedbackTemplateMerchantStatisticsList
        {
            get
            {
                return _feedbackTemplateMerchantStatisticsList;
            }
            set
            {
                _feedbackTemplateMerchantStatisticsList = value;
            }
        }
    }

    /// <summary>
    /// 反馈模块会员反馈信息
    /// </summary>
    public class FeedbackTemlateMembers
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get;
            set;
        }
        /// <summary>
        /// 信息点评会员编号
        /// </summary>
        public string MemberId
        {
            get;
            set;
        }

        /// <summary>
        /// 信息点评会员名称
        /// </summary>
        public string MemberName
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Phone 
        { 
            get;
            set; 
        }

        /// <summary>
        /// 反馈模板名称
        /// </summary>
        public string FeedbackTemplateName
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string FeedbackContent
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 反馈模板会员反馈统计
    /// </summary>
    public class  FeedbackTemplateStatistics
    {
        /// <summary>
        /// 反馈模板编号
        /// </summary>
        public string FeedbackTemplateId
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈模板内容
        /// </summary>
        public string FeedbackTemplateName
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈模板会员反馈数量
        /// </summary>
        public int FeedbackTemplateFeedbackAmount
        {
            get;
            set;
        }
    }
}
