using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StajyerProject.Core.DTO;
using StajyerProject.Core.Entity;
//using StajyerProject.Data.Context;
using Dapper;
using StajyerProject.Data.Context;

namespace StajyerProject.Data.Repository
{
    public class CrudRepository
    {
        private readonly AppDbContext _dbContext;

        public CrudRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<MesajResponse>> CreateAsyncEntity(MesajRequest request)
        {
            var response = new ApiResponse<MesajResponse>();
            try
            {
                var entity = new MesajResponse
                {
                    Ad = request.Ad,
                    Soyad = request.Soyad,
                    Tip = request.Tip,
                    MesajMetni = request.MesajMetni,
                    Durum = request.Durum,
                    Tarih = request.Tarih ?? DateTime.Now
                };
                await _dbContext.Set<MesajResponse>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Data = entity;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while creating the message.";
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<ApiResponse<MesajResponse>> UpdateAsyncEntity(MesajRequest request)
        {
            var response = new ApiResponse<MesajResponse>();
            try
            {
                var entity = await _dbContext.Set<MesajResponse>().FindAsync(request.id);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "Message not found.";
                    return response;
                }
                entity.Ad = request.Ad;
                entity.Soyad = request.Soyad;
                entity.Tip = request.Tip;
                entity.MesajMetni = request.MesajMetni;
                entity.Durum = request.Durum;
                entity.Tarih = request.Tarih ?? DateTime.Now;
                _dbContext.Set<MesajResponse>().Update(entity);
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Data = entity;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating the message.";
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<ApiResponse<MesajResponse>> DeleteAsyncEntity(int id)
        {
            var response = new ApiResponse<MesajResponse>();
            try
            {
                var entity = await _dbContext.Set<MesajResponse>().FindAsync(id);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "Message not found.";
                    return response;
                }
                _dbContext.Set<MesajResponse>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                response.Success = true;
                response.Data = entity;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while deleting the message.";
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<ApiResponse<List<MesajResponse>>> GetAllAsyncEntities()
        {
            var response = new ApiResponse<List<MesajResponse>>();
            try
            {
                var entities = await _dbContext.Set<MesajResponse>().ToListAsync();
                response.Success = true;
                response.Data = entities;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving messages.";
                response.Errors.Add(ex.Message);
            }
            return response;
        }
        public async Task<ApiResponse<MesajResponse>> GetByIdAsyncEntity(int id)
        {
            var response = new ApiResponse<MesajResponse>();
            try
            {
                var entity = await _dbContext.Set<MesajResponse>().FindAsync(id);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "Message not found.";
                    return response;
                }
                response.Success = true;
                response.Data = entity;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving the message.";
                response.Errors.Add(ex.Message);
            }
            return response;
        }

        public async Task<int> AddMesajAsyncDapper(MesajResponse yeniMesaj)
        {
            var query = @"
        INSERT INTO StajyerDb.dbo.dt_mesaj (ad, soyad, tip, mesaj, durum, tarih)
        VALUES (@Ad, @Soyad, @Tip, @MesajMetni, @Durum, ISNULL(@Tarih, GETDATE()));
        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, yeniMesaj);
        }


    }
}
