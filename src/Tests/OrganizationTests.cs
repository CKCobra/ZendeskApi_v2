using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ZendeskApi_v2;
using ZendeskApi_v2.Models.Organizations;
using ZendeskApi_v2.Models.Tags;
using ZendeskApi_v2.Models.Users;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class OrganizationTests
    {
        ZendeskApi api = new ZendeskApi(Settings.Site, Settings.AdminEmail, Settings.AdminPassword);
        [OneTimeSetUp]
        public void Init()
        {
            var orgs = api.Organizations.GetOrganizations();
            if (orgs != null)
            {
                foreach (var org in orgs.Organizations.Where(o => o.Name.Contains("Test Org") || o.Name.Contains("Test Org2")))
                {
                    api.Organizations.DeleteOrganization(org.Id.Value);
                }
            }

            var users = api.Users.SearchByEmail("test_org_mem@test.com");
            if (users != null)
            {
                foreach (var user in users.Users.Where(o => o.Name.Contains("Test User Org Mem")))
                {
                    api.Users.DeleteUser(user.Id.Value);
                }
            }
        }

        [Test]
        public void CanAddAndRemoveTagsFromOrganization()
        {
            var tag = new Tag();
            var organization = api.Organizations.GetOrganizations().Organizations.First();
            tag.Name = "MM";
            organization.Tags.Add(tag.Name);

            var org = api.Organizations.UpdateOrganization(organization);
            org.Organization.Tags.Add("New");

            var org2 = api.Organizations.UpdateOrganization(org.Organization);
            org2.Organization.Tags.Remove("MM");
            org2.Organization.Tags.Remove("New");
            api.Organizations.UpdateOrganization(org2.Organization);
        }

        [Test]
        public void CanGetOrganizations()
        {
            var res = api.Organizations.GetOrganizations();
            Assert.Greater(res.Count, 0);

            var org = api.Organizations.GetOrganization(res.Organizations[0].Id.Value);
            Assert.AreEqual(org.Organization.Id, res.Organizations[0].Id);
        }

        [Test]
        public void CanSearchForOrganizations()
        {
            var res = api.Organizations.GetOrganizationsStartingWith(Settings.DefaultOrg.Substring(0, 3));
            Assert.Greater(res.Count, 0);

            var search = api.Organizations.SearchForOrganizationsByExternalId(Settings.DefaultExternalId);
            Assert.Greater(search.Count, 0);
        }

        [Test]
        public void CanGetMultipleOrganizations()
        {
            var org = api.Organizations.CreateOrganization(new Organization()
            {
                Name = "Test Org"
            });

            Assert.Greater(org.Organization.Id, 0);

            var org2 = api.Organizations.CreateOrganization(new Organization()
            {
                Name = "Test Org2"
            });

            var orgs = api.Organizations.GetMultipleOrganizations(new [] { org.Organization.Id.Value, org2.Organization.Id.Value});
            Assert.AreEqual(orgs.Organizations.Count, 2);
        }

        [Test]
        public void CanCreateUpdateAndDeleteOrganizations()
        {
            var res = api.Organizations.CreateOrganization(new Organization()
            {
                Name = "Test Org"
            });

            Assert.Greater(res.Organization.Id, 0);

            res.Organization.Notes = "Here is a sample note";
            var update = api.Organizations.UpdateOrganization(res.Organization);
            Assert.AreEqual(update.Organization.Notes, res.Organization.Notes);

            Assert.True(api.Organizations.DeleteOrganization(res.Organization.Id.Value));
        }

        [Test]
        public void CanCreateAndDeleteOrganizationMemberships()
        {
            var org = api.Organizations.CreateOrganization(new Organization()
            {
                Name = "Test Org"
            });

            var user = new User()
            {
                Name = "Test User Org Mem",
                Email = "test_org_mem@test.com",
                Role = "end-user"
            };


            var res = api.Users.CreateUser(user);

            var org_membership = new OrganizationMembership() { UserId = res.User.Id, OrganizationId = org.Organization.Id };

            var res2 = api.Organizations.CreateOrganizationMembership(org_membership);

            Assert.Greater(res2.OrganizationMembership.Id, 0);
            Assert.True(api.Organizations.DeleteOrganizationMembership(res2.OrganizationMembership.Id.Value));
            Assert.True(api.Users.DeleteUser(res.User.Id.Value));
            Assert.True(api.Organizations.DeleteOrganization(org.Organization.Id.Value));
        }

        [Test]
        public void CanCreateManyAndDeleteOrganizationMemberships()
        {
            var org = api.Organizations.CreateOrganization(new Organization()
            {
                Name = "Test Org"
            });

            Assert.Greater(org.Organization.Id, 0);

            var org2 = api.Organizations.CreateOrganization(new Organization()
            {
                Name = "Test Org2"
            });

            Assert.Greater(org2.Organization.Id, 0);

            var res = api.Users.CreateUser(new User()
            {
                Name = "Test User Org Mem",
                Email = "test_org_mem@test.com",
                Role = "end-user"
            });

            Assert.Greater(res.User.Id, 0);

            var memberships = new List<OrganizationMembership>();
            memberships.Add(new OrganizationMembership() { UserId = res.User.Id, OrganizationId = org.Organization.Id });
            memberships.Add(new OrganizationMembership() { UserId = res.User.Id, OrganizationId = org2.Organization.Id });

            var job = api.Organizations.CreateManyOrganizationMemberships(memberships).JobStatus;
            job     = PollJobStatus(job);

            Assert.Greater(job.Results.Count(), 0);

            Assert.True(api.Organizations.DeleteOrganizationMembership(job.Results[0].Id));
            Assert.True(api.Organizations.DeleteOrganizationMembership(job.Results[1].Id));

            Assert.True(api.Users.DeleteUser(res.User.Id.Value));
            Assert.True(api.Organizations.DeleteOrganization(org.Organization.Id.Value));
            Assert.True(api.Organizations.DeleteOrganization(org2.Organization.Id.Value));
        }

        [Test]
        public void CanCreateAndDeleteManyOrganizations()
        {
            var organizations = new List<Organization>();
            organizations.Add(new Organization() { Name = "Test Org" });
            organizations.Add(new Organization() { Name = "Test Org2" });

            var job = api.Organizations.CreateMultipleOrganizations(organizations).JobStatus;
            job = PollJobStatus(job);

            Assert.That(job.Status, Is.EqualTo("completed"));
            Assert.That(job.Results, Has.Count.GreaterThan(0));
            var statuses = job.Results.Select(o => o.Status);
            Assert.That(statuses, Has.All.EqualTo("Created"));

            var new_organizations = api.Organizations.GetMultipleOrganizations(job.Results.Select(i => i.Id)).Organizations;

            Assert.That(new_organizations, Is.Not.Empty);
            Assert.That(new_organizations, Has.Count.EqualTo(organizations.Count));

            job = api.Organizations.DeleteMultipleOrganizations(new_organizations.Select(i => i.Id.Value)).JobStatus;
            job = PollJobStatus(job);

            Assert.That(job.Total.Value, Is.EqualTo(2));
            Assert.That(job.Status, Is.EqualTo("completed"));
        }

        [Test]
        public void CanCreateAndDeleteManyOrganizationsByExternalId()
        {
            var organizations = new List<Organization>();
            organizations.Add(new Organization() { Name = "Test Org", ExternalId = "112233" });
            organizations.Add(new Organization() { Name = "Test Org2", ExternalId = "223344" });

            var job = api.Organizations.CreateMultipleOrganizations(organizations).JobStatus;
            job     = PollJobStatus(job);

            Assert.That(job.Status, Is.EqualTo("completed"));
            Assert.That(job.Results, Has.Count.GreaterThan(0));
            var statuses = job.Results.Select(o => o.Status);
            Assert.That(statuses, Has.All.EqualTo("Created"));

            var new_organizations = api.Organizations.GetMultipleOrganizationsByExternalId(new List<string> { "112233", "223344" }).Organizations;

            Assert.That(new_organizations, Is.Not.Empty);
            Assert.That(new_organizations, Has.Count.EqualTo(organizations.Count));

            job = api.Organizations.DeleteMultipleOrganizationsByExternalIds(new_organizations.Select(i => i.ExternalId.ToString())).JobStatus;
            job = PollJobStatus(job);

            Assert.That(job.Total.Value, Is.EqualTo(2));
            Assert.That(job.Status, Is.EqualTo("completed"));
        }

        [Test]
        public void CanUpdateMultipleOrganizations()
        {
            var organizations = new List<Organization>();
            organizations.Add(new Organization() { Name = "Test Org", ExternalId = "112233" });
            organizations.Add(new Organization() { Name = "Test Org2", ExternalId = "223344" });

            var job = api.Organizations.CreateMultipleOrganizations(organizations).JobStatus;
            job = PollJobStatus(job);

            Assert.That(job.Status, Is.EqualTo("completed"));
            Assert.That(job.Results, Has.Count.GreaterThan(0));
            var statuses = job.Results.Select(o => o.Status);
            Assert.That(statuses, Has.All.EqualTo("Created"));

            organizations[0].Id    = job.Results[0].Id;
            organizations[0].Notes = "Here is a sample note.";
            organizations[1].Id    = job.Results[1].Id;
            organizations[1].Notes = "Here is a sample note.";

            job = api.Organizations.UpdateMultipleOrganizations(organizations).JobStatus;
            job = PollJobStatus(job);

            Assert.That(job.Status, Is.EqualTo("completed"));
            Assert.That(job.Results, Has.Count.GreaterThan(0));
            statuses = job.Results.Select(o => o.Status);
            Assert.That(statuses, Has.All.EqualTo("Updated"));

            var new_organizations = api.Organizations.GetMultipleOrganizations(job.Results.Select(i => i.Id)).Organizations;

            Assert.That(new_organizations, Is.Not.Empty);
            Assert.That(new_organizations, Has.Count.EqualTo(organizations.Count));
            var notes = new_organizations.Select(o => o.Notes.ToString());
            Assert.That(notes, Has.All.EqualTo("Here is a sample note."));

            job = api.Organizations.DeleteMultipleOrganizations(new_organizations.Select(i => i.Id.Value)).JobStatus;
            job = PollJobStatus(job);

            Assert.That(job.Total.Value, Is.EqualTo(2));
            Assert.That(job.Status, Is.EqualTo("completed"));
        }

        [Test]
        public async Task CanSearchForOrganizationsAsync()
        {
            var search = await api.Organizations.SearchForOrganizationsAsync(Settings.DefaultExternalId);
            Assert.Greater(search.Count, 0);
        }

        private ZendeskApi_v2.Models.Shared.JobStatus PollJobStatus(ZendeskApi_v2.Models.Shared.JobStatus job)
        {
            int sleep   = 2000;
            int retries = 0;
            while (!job.Status.Equals("completed") && retries < 7)
            {
                System.Threading.Thread.Sleep(sleep);
                job   = api.JobStatuses.GetJobStatus(job.Id).JobStatus;
                sleep = (sleep < 64000 ? sleep *= 2 : 64000);
                retries++;
            }

            return job;
        }
    }
}