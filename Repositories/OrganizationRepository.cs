using task_management.IRepositories;
using task_management.Models;
using task_management.Data;
namespace task_management.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
