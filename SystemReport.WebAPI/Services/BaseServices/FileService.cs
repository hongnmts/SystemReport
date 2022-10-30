using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.Identity;
using SystemReport.WebAPI.Models;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class FileService : BaseService, IFileService
    {
        private DataContext _context;
        private BaseMongoDb<File, string> BaseMongoDb;
        private ILoggingService _logger;
        private IDbSettings _settings;

        IMongoCollection<File> _collection;

        public FileService(
            IMenuService menuService,
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<File, string>(_context.Files);
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.FileCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _collection = context.Files;
        }


        public File GetById(string id)
        {
            return _collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public async Task<File> SaveFileAsync(string filePath, string FileName, string newFileName,
            string fileExt, long fileSize)
        {
            var entity = new File();
            entity.FileName = FileName;
            entity.SaveName = newFileName;
            entity.Path = filePath;
            entity.Size = fileSize;
            entity.Ext = fileExt;
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;
            //        await _collection.InsertOneAsync(entity);

            var result = await BaseMongoDb.CreateAsync(entity);

            if (result.Entity.Id == default || !result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);

            return entity;
        }

        public Task<File> SaveFileAsync(string fileId, string filePath, string fileName, string newFileName, string fileExt, long fileSize)
        {
            throw new NotImplementedException();
        }
    }
}