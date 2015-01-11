using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 用户（会员、商家统称为用户）
    /// </summary>
    [DataContract]
    public class User
    {
        [Display(Name="用户Id")]
        [DataMember]
        public string Id { get; set; }

        [Display(Name = "用户名称")]
        [DataMember]
        public string Name { get; set; }

        [Display(Name = "注册码")]
        [DataMember]
        public string Code { get; set; }

        [Display(Name = "有效")]
        [DataMember]
        public bool IsValid { get; set; }

        /// <summary>
        /// 普通会员:0;VIP：1；商家:2
        /// </summary>
        [Display(Name = "用户类型")]
        [DataMember]
        public int Type { get; set; }

        /// <summary>
        /// 会员:1;商家:2
        /// </summary>
        [Display(Name = "会员类型")]
        [DataMember]
        public int MemberType { get; set; }


    }
}
