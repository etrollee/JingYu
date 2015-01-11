﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using IBLL;

namespace BLL
{
    public class AccountBLL : IAccountBLL
    {
        /// <summary>
        /// 验证用户名和密码是否正确
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功后的用户信息</returns>
        public SysPerson ValidateUser(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
                return null;

            //获取用户信息,请确定web.config中的连接字符串正确
            using (SysEntities db = new SysEntities())
            {
                return (from p in db.SysPerson
                        where p.Name == userName
                        && p.Password == password
                        && p.State == "开启"
                        select p).FirstOrDefault();
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="personName">用户名</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>修改密码是否成功</returns>
        public bool ChangePassword(string personName, string oldPassword, string newPassword)
        {
            if (!string.IsNullOrWhiteSpace(personName) && !string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword))
            {
                try
                {
                    using (SysEntities db = new SysEntities())
                    {
                        var person = db.SysPerson.FirstOrDefault(p => (p.Name == personName) && (p.Password == oldPassword));
                        person.Password = newPassword;
                        person.SurePassword = newPassword;
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionsHander.WriteExceptions(ex);
                }

            }
            return false;
        }
    }
}

