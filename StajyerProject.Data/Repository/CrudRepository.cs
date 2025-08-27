//using StajyerProject.Data.Context;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StajyerProject.Core.DTO;
using StajyerProject.Core.Entity;
using StajyerProject.Data.Context;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajyerProject.Data.Repository
{
    public class CrudRepository
    {
        private readonly AppDbContext _dbContext;

        public CrudRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Entity Framework Core CRUD Örnekleri
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

                // 🟥 Inner Exception'ı da ekle
                if (ex.InnerException != null)
                    response.Errors.Add("INNER: " + ex.InnerException.Message);
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


        //StoredProcedure entity framework core ile örnek

        public async Task<int> AddMesajWithSpEfAsync(MesajRequest request)
        {
            var newIdParam = new SqlParameter
            {
                ParameterName = "@NewId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_dt_mesaj_Insert @Ad, @Soyad, @Tip, @MesajMetni, @Durum, @Tarih, @NewId OUT",
                new SqlParameter("@Ad", request.Ad ?? (object)DBNull.Value),
                new SqlParameter("@Soyad", request.Soyad ?? (object)DBNull.Value),
                new SqlParameter("@Tip", request.Tip ?? (object)DBNull.Value),
                new SqlParameter("@MesajMetni", request.MesajMetni ?? (object)DBNull.Value),
                new SqlParameter("@Durum", request.Durum ?? (object)DBNull.Value),
                new SqlParameter("@Tarih", request.Tarih ?? (object)DBNull.Value),
                newIdParam
            );

            return (int)newIdParam.Value;
        }

        public async Task<bool> UpdateMesajEfAsync(MesajRequest request)
        {
            var parameters = new[]
            {
            new SqlParameter("@id", request.id),
            new SqlParameter("@Ad", request.Ad ?? (object)DBNull.Value),
            new SqlParameter("@Soyad", request.Soyad ?? (object)DBNull.Value),
            new SqlParameter("@Tip", request.Tip ?? (object)DBNull.Value),
            new SqlParameter("@MesajMetni", request.MesajMetni ?? (object)DBNull.Value),
            new SqlParameter("@Durum", request.Durum ?? (object)DBNull.Value),
            new SqlParameter("@Tarih", request.Tarih ?? (object)DBNull.Value)
        };

            int affectedRows = await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC StajyerDb.dbo.sp_dt_mesaj_Update @id, @Ad, @Soyad, @Tip, @MesajMetni, @Durum, @Tarih",
                parameters);

            return affectedRows > 0;
        }



        //DAPPER
        public async Task<int> AddMesajAsyncDapper(MesajResponse yeniMesaj)
        {
            var query = @"
        INSERT INTO StajyerDb.dbo.dt_mesaj (ad, soyad, tip, mesajmetni, durum, tarih)
        VALUES (@Ad, @Soyad, @Tip, @MesajMetni, @Durum, ISNULL(@Tarih, GETDATE()));
        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, yeniMesaj);
        }


        //StoredProcedure dapper ile örnek
        public async Task<int> AddMesajWithSpDapperAsync(MesajRequest request)
        {
            using var connection = _dbContext.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Ad", request.Ad);
            parameters.Add("@Soyad", request.Soyad);
            parameters.Add("@Tip", request.Tip);
            parameters.Add("@MesajMetni", request.MesajMetni);
            parameters.Add("@Durum", request.Durum);
            parameters.Add("@Tarih", request.Tarih ?? DateTime.Now);
            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("dbo.sp_dt_mesaj_Insert", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewId");
        }

        public async Task<int> UpdateMesajAsync(MesajRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", request.id);
            parameters.Add("@Ad", request.Ad);
            parameters.Add("@Soyad", request.Soyad);
            parameters.Add("@Tip", request.Tip);
            parameters.Add("@MesajMetni", request.MesajMetni);
            parameters.Add("@Durum", request.Durum);
            parameters.Add("@Tarih", request.Tarih);

            using var connection = _dbContext.CreateConnection();
            return await connection.ExecuteAsync("StajyerDb.dbo.sp_dt_mesaj_Update", parameters, commandType: CommandType.StoredProcedure);
        }


        //Ado.net örnekleri
        public async Task<int> AddMesajAsyncAdoNet(MesajRequest mesaj)
        {
            var query = @"
            INSERT INTO StajyerDb.dbo.dt_mesaj (ad, soyad, tip, mesajmetni, durum, tarih)
            VALUES (@Ad, @Soyad, @Tip, @MesajMetni, @Durum, ISNULL(@Tarih, GETDATE()));
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var connection = (SqlConnection)_dbContext.CreateConnection())
            {
                await connection.OpenAsync();
       

            using var command = new SqlCommand(query, (SqlConnection)connection);
            command.Parameters.AddWithValue("@Ad", mesaj.Ad ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Soyad", mesaj.Soyad ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Tip", mesaj.Tip ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MesajMetni", mesaj.MesajMetni ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Durum", mesaj.Durum ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Tarih", mesaj.Tarih ?? (object)DBNull.Value);

            var insertedId = (int)(await command.ExecuteScalarAsync());
            return insertedId;
            }
        }

        public async Task<List<DosyaResponse>> GetDosyalarAsync()
        {
            const string query = @"SELECT id, dosya, adres, tarih FROM StajyerDb.dbo.dt_dosya";

            using var connection = _dbContext.CreateConnection();
            var result = await connection.QueryAsync<DosyaResponse>(query);
            return result.ToList();
        }

        public async Task<int> AddDosyaAsync(DosyaResponse dosya)
        {
            var query = @"
        INSERT INTO dt_dosya (dosya, adres, tarih)
        VALUES (@Dosya, @Adres, @Tarih);
        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, dosya);
        }

    }


}
