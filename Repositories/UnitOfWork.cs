using task_management.Data;
using task_management.IRepositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace task_management.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private IProjectAssignment _projectManagement;
        private ITaskRepository _taskRepository;
        private IProjectRepository _projectRepository;

        public UnitOfWork(ApplicationDbContext context, IProjectAssignment projectManagement, ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _context = context;
            _projectManagement = projectManagement;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<TEntity>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public IProjectAssignment ProjectAssignmentRepository
        {
            get
            {
                return _projectManagement ??= new ProjectAssignmentRepository(_context);
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                return _taskRepository ??= new TaskRepository(_context);    
            }
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                return _projectRepository ??= new ProjectRepository(_context);
            }
        }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
