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
            var tasks =  _unitOfWork.TaskRepository.GetTasksByUserId(userId);
            await _unitOfWork.CompleteAsync();
            return tasks;
        }

        /// <summary>
        /// Get all task of a project all joint staff id (staff joint project)
        /// </summary>
        /// <param name="assignments"></param>
        /// <returns>A list of tasks</returns>
        public async Task<IEnumerable<Tasks>> GetTasksInProjects(List<ProjectAssignment> assignments)
        {
            var listOfTasks = new HashSet<Tasks>(); 

            // Collect all tasks for each user in parallel 
            var taskLists = await Task.WhenAll(assignments.Select(a => GetTasksByUserId(a.userId)));

            // Flatten the taskLists and add all tasks to the HashSet 
            listOfTasks.UnionWith(taskLists.SelectMany(t => t));

            await _unitOfWork.CompleteAsync();
            return listOfTasks;
        }

        public async Task<Tasks> GetTaskByIdAsync(int id)
        {   
            return await _unitOfWork.TaskRepository.GetTaskById(id);   
        }


    }
}
