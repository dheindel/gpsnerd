using System;
using System.Linq;

namespace Cravens.Infrastructure.Membership
{
    public interface IMembership
    {
        string UserName { get; set; }
        string Email { get; set; }
    }

    public interface IMembershipRepository
    {
        bool CreateUser(string userName, string password, string email, out string errorString);
        IMembership GetUser(string userName, bool userIsOnline);
        IQueryable<IMembership> GetAllUsers();
        IQueryable<IMembership> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);
        void UpdateUser(IMembership user);
        bool DeleteUser(string userName, bool deleteAllRelatedData);
    }

    public interface IMembershipService : IMembershipRepository
    {
        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword, out string errorString);
        bool ResetPassword(string userName, string adminUserName, out string newPassword, out string errorString);
    }



}