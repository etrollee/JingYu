using System;
using DAL;

namespace IBLL
{
    public interface IAccountBLL
    {
        bool ChangePassword(string personName, string oldPassword, string newPassword);
        SysPerson ValidateUser(string userName, string password);
    }
}
