using System.Collections.Generic;
using System.Threading.Tasks;
using task_management.Models;
using task_management.IRepositories;

namespace task_management.Services
{
    public class OrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            var organizationRepository = _unitOfWork.Repository<Organization>();
            return await organizationRepository.GetAllAsync();
        }

        //public async Task UpdateOrganization(
    }
}
