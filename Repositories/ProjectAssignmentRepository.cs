﻿using System.Data.Entity;
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

        public List<ProjectAssignment> GetProjectsByUserId(string userId)
        {
            return _context.ProjectAssignments
                .Where(u => u.userId == userId)
                .ToList();
        }
        public List<ProjectAssignment> GetTeamMembersByProject(int projectId)
        {
            return _context.ProjectAssignments
                .Where(p => p.projectId == projectId)
                .ToList(); 
        }
    }
}
