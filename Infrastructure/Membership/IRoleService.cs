namespace Cravens.Infrastructure.Membership
{
	public interface IRoleService
	{
		string[] GetAllRoles();
		string[] GetUsersInRole(string role);
		void RemoveUserFromRole(string userName, string roleName);
		void RemoveUsersFromRole(string[] userNames, string role);
		void RemoveUsersFromRoles(string[] userNames, string[] roleNames);
		bool DeleteRole(string roleName, bool throwOnPopulatedRole);
		bool RoleExists(string roleName);
		void CreateRole(string roleName);
		bool IsUserInRole(string userName, string roleName);
		void AddUserToRole(string userName, string roleName);
		void AddUsersToRoles(string[] userName, string[] roleName);
	}
}