using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using task_management.Data;
using task_management.IRepositories;
using task_management.Models;

namespace task_management.Repositories
{
    public class ProjectAssignmentRepository : Repository<ProjectAssignment>, IProjectAssignment
    {
        public ProjectAssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Project> GetProjectsByUserId(string userId)
        {
            return _context.ProjectAssignments
                .Where(u => u.userId == userId)
                .Select(p => p.Project)
                .ToList();
        }
        public List<Users> GetTeamMembersByProject(int projectId)
        {
            return _context.ProjectAssignments
                .Where(p => p.projectId == projectId)
                .Select(u => u.User)
                .ToList();
        }
        public async Task<ProjectAssignment> FindAsync(Expression<Func<ProjectAssignment, bool>> predicate)
        {
            return await _context.ProjectAssignments.FirstOrDefaultAsync(predicate);
        }
    }
}
