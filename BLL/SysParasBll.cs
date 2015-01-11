using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using DAL;
using IBLL;

namespace BLL
{
    public class SysParasBll : ISysParasBll, IDisposable
    {
        public SysParas GetSysParas()
        {
            using (var dataContext=new SysEntities())
            {
                return dataContext.SysParas.FirstOrDefault();
            }
        }

        public bool Edit(ref ValidationErrors validationErrors, SysParas entity)
        {
            using (var dataContext=new SysEntities())
            {
                var sysParas = dataContext.SysParas.FirstOrDefault(o=>o.Id==entity.Id);
                sysParas.WelcomeInfo = entity.WelcomeInfo;
                sysParas.DeductMoney = entity.DeductMoney;

              return  dataContext.SaveChanges() > 0;
            }
        }

        public void Dispose()
        {

        }
    }
}
