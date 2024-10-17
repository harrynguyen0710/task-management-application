using task_management.IRepositories;
using System.Threading.Tasks;

namespace task_management.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;   
        }
        /*
        public async Task<int> GetTotalTasksAsync()
        {
            return await _taskRepository.GetTotalTasksAsync();
        }

        public async Task<int> FilterTask(string status)
        {
            return await _taskRepository.FilterTask(status);
        }

        public async Task<int> GetCompletedTasksAsync()
        {
            return await _taskRepository.FilterTask("Completed");
        }

        public async Task<int> GetTotalEmployeesAsync()
        {
            return await _taskRepository.GetTotalEmployeesAsync();
        }

        public async Task<double> GetCompletionRateAsync()
        {
            var totalTasks = await GetTotalTasksAsync();
            var completedTasks = await GetCompletedTasksAsync();
            return totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0;
        }
        */
    }

}
