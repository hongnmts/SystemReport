// using System.Collections.Generic;
// using System.Threading.Tasks;
// using SystemReport.WebAPI.Data;
// using SystemReport.WebAPI.Enums;
// using SystemReport.WebAPI.Extensions;
// using SystemReport.WebAPI.Interfaces;
//
// using Microsoft.OpenApi.Extensions;
// using SystemReport.WebAPI.Models;
// using Microsoft.AspNetCore.Http;
// using MongoDB.Bson;
// using MongoDB.Driver;
// using StringExtensions = SystemReport.WebAPI.Extensions.StringExtensions;
//
// namespace SystemReport.WebAPI.Services
// {
//     public class HistoryVanBanDenService : BaseService
//     {
//         private DataContext _context;
//         private BaseMongoDb<HistoryQuestion, string> BaseMongoDb;
//         private IMongoCollection<HistoryQuestion> _collection;
//         private IDbSettings _settings;
//         private ILoggingService _logger;
//
//         public HistoryVanBanDenService(ILoggingService logger, IDbSettings settings, DataContext context,
//             IHttpContextAccessor contextAccessor)
//             : base(context, contextAccessor)
//         {
//             _context = context;
//             BaseMongoDb = new BaseMongoDb<HistoryQuestion, string>(_context.HistoryQuestions);
//             _collection = context.HistoryQuestions;
//             _settings = settings;
//             _logger = logger.WithCollectionName(_settings.HistoryQuestionCollectionName)
//                 .WithDatabaseName(_settings.DatabaseName)
//                 .WithUserName(CurrentUserName);
//             history = new HistoryQuestion();
//             history.CreatedBy = CurrentUserName;
//         }
//
//         public async Task<List<HistoryQuestion>> GetHistoryByQuestionId(string questionId)
//         {
//             return await _collection.Find(x => x.QuestionId == questionId).SortByDescending(x => x.ModifiedAt).ToListAsync();
//         }
//         public async Task<bool> SaveChangeHistoryQuestion()
//         {
//             if (history == default)
//                 return false;
//             Hash();
//             history.Id = BsonObjectId.GenerateNewId().ToString();
//             var result = await BaseMongoDb.CreateAsync(history);
//             if (result.Entity.Id == default || !result.Success)
//                 return false;
//             return true;
//         }
//
//         public HistoryVanBanDenService WithTitle(string title)
//         {
//             if (!string.IsNullOrEmpty(title))
//             {
//                 history.Title = title;
//             }
//
//             return this;
//         }
//         public HistoryVanBanDenService WithStatus(StatusQuestion status)
//         {
//             if (status != default)
//             {
//                 history.StatusQuestion = status;
//             }
//
//             return this;
//         }
//         public HistoryVanBanDenService WithUserName(string userName)
//         {
//             if (!string.IsNullOrEmpty(userName))
//             {
//                 var user = _context.Users.Find(x => x.UserName == userName).FirstOrDefault();
//                 if (user == default)
//                     return this;
//
//                 history.UserName = user.UserName;
//                 history.UserFullName = user.FullName;
//             }
//
//             return this;
//         }
//
//         public HistoryVanBanDenService WithDepartment(string departmentId)
//         {
//             if (!string.IsNullOrEmpty(departmentId))
//             {
//                 var entity = _context.DonVis.Find(x => x.Id == departmentId).FirstOrDefault();
//                 if (entity == default)
//                     return this;
//
//                 history.DepartmentId = entity.Id;
//                 history.DepartmentName = entity.Ten;
//             }
//
//             return this;
//         }
//         public HistoryVanBanDenService WithQuestionId(string questionId)
//         {
//             if (!string.IsNullOrEmpty(questionId))
//             {
//                 history.QuestionId = questionId;
//             }
//
//             return this;
//         }
//         public HistoryVanBanDenService WithType(ETypeHistory type, dynamic oldValue)
//         {
//             if (!string.IsNullOrEmpty(type.GetDisplayName()))
//             {
//                 history.TypeHistory = type.GetDisplayName();
//                 history.OldValue = oldValue;
//             }
//
//             return this;
//         }
//         public HistoryVanBanDenService WithAction(EAction action)
//         {
//             history.Action = action.GetDisplayName();
//
//             return this;
//         }
//         public HistoryVanBanDenService WithAction(string action)
//         {
//             history.Action = action;
//
//             return this;
//         }
//         public HistoryVanBanDenService WithHistoryAction(HistoryAction action)
//         {
//             if (action != default)
//             {
//                 history.HistoryAction = action;
//             }
//
//             return this;
//         }
//
//         private void Hash()
//         {
//             var hash = StringExtensions.SHA256(history.CreatedAt.ToString() + history.CreatedBy + history.Id);
//             history.Hash = hash;
//             var pre = _collection.Find(x => x.QuestionId == history.QuestionId)
//                 .SortByDescending(x => x.CreatedAt).FirstOrDefault();
//             if (pre != default)
//                 history.ReferenceHash = pre.Hash;
//         }
//         #region Properties
//
//         private HistoryQuestion history;
//
//         #endregion
//     }
// }