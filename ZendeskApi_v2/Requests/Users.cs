﻿using System.Collections.Generic;
using System.Linq;
#if ASYNC
using System.Net;
using System.Threading.Tasks;
#endif
using ZendeskApi_v2.Models.Shared;
using ZendeskApi_v2.Models.Users;
using User = ZendeskApi_v2.Models.Users.User;

namespace ZendeskApi_v2.Requests
{
    public interface IUsers : ICore
    {
#if SYNC
        IndividualUserResponse GetCurrentUser();
        GroupUserResponse GetAllUsers(int? perPage = null, int? page = null);
        IndividualUserResponse GetUser(long id);
        GroupUserResponse SearchByEmail(string email);
        GroupUserResponse SearchByExternalId(string externalId);
        GroupUserResponse GetUsersInGroup(long id);
        GroupUserResponse GetUsersInOrganization(long id);
        IndividualUserResponse CreateUser(User user);
        JobStatusResponse BulkCreateUsers(IEnumerable<User> users);
        IndividualUserResponse SuspendUser(long id);
        IndividualUserResponse UpdateUser(User user);
        bool DeleteUser(long id);
        bool DeleteManyUsers(IEnumerable<long> ids);
        bool SetUsersPassword(long userId, string newPassword);
        bool ChangeUsersPassword(long userId, string oldPassword, string newPassword);
        GroupUserIdentityResponse GetUserIdentities(long userId);
        IndividualUserIdentityResponse GetSpecificUserIdentity(long userId, long identityId);
        IndividualUserIdentityResponse AddUserIdentity(long userId, UserIdentity identity);
        IndividualUserIdentityResponse SetUserIdentityAsVerified(long userId, long identityId);
        GroupUserIdentityResponse SetUserIdentityAsPrimary(long userId, long identityId);

        /// <summary>
        /// This sends a verification email to the user, asking him to click a link in order to verify ownership of the email address
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityId"></param>
        /// <returns></returns>
        IndividualUserIdentityResponse SendUserVerificationRequest(long userId, long identityId);

        bool DeleteUserIdentity(long userId, long identityId);
#endif

#if ASYNC
        Task<IndividualUserResponse> GetCurrentUserAsync();
        Task<GroupUserResponse> GetAllUsersAsync(int? perPage = null, int? page = null);
        Task<IndividualUserResponse> GetUserAsync(long id);
        Task<GroupUserResponse> SearchByEmailAsync(string email);
        Task<GroupUserResponse> SearchByExternalIdAsync(string externalId);
        Task<GroupUserResponse> GetUsersInGroupAsync(long id);
        Task<GroupUserResponse> GetUsersInOrganizationAsync(long id);
        Task<IndividualUserResponse> CreateUserAsync(User user);
        Task<JobStatusResponse> BulkCreateUsersAsync(IEnumerable<User> users);
        Task<IndividualUserResponse> SuspendUserAsync(long id);
        Task<IndividualUserResponse> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(long id);
        Task<bool> SetUsersPasswordAsync(long userId, string newPassword);
        Task<bool> ChangeUsersPasswordAsync(long userId, string oldPassword, string newPassword);
        Task<GroupUserIdentityResponse> GetUserIdentitiesAsync(long userId);
        Task<IndividualUserIdentityResponse> GetSpecificUserIdentityAsync(long userId, long identityId);
        Task<IndividualUserIdentityResponse> AddUserIdentityAsync(long userId, UserIdentity identity);
        Task<IndividualUserIdentityResponse> SetUserIdentityAsVerifiedAsync(long userId, long identityId);
        Task<GroupUserIdentityResponse> SetUserIdentityAsPrimaryAsync(long userId, long identityId);

        /// <summary>
        /// This sends a verification email to the user, asking him to click a link in order to verify ownership of the email address
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityId"></param>
        /// <returns></returns>
        Task<IndividualUserIdentityResponse> SendUserVerificationRequestAsync(long userId, long identityId);

        Task<bool> DeleteUserIdentityAsync(long userId, long identityId);
#endif
    }

    public class Users : Core, IUsers
    {
        public Users(string yourZendeskUrl, string user, string password, string apiToken, string p_OAuthToken)
            : base(yourZendeskUrl, user, password, apiToken, p_OAuthToken)
        {
        }

#if SYNC
        public IndividualUserResponse GetCurrentUser()
        {
            return GenericGet<IndividualUserResponse>("users/me.json");
        }

        public GroupUserResponse GetAllUsers(int? perPage = null, int? page = null)
        {
            return GenericPagedGet<GroupUserResponse>("users.json", perPage, page);
        }

