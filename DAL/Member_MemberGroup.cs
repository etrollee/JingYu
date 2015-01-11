using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class Member_MemberGroup
    {
        public string MemberId
        {
            get;
            set;
        }

        public string MemberGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// 会员分组名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
