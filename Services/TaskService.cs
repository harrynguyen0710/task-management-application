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
        public async Task<IEnumerable<Tasks>> GetTasksByUserId(string userId)
        {
            var tasks = _unitOfWork.TaskRepository.GetTasksByUserId(userId);
            await _unitOfWork.CompleteAsync();
            return tasks;
        }

        /// <summary>
        /// Get all tasks of a project, joint by staff id (staff joint project) with pagination
        /// </summary>
        /// <param name="assignments"></param>
        /// <param name="pageNumber">The page number to fetch</param>
        /// <param name="pageSize">The number of tasks per page</param>
        /// <returns>A paginated list of tasks</returns>
        public async Task<IEnumerable<Tasks>> GetTasksInProjects(List<ProjectAssignment> assignments, int pageNumber, int pageSize)
        {
            var listOfTasks = new HashSet<Tasks>();

            // Collect tasks for each user sequentially to avoid DbContext concurrency issues
            foreach (var assignment in assignments)
            {
                var userTasks = await GetTasksByUserId(assignment.userId);
                listOfTasks.UnionWith(userTasks);
            }

            // Apply pagination
            var paginatedTasks = listOfTasks
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            await _unitOfWork.CompleteAsync();
            return paginatedTasks;
        }

        public int GetTotalTasks(int projectId)
        {
            var assignments = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);
            var count = 0;

            foreach (var assignment in assignments)
            {
                count += _unitOfWork.TaskRepository.GetTotalTask(assignment.userId);
            }

            return count;
        }


        public async Task InActiveTaskAsync(Tasks task)
        {
            _unitOfWork.TaskRepository.InActive(task);
            await _unitOfWork.CompleteAsync();
        }

        /*   public async Task GetTasksByUserId(string userId)
           {

           }*/

        public async Task<Tasks> GetTaskByIdAsync(int id)
        {
            return await _unitOfWork.TaskRepository.GetTaskById(id);
        }
        public async Task<IEnumerable<Tasks>> GetTasksByProjectId(int projectId)
        {
            var assignments = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);

            var tasksInProject = new List<Tasks>();

            foreach (var assignment in assignments)
            {
                var userTasks = await GetTasksByUserId(assignment.userId);
                tasksInProject.AddRange(userTasks);
            }

            return tasksInProject;
        }

    }
}