        public IndividualUserResponse GetUser(long id)
        {
            return GenericGet<IndividualUserResponse>(string.Format("users/{0}.json", id));
        }

        public GroupUserResponse SearchByEmail(string email)
        {
            return GenericGet<GroupUserResponse>(string.Format("users/search.json?query={0}", email));
        }

        public GroupUserResponse SearchByExternalId(string externalId)
        {
            return GenericGet<GroupUserResponse>(string.Format("users/search.json?external_id={0}", externalId));
        }

        public GroupUserResponse GetUsersInGroup(long id)
        {
            return GenericGet<GroupUserResponse>(string.Format("groups/{0}/users.json", id));
        }

        public GroupUserResponse GetUsersInOrganization(long id)
        {
            return GenericGet<GroupUserResponse>(string.Format("organizations/{0}/users.json", id));
        }

        public IndividualUserResponse CreateUser(User user)
        {
            var body = new { user = user };
            return GenericPost<IndividualUserResponse>("users.json", body);
        }

        public JobStatusResponse BulkCreateUsers(IEnumerable<User> users)
        {
            var body = new {users = users};
            return GenericPost<JobStatusResponse>("users/create_many.json", body);
        }

        public IndividualUserResponse SuspendUser(long id)
        {
            var body = new {user = new {suspended = true}};
            return GenericPut<IndividualUserResponse>(string.Format("users/{0}.json", id), body);
        }

        public IndividualUserResponse UpdateUser(User user)
        {
            var body = new { user = user };
            return GenericPut<IndividualUserResponse>(string.Format("users/{0}.json", user.Id), body);
        }

        public bool DeleteUser(long id)
        {
            return GenericDelete(string.Format("users/{0}.json", id));
        }

        public bool DeleteManyUsers(IEnumerable<long> ids)
        {
            string idList = string.Join(",", ids.Select(i => i.ToString()).ToArray());
            return GenericDelete(string.Format("users/destroy_many.json?ids={0}", idList));
        }

        public bool SetUsersPassword(long userId, string newPassword)
        {
            var body = new {password = newPassword};
            return GenericBoolPost(string.Format("users/{0}/password.json", userId), body);           
        }

        public bool ChangeUsersPassword(long userId, string oldPassword, string newPassword)
        {
            var body = new
                {
                    previous_password = oldPassword,
                    password = newPassword
                };
            return GenericBoolPost(string.Format("users/{0}/password.json", userId), body);
        }

        public GroupUserIdentityResponse GetUserIdentities(long userId)
        {
            return GenericGet<GroupUserIdentityResponse>(string.Format("users/{0}/identities.json", userId));
        }

        public IndividualUserIdentityResponse GetSpecificUserIdentity(long userId, long identityId)
        {
            return GenericGet<IndividualUserIdentityResponse>(string.Format("users/{0}/identities/{1}.json", userId, identityId));
        }

        public IndividualUserIdentityResponse AddUserIdentity(long userId, UserIdentity identity)
        {
            var body = new { identity };
            return GenericPost<IndividualUserIdentityResponse>(string.Format("users/{0}/identities.json", userId), body);
        }        

        public IndividualUserIdentityResponse SetUserIdentityAsVerified(long userId, long identityId)
        {
            return GenericPut<IndividualUserIdentityResponse>(string.Format("users/{0}/identities/{1}/verify.json", userId, identityId));
        }

        public GroupUserIdentityResponse SetUserIdentityAsPrimary(long userId, long identityId)
        {
            return GenericPut<GroupUserIdentityResponse>(string.Format("users/{0}/identities/{1}/make_primary.json", userId, identityId));
        }

        /// <summary>
        /// This sends a verification email to the user, asking him to click a link in order to verify ownership of the email address
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityId"></param>
        /// <returns></returns>
        public IndividualUserIdentityResponse SendUserVerificationRequest(long userId, long identityId)
        {            
            return GenericPut<IndividualUserIdentityResponse>(string.Format("users/{0}/identities/{1}/request_verification.json", userId, identityId));
        }

        public bool DeleteUserIdentity(long userId, long identityId)
        {
            return GenericDelete(string.Format("users/{0}/identities/{1}.json", userId, identityId));
        }       

#endif

#if ASYNC
        public async Task<IndividualUserResponse> GetCurrentUserAsync()
        {
            return await GenericGetAsync<IndividualUserResponse>("users/me.json");
        }

        public async Task<GroupUserResponse> GetAllUsersAsync(int? perPage = null, int? page = null)
        {
            return await GenericPagedGetAsync<GroupUserResponse>("users.json", perPage, page);
        }

