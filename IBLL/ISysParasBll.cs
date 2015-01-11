using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DAL;

namespace IBLL
{
    public interface ISysParasBll
    {
        SysParas GetSysParas();

        bool Edit(ref ValidationErrors validationErrors, SysParas entity);
    }
}
