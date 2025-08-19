using StajyerProject.Core.DTO;
using StajyerProject.Core.Entity;
using StajyerProject.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajyerProject.Service
{
 
    public class CrudService
    {
        private readonly CrudRepository _crudRepository;
        public CrudService(CrudRepository crudRepository)
        {
            _crudRepository = crudRepository;
        }

        public async Task<ApiResponse<MesajResponse>> CreateMesajAsync(MesajRequest request)
        {
            return await _crudRepository.CreateAsyncEntity(request);
        }

        public async Task<int> AddMesajAsync(MesajResponse model)
        {
            return await _crudRepository.AddMesajAsyncDapper(model);
        }
    }
}
