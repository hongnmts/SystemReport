using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.Identity;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class RefreshTokenService : BaseService, IRefreshTokenService
    {
        private DataContext _context;
        private BaseMongoDb<RefreshToken, string> BaseMongoDb;
        private ILoggingService _logger;
        private IDbSettings _settings;

        IMongoCollection<RefreshToken> _collection;
        
        public RefreshTokenService(
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<RefreshToken, string>(_context.RefreshToken);
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.RefreshTokenCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);

            _collection = context.RefreshToken;
        }

        public async Task<RefreshToken> Create(RefreshToken model)
        {
            if (model == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var result = await BaseMongoDb.CreateAsync(model);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }
            return model;
        }

        public async Task<RefreshToken> Update(RefreshToken model)
        {
            if (model == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var result = await BaseMongoDb.UpdateAsync(model);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }
            return model;
        }

        public async Task Delete(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var result = await BaseMongoDb.DeleteByIdsync(id);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }
        }

        public async Task<IEnumerable<RefreshToken>> GetAllAsync()
        {
            return await _context.RefreshToken.Find(_ => true).ToListAsync();
        }

        public async Task<RefreshToken> GetById(string id)
        {
            return await _context.RefreshToken.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RefreshToken> GetByUserId(string userId)
        {
            return await _context.RefreshToken.Find(x => x.UserId == userId)
                .SortByDescending(x => x.ExpiryDate)
                .FirstOrDefaultAsync();
        }

        public async Task<RefreshToken> GetByJwtId(string jwtId)
        {
            return await _context.RefreshToken.Find(x => x.JwtId == jwtId).FirstOrDefaultAsync();
        }
    }
}