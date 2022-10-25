using System;
using System.Collections.Generic;
using System.Linq;
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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private DataContext _context;
        private BaseMongoDb<Role, string> BaseMongoDb;
        private ILoggingService _logger;
        private IDbSettings _settings;

        IMongoCollection<Module> _collection;
        private IMenuService _menuService;


        public RoleService(
            IMenuService menuService,
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<Role, string>(_context.Roles);
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.RoleCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);

            _collection = context.Modules;
            _menuService = menuService;
        }

        public async Task<Role> Create(Role model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var checkName = _context.Roles.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var entity = new Role
            {
                Ten = model.Ten,
                Code = model.Code,
                ThuTu = model.ThuTu,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
                // ModifiedBy = CurrentUser.Id,
                // CreatedBy = CurrentUser.Id
            };

            var result = await BaseMongoDb.CreateAsync(entity);
            
            if (result.Entity.Id == default || !result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);


            await UpdateRolesInUser(entity);
            return entity;
        }

        public async Task<Role> Update(Role model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.Roles.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var checkName = _context.Roles.Find(x => x.Id != model.Id && x.Ten == model.Ten && x.IsDeleted != true)
                .FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);

            entity.Ten = model.Ten;
            entity.Code = model.Code;
            entity.Permissions = model.Permissions;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);

            // Cập nhật quyền toàn user.
            await UpdateRolesInUser(entity);
            return entity;
        }

        private async Task UpdateRolesInUser(Role role)
        {
            var filter = Builders<User>.Filter
                .ElemMatch(z => z.Roles, a => a.Id == role.Id);
            var userUseRole = await _context.Users.Find(filter).ToListAsync();
            if (userUseRole != default)
            {
                foreach (var item in userUseRole)
                {
                    var index = item.Roles.FindIndex(x => x.Id == role.Id);
                    if (index != -1)
                    {
                        var filterUser = Builders<User>.Filter.Eq(x => x.Id, item.Id);
                        var update = Builders<User>.Update
                            .Set(s => s.Roles[index], role)
                            .Set(s => s.ModifiedBy, CurrentUser.UserName)
                            .Set(s => s.ModifiedAt, DateTime.Now);

                        UpdateResult actionResult
                            = await _context.Users.UpdateOneAsync(filterUser, update);
                    }
                }
            }
        }

        public async Task Delete(string id)
        {
            if (id == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.Roles.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            entity.ModifiedAt = DateTime.Now;
            entity.IsDeleted = true;

            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
        }

        public async Task<IEnumerable<Role>> Get()
        {
            var data = (await _context.Roles.Find(x => x.IsDeleted != true).ToListAsync())
                .Select(x => new Role(x));
            return data;
        }

        public async Task<Role> GetById(string id)
        {
            return await _context.Roles.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<Role>> GetPaging(RoleParam param)
        {
            PagingModel<Role> result = new PagingModel<Role>();
            var builder = Builders<Role>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = "Code";
            result.TotalRows = await _context.Roles.CountDocumentsAsync(filter);
            result.Data = await _context.Roles.Find(filter)
                .Sort(false
                ? Builders<Role>
                .Sort.Ascending(sortBy)    
                : Builders<Role>
                        .Sort.Descending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<List<NavMenuVM>> GetMenuForUser(string userName)
        {
            var result = new List<NavMenuVM>();
            var user = _context.Users.Find(x => x.UserName == userName).FirstOrDefault();
            if (user != default)
            {
                var permissionsView = user.Roles.SkipWhile(x => x.Permissions == null)
                    .SelectMany(x => x.Permissions)?
                    .Where(x => x.Action != null && x.Action.Contains("viewpage"))
                    .Select(p => p.Action)
                    .Distinct()
                    .ToList();

                if (permissionsView.Count > 0)
                {
                    var listMenus = await _context.Menu.Find(x => permissionsView.Contains(x.Action)).ToListAsync();
                    if (listMenus.Count > 0)
                    {
                        result = await _menuService.GetTreeListMenuForCurrentUser(listMenus);
                    }
                }
            }

            return result;
        }

        public async Task<List<string>> GetPermissionForCurrentUer(string userName)
        {
            var currentUser = await _context.Users.Find(x => x.UserName == userName && x.IsDeleted != true)
                .FirstOrDefaultAsync();
            if (currentUser == null)
                return new List<string>();
            else
            {
                if (currentUser.Roles == null)
                    return new List<string>();
                var permissions = currentUser.Roles
                    .Where(x => x.Permissions != null)
                    .SelectMany(x => x.Permissions)
                    .Select(x => x.Action)
                    .Distinct()
                    .ToList();
                return permissions;
            }
        }
    }
}