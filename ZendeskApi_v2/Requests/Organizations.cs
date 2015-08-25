#if ASYNC
using System.Threading.Tasks;
#endif
using System.Collections.Generic;
using System.Linq;
using ZendeskApi_v2.Models.Organizations;
using ZendeskApi_v2.Models.Shared;

namespace ZendeskApi_v2.Requests
{
    public interface IOrganizations : ICore
    {
#if SYNC
        GroupOrganizationResponse GetOrganizations(int? perPage = null, int? page = null);

        /// <summary>
        /// Returns an array of organizations whose name starts with the value specified in the name parameter. The name must be at least 2 characters in length.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        GroupOrganizationResponse GetOrganizationsStartingWith(string name);

        GroupOrganizationResponse SearchForOrganizations(string searchTerm);
        IndividualOrganizationResponse GetOrganization(long id);
        IndividualOrganizationResponse CreateOrganization(Organization organization);
        JobStatusResponse CreateManyOrganizations(IEnumerable<Organization> organizations);

        IndividualOrganizationResponse UpdateOrganization(Organization organization);
        bool DeleteOrganization(long id);
        JobStatusResponse DestroyManyOrganizations(IEnumerable<long> ids);
#endif

#if ASYNC
        Task<GroupOrganizationResponse> GetOrganizationsAsync(int? perPage = null, int? page = null);

        /// <summary>
        /// Returns an array of organizations whose name starts with the value specified in the name parameter. The name must be at least 2 characters in length.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<GroupOrganizationResponse> GetOrganizationsStartingWithAsync(string name);

        Task<GroupOrganizationResponse> SearchForOrganizationsAsync(string searchTerm);
        Task<IndividualOrganizationResponse> GetOrganizationAsync(long id);
        Task<IndividualOrganizationResponse> CreateOrganizationAsync(Organization organization);
        Task<JobStatusResponse> CreateManyOrganizationsAsync(IEnumerable<Organization> organizations);
        Task<IndividualOrganizationResponse> UpdateOrganizationAsync(Organization organization);
        Task<bool> DeleteOrganizationAsync(long id);
        Task<JobStatusResponse> DestroyManyOrganizationsAsync(IEnumerable<long> ids);

#endif
    }

    public class Organizations : Core, IOrganizations
    {
        public Organizations(string yourZendeskUrl, string user, string password, string apiToken, string p_OAuthToken)
            : base(yourZendeskUrl, user, password, apiToken, p_OAuthToken)
        {
        }

#if SYNC
        public GroupOrganizationResponse GetOrganizations(int? perPage = null, int? page = null)
        {
            return GenericPagedGet<GroupOrganizationResponse>("organizations.json", perPage, page);
        }

        /// <summary>
        /// Returns an array of organizations whose name starts with the value specified in the name parameter. The name must be at least 2 characters in length.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GroupOrganizationResponse GetOrganizationsStartingWith(string name)
        {
            return GenericPost<GroupOrganizationResponse>(string.Format("organizations/autocomplete.json?name={0}", name));
        }

        public GroupOrganizationResponse SearchForOrganizations(string searchTerm)
        {
            return GenericGet<GroupOrganizationResponse>(string.Format("organizations/search.json?external_id={0}", searchTerm));
        }

        public IndividualOrganizationResponse GetOrganization(long id)
        {
            return GenericGet<IndividualOrganizationResponse>(string.Format("organizations/{0}.json", id));
        }

        public IndividualOrganizationResponse CreateOrganization(Organization organization)
        {
            var body = new {organization};
            return GenericPost<IndividualOrganizationResponse>("organizations.json", body);
        }

        public JobStatusResponse CreateManyOrganizations(IEnumerable<Organization> organizations)
        {
            var body = new { organizations };
            return GenericPost<JobStatusResponse>("organizations/create_many.json", body);
        }

        public IndividualOrganizationResponse UpdateOrganization(Organization organization)
        {
            var body = new { organization };
            return GenericPut<IndividualOrganizationResponse>(string.Format("organizations/{0}.json", organization.Id), body);
        }

        public bool DeleteOrganization(long id)
        {            
            return GenericDelete(string.Format("organizations/{0}.json", id));
        }

        public JobStatusResponse DestroyManyOrganizations(IEnumerable<long> ids)
        {
            string idList = string.Join(",", ids.Select(i => i.ToString()).ToArray());
            return GenericDelete<JobStatusResponse>(string.Format("organizations/destroy_many.json?ids={0}", idList));
        }

#endif

#if ASYNC

        public async Task<GroupOrganizationResponse> GetOrganizationsAsync(int? perPage = null, int? page = null)
        {
            return await GenericPagedGetAsync<GroupOrganizationResponse>("organizations.json", perPage, page);
        }

        /// <summary>
        /// Returns an array of organizations whose name starts with the value specified in the name parameter. The name must be at least 2 characters in length.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<GroupOrganizationResponse> GetOrganizationsStartingWithAsync(string name)
        {
            return await GenericPostAsync<GroupOrganizationResponse>(string.Format("organizations/autocomplete.json?name={0}", name));
        }

        public async Task<GroupOrganizationResponse> SearchForOrganizationsAsync(string searchTerm)
        {
            return await GenericPostAsync<GroupOrganizationResponse>(string.Format("organizations/autocomplete.json?external_id={0}", searchTerm));
        }

        public async Task<IndividualOrganizationResponse> GetOrganizationAsync(long id)
        {
            return await GenericGetAsync<IndividualOrganizationResponse>(string.Format("organizations/{0}.json", id));
        }

        public async Task<IndividualOrganizationResponse> CreateOrganizationAsync(Organization organization)
        {
            var body = new {organization};
            return await GenericPostAsync<IndividualOrganizationResponse>("organizations.json", body);
        }

        public async Task<JobStatusResponse> CreateManyOrganizationsAsync(IEnumerable<Organization> organizations)
        {
            var body = new { organizations };
            return await GenericPostAsync<JobStatusResponse>("organizations/create_many.json", body);
        }

        public async Task<IndividualOrganizationResponse> UpdateOrganizationAsync(Organization organization)
        {
            var body = new { organization };
            return await GenericPutAsync<IndividualOrganizationResponse>(string.Format("organizations/{0}.json", organization.Id), body);
        }

        public async Task<bool> DeleteOrganizationAsync(long id)
        {            
            return await GenericDeleteAsync(string.Format("organizations/{0}.json", id));
        }

        public async Task<JobStatusResponse> DestroyManyOrganizationsAsync(IEnumerable<long> ids)
        {
            string idList = string.Join(",", ids.Select(i => i.ToString()).ToArray());
            return await GenericDeleteAsync<JobStatusResponse>(string.Format("organizations/destroy_many.json?ids={0}", idList));
        }
#endif
    }
}