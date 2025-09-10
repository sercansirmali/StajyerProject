using Microsoft.EntityFrameworkCore;
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

        public async Task<ApiResponse<bool>> UpdateMesajEfAsync(MesajRequest request)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var success = await _crudRepository.UpdateMesajEfAsync(request);

                response.Success = success;
                response.Data = success;
                response.Message = success ? "Güncelleme başarılı." : "Güncelleme yapılamadı.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Güncelleme sırasında hata oluştu.";
                response.Errors.Add(ex.Message);
                if (ex.InnerException != null)
                    response.Errors.Add("INNER: " + ex.InnerException.Message);
            }

            return response;
        }
        public async Task<int> AddMesajAsync(MesajResponse model)
        {
            return await _crudRepository.AddMesajAsyncDapper(model);
        }

        public async Task<ApiResponse<int>> CreateWithSpEfAsync(MesajRequest request)
        {
            var response = new ApiResponse<int>();

            try
            {
                var newId = await _crudRepository.AddMesajWithSpEfAsync(request);
                response.Success = true;
                response.Data = newId;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "EF Stored Procedure ile ekleme hatası.";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<ApiResponse<int>> CreateWithSpDapperAsync(MesajRequest request)
        {
            var response = new ApiResponse<int>();

            try
            {
                var newId = await _crudRepository.AddMesajWithSpDapperAsync(request);
                response.Success = true;
                response.Data = newId;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Dapper Stored Procedure ile ekleme hatası.";
                response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<bool> UpdateMesajAsync(MesajRequest request)
        {
            var affected = await _crudRepository.UpdateMesajAsync(request);
            return affected > 0;
        }

        public async Task<ApiResponse<int>> AddMesajAsyncAdoNet(MesajRequest request)
        {
            var response = new ApiResponse<int>();
            try
            {
                var newId = await _crudRepository.AddMesajAsyncAdoNet(request);
                response.Success = true;
                response.Data = newId;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "ADO.NET ile mesaj ekleme sırasında hata oluştu.";
                response.Errors.Add(ex.Message);
                if (ex.InnerException != null)
                    response.Errors.Add("INNER: " + ex.InnerException.Message);
            }

            return response;
        }

        public async Task<int> AddDosyaAsync(DosyaResponse dosya)
        {
            return await _crudRepository.AddDosyaAsync(dosya);
        }

        /// <summary>
        /// Mesajları listeler (Repository'deki GetAllAsyncEntities'i kullanır)
        /// </summary>
        public async Task<ApiResponse<List<MesajResponse>>> GetAllAsyncEntities(CancellationToken ct = default)
        {
      
            var result = await _crudRepository.GetAllAsyncEntities();

            // Örn: Tarih'e göre sıralamak istersen:
            if (result.Success && result.Data is not null)
            {
                result.Data = result.Data
                    .OrderByDescending(x => x.Tarih) // MesajResponse içinde Tarih var
                    .ToList();
            }

            return result;
        }
    }
}
