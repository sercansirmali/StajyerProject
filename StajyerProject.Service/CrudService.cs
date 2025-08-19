using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StajyerProject.Core.DTO;
using StajyerProject.Data.Repository;

namespace StajyerProject.Service
{
 
    public class CrudService
    {
        private readonly CrudRepository _crudRepository;
        public CrudService(CrudRepository crudRepository)
        {
            _crudRepository = crudRepository;
        }

        public async Task<int> AddMesajAsync(MesajResponse model)
        {
            return await _crudRepository.AddMesajAsyncDapper(model);
        }
    }
}
