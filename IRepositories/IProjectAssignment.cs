using Microsoft.EntityFrameworkCore;
using task_management.Models;

namespace task_management.IRepositories
{
    public interface IProjectAssignment : IRepository<ProjectAssignment>
    {
        public List<ProjectAssignment> GetTeamMembersByProject(int id);
        public List<ProjectAssignment> GetProjectsByUserId(string userId); 
/*        public Task<int> GetTotal
*/    
    }
}
