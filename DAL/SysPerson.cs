using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;

namespace DAL
{
    [MetadataType(typeof(SysPersonMetadata))]//使用SysPersonMetadata对SysPerson进行数据验证
    public partial class SysPerson : IBaseEntity
    {
      
        #region 自定义属性，即由数据实体扩展的实体
        
        [Display(Name = "角色")]
        public string SysRoleId { get; set; }
        [Display(Name = "角色")]
        public string SysRoleIdOld { get; set; }

        [Display(Name = "商家")]
        public string MerchantId { get; set; }
        [Display(Name = "商家")]
        public string MerchantIdOld { get; set; }
        
        #endregion

    }
  
    public class SysPersonMetadata
    {


			[ScaffoldColumn(false)]
			[Display(Name = "主键", Order = 1)]
			public object Id { get; set; }
        
			[ScaffoldColumn(true)]
			[Display(Name = "用户名", Order = 2)]
			[Required(ErrorMessage = "不能为空")]
            [StringLength(200, ErrorMessage = "长度不可超过200")] 
			public string Name { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "姓名", Order = 3)]
            [Required(ErrorMessage = "不能为空")]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object MyName { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "密码", Order = 4)]
			[Required(ErrorMessage = "不能为空")]
			[StringLength(200,MinimumLength=6, ErrorMessage = "长度不可小于6")]
			[DataType(DataType.Password)]
			public object Password { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "确认密码", Order = 5)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			[DataType(DataType.Password)]
			[System.Web.Mvc.Compare("Password", ErrorMessage = "两次密码不一致")]
			public object SurePassword { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "性别", Order =6)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object Sex { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "职位", Order = 7)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object Position { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "手机号码", Order =8)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object MobilePhoneNumber { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "办公电话", Order = 9)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			[DataType(DataType.PhoneNumber,ErrorMessage="号码格式不正确")]
			public object PhoneNumber { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "省", Order = 10)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object Province { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "市", Order = 11)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object City { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "县", Order = 12)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object Village { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "联系地址", Order = 13)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object Address { get; set; }

			[ScaffoldColumn(true)]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "{0}的格式不正确")]
			[Display(Name = "邮箱", Order = 14)]
            //[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object EmailAddress { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "备注", Order = 15)]
			public object Remark { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "状态", Order = 16)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object State { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "创建时间", Order = 17)]
			[DataType(DataType.DateTime,ErrorMessage="时间格式不正确")]
			public DateTime? CreateTime { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "创建人", Order = 18)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object CreatePerson { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "编辑时间", Order = 19)]
			[DataType(DataType.DateTime,ErrorMessage="时间格式不正确")]
			public DateTime? UpdateTime { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "编辑人", Order = 20)]
			[StringLength(200, ErrorMessage = "长度不可超过200")]
			public object UpdatePerson { get; set; }

			[ScaffoldColumn(true)]
			[Display(Name = "时间戳", Order = 21)]
			public object Version { get; set; }

            [ScaffoldColumn(false)]
            [Display(Name = "系统默认Id", Order = 22)]
            public int SysId { get; set; }

    }


}