        public async Task<IndividualUserResponse> GetUserAsync(long id)
        {
            return await GenericGetAsync<IndividualUserResponse>(string.Format("users/{0}.json", id));
        }

        public async Task<GroupUserResponse> SearchByEmailAsync(string email)
        {
            return await GenericGetAsync<GroupUserResponse>(string.Format("users/search.json?query={0}", email));
        }

        public async Task<GroupUserResponse> SearchByExternalIdAsync(string externalId)
        {
            return await GenericGetAsync<GroupUserResponse>(string.Format("users/search.json?external_id={0}", externalId));
        }

        public async Task<GroupUserResponse> GetUsersInGroupAsync(long id)
        {
            return await GenericGetAsync<GroupUserResponse>(string.Format("groups/{0}/users.json", id));
        }

        public async Task<GroupUserResponse> GetUsersInOrganizationAsync(long id)
        {
            return await GenericGetAsync<GroupUserResponse>(string.Format("organizations/{0}/users.json", id));
        }

        public async Task<IndividualUserResponse> CreateUserAsync(User user)
        {
            var body = new { user = user };
            return await GenericPostAsync<IndividualUserResponse>("users.json", body);
        }

        public async Task<JobStatusResponse> BulkCreateUsersAsync(IEnumerable<User> users)
        {
            var body = new {users = users};
            return await GenericPostAsync<JobStatusResponse>("users/create_many.json", body);
        }

        public async Task<IndividualUserResponse> SuspendUserAsync(long id)
        {
            var body = new {user = new {suspended = true}};
            return await GenericPutAsync<IndividualUserResponse>(string.Format("users/{0}.json", id), body);
        }

        public async Task<IndividualUserResponse> UpdateUserAsync(User user)
        {
            var body = new { user = user };
            return await GenericPutAsync<IndividualUserResponse>(string.Format("users/{0}.json", user.Id), body);
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            return await GenericDeleteAsync(string.Format("users/{0}.json", id));
        }

        public async Task<bool> SetUsersPasswordAsync(long userId, string newPassword)
        {
            var body = new { password = newPassword };
            return await GenericBoolPostAsync(string.Format("users/{0}/password.json", userId), body);
        }

        public async Task<bool> ChangeUsersPasswordAsync(long userId, string oldPassword, string newPassword)
        {
            var body = new
            {
                previous_password = oldPassword,
                password = newPassword
            };
            return await GenericBoolPostAsync(string.Format("users/{0}/password.json", userId), body);
        }

        public async Task<GroupUserIdentityResponse> GetUserIdentitiesAsync(long userId)
        {
            return await GenericGetAsync<GroupUserIdentityResponse>(string.Format("users/{0}/identities.json", userId));
        }

        public async Task<IndividualUserIdentityResponse> GetSpecificUserIdentityAsync(long userId, long identityId)
        {
            return await GenericGetAsync<IndividualUserIdentityResponse>(string.Format("users/{0}/identities/{1}.json", userId, identityId));
        }

        public async Task<IndividualUserIdentityResponse> AddUserIdentityAsync(long userId, UserIdentity identity)
        {
            var body = new { identity };
            return await GenericPostAsync<IndividualUserIdentityResponse>(string.Format("users/{0}/identities.json", userId), body);
        }        

        public async Task<IndividualUserIdentityResponse> SetUserIdentityAsVerifiedAsync(long userId, long identityId)
        {
            return await GenericPutAsync<IndividualUserIdentityResponse>(string.Format("users/{0}/identities/{1}/verify.json", userId, identityId));
        }

        public async Task<GroupUserIdentityResponse> SetUserIdentityAsPrimaryAsync(long userId, long identityId)
        {
            return await GenericPutAsync<GroupUserIdentityResponse>(string.Format("users/{0}/identities/{1}/make_primary.json", userId, identityId));
        }

        /// <summary>
        /// This sends a verification email to the user, asking him to click a link in order to verify ownership of the email address
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityId"></param>
        /// <returns></returns>
        public async Task<IndividualUserIdentityResponse> SendUserVerificationRequestAsync(long userId, long identityId)
        {            
            return await GenericPutAsync<IndividualUserIdentityResponse>(string.Format("users/{0}/identities/{1}/request_verification.json", userId, identityId));
        }

        public async Task<bool> DeleteUserIdentityAsync(long userId, long identityId)
        {
            return await GenericDeleteAsync(string.Format("users/{0}/identities/{1}.json", userId, identityId));
        }
#endif
    }
}
