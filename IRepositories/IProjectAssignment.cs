using System.Linq.Expressions;
using task_management.Models;

namespace task_management.IRepositories
{
    public interface IProjectAssignment : IRepository<ProjectAssignment>
    {
        public List<ProjectAssignment> GetTeamMembersByProject(int id);
        public List<ProjectAssignment> GetProjectsByUserId(string userId);
        Task<ProjectAssignment> FindAsync(Expression<Func<ProjectAssignment, bool>> predicate);

    }
}
