using task_management.IRepositories;
using task_management.Models;

namespace task_management.Services
{
    public class TaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tasks>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Repository<Tasks>().GetAllAsync();
            return projects;
        }


        public async Task AddTaskAsync(Tasks task)
        {
            _unitOfWork.Repository<Tasks>().Add(task);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateProjectAsync(Tasks project)
        {
            _unitOfWork.Repository<Tasks>().Update(project);
            await _unitOfWork.CompleteAsync();
        }

        /// <summary>
        ///     Get all tasks of a staff
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of tasks</returns>

        public async Task<IEnumerable<Tasks>> GetTasksByUserId(string userId, int pageNumber = 1, int pageSize = 6)
        {
            var tasks = await _unitOfWork.TaskRepository.GetTasksByUserId(userId, pageNumber, pageSize);
            await _unitOfWork.CompleteAsync();
            return tasks;
        }

        public async Task<IEnumerable<Tasks>> GetTasksByUserId(string userId)
        {
            var tasks = _unitOfWork.TaskRepository.GetTasksByUserId(userId);
            await _unitOfWork.CompleteAsync();
            return tasks;
        }

        public async Task<double> GetTotalUserTask(string userId)
        {
            return await _unitOfWork.TaskRepository.GetTotalUserTask(userId);
        }


        /// <summary>
        /// Get all tasks of a project with pagination
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="pageNumber">The page number to fetch</param>
        /// <param name="pageSize">The number of tasks per page</param>
        /// <returns>A paginated list of tasks</returns>

        public async Task<IEnumerable<Tasks>> GetTasksInProjects(int projectId, int pageNumber, int pageSize, string? status = null, string? priority = null, string? assignee = null)
        {
            var listOfTasks = await _unitOfWork.TaskRepository.GetTasksByProjectId(projectId, pageNumber, pageSize, status, priority, assignee);
            return listOfTasks;
        }

        public async Task<List<Tasks>> SearchTaskByName(string taskName)
        {
            var tasks = await _unitOfWork.TaskRepository.SearchTasksByName(taskName);
            return tasks;
        }

        public int GetTotalTasks(int projectId)
        {
            return _unitOfWork.TaskRepository.GetTotalTaskInProject(projectId);
        }


        public async Task InActiveTaskAsync(Tasks task)
        {
            _unitOfWork.TaskRepository.InActive(task);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Tasks> GetTaskByIdAsync(int id)
        {
            return await _unitOfWork.TaskRepository.GetTaskById(id);
        }


    }
}
