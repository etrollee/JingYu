using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    [MetadataType(typeof(AppointmentMetadata))]//使用AppointmentMetadata对Appointment进行数据验证
    public partial class Appointment
    {

    }

    public class AppointmentMetadata
    {
        [ScaffoldColumn(false)]
        [Display(Name = "预约编号", Order = 1)]
        public int Id { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "会员Id", Order = 2)]
        public string MemberId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "预约者", Order = 3)]
        public string MemberName { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "服务产品", Order = 4)]
        public int ServiceProductId { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "创建时间", Order = 5)]
        public DateTime CreateTime { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "已处理", Order = 6)]
        public bool State { get; set; }
    }
}
