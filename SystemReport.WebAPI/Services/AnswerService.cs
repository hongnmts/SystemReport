// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using MongoDB.Bson;
// using MongoDB.Driver;
// using SystemReport.WebAPI.Data;
// using SystemReport.WebAPI.Enums;
// using SystemReport.WebAPI.Exceptions;
// using SystemReport.WebAPI.Extensions;
// using SystemReport.WebAPI.Helpers;
// using SystemReport.WebAPI.Interfaces;
// using SystemReport.WebAPI.Models;
// using SystemReport.WebAPI.Params;
// using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;
//
// namespace SystemReport.WebAPI.Services
// {
//     public class AnswerService : BaseService, IAnswerService
//     {
//         private DataContext _context;
//         private BaseMongoDb<Answer, string> BaseMongoDb;
//         private IMongoCollection<Answer> _collection;
//         private IDbSettings _settings;
//         private ILoggingService _logger;
//         private IQuestionService _question;
//         private HistoryQuestionService _history;
//         private List<String> filePicture = new List<string>() {".jpeg", ".jpg" , ".gif",".png"};
//         private List<String> filSystemReport = new List<string>() {".docx",".doc", ".csv" , ".xlsx",".pptx",".pdf"};
//         public AnswerService(HistoryQuestionService history, ILoggingService logger, IDbSettings settings, DataContext context, 
//             IQuestionService questionService, 
//             IHttpContextAccessor contextAccessor)
//             : base(context, contextAccessor)
//         {
//             _context = context;
//             BaseMongoDb = new BaseMongoDb<Answer, string>(_context.Answers);
//             _collection = context.Answers;
//             _settings = settings;
//             _question = questionService;
//             _logger = logger.WithCollectionName(_settings.QuestionCollectionName)
//                 .WithDatabaseName(_settings.DatabaseName)
//                 .WithUserName(CurrentUserName);
//             _history = history;
//         }
//
//         public async Task<Answer> Create(Answer model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = new Answer()
//             {
//                 Content = model.Content,
//                 ListLink = model.ListLink,
//                 UserName = CurrentUserName,
//                 FullName = CurrentUser.FullName,
//                 DepartmentId = CurrentUser.DonVi?.Id,
//                 DepartmentName = CurrentUser.DonVi?.Ten,
//                 QuestionId = model.QuestionId,
//                 ModifiedAt = DateTime.Now ,
//                 CreatedAt = DateTime.Now,
//                 CreatedBy = CurrentUserName,
//                 ModifiedBy = CurrentUserName
//             };
//             if (model.UploadFiles!= default && model.UploadFiles.Count > 0)
//             {
//                 foreach (var file in model.UploadFiles)
//                 {
//                     var newFile = new FileShort();
//                     newFile.FileId = file.FileId;
//                     newFile.FileName = file.FileName;
//                     newFile.Ext = file.Ext;
//                     if (filSystemReport.Contains(file.Ext))
//                     {
//                         entity.FilSystemReport.Add(newFile);
//                     }
//                     else if (filePicture.Contains(file.Ext))
//                     {
//                         entity.FileImage.Add(newFile);
//                     }
//                     entity.FileManagers.Add(newFile);
//                 }
//             }
//             var result = await BaseMongoDb.CreateAsync(entity);
//             if (result.Entity.Id == default || !result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.CREATE_FAILURE);
//             }
//             var status = new StatusQuestion();
//             status.StatusCode = model.Status.StatusCode;
//             status.QuestionId = entity.QuestionId;
//             await  _question.ChangeStatusQuestion(status);
//             var question = _context.Questions.Find(x => x.Id == entity.QuestionId).FirstOrDefault();
//             await _history.WithQuestionId(entity.QuestionId).WithUserName(entity.UserName)
//                 .WithStatus(question.LastedStatus)
//                 .WithAction(EAction.CREATE)
//                 .WithType(ETypeHistory.Answer, null)
//                 .WithTitle("Trả lời phản ánh/kiến nghị.")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//         public async Task<Answer> Update(Answer  model)
//         {
//             if (model == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//             var entity = _context.Answers.Find(x => x.Id == model.Id).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//             var oldValue = entity;
//             entity.Content = model.Content;
//             entity.ListLink = model.ListLink;
//             entity.UserName = CurrentUserName;
//             entity.FullName = CurrentUser.FullName;
//             entity.DepartmentId = CurrentUser.DonVi?.Id;
//             entity.DepartmentName = CurrentUser.DonVi?.Ten;
//             entity.ModifiedBy = CurrentUserName;
//             if (model.UploadFiles != default && model.UploadFiles.Count > 0)
//             {
//                 foreach (var file in model.UploadFiles)
//                 {
//                     var newFile = new FileShort();
//                     newFile.FileId = file.FileId;
//                     newFile.FileName = file.FileName;
//                     if (filSystemReport.Contains(file.Ext))
//                     {
//                         entity.FilSystemReport.Add(newFile);
//                     }
//                     else if (filePicture.Contains(file.Ext))
//                     {
//                         entity.FileImage.Add(newFile);
//                     }
//                     entity.FileManagers.Add(newFile);
//                 }
//             }
//             var result = await BaseMongoDb.UpdateAsync(entity);
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.UPDATE_FAILURE);
//             }
//             var question = _context.Questions.Find(x => x.Id == entity.QuestionId).FirstOrDefault();
//             
//             await _history.WithQuestionId(entity.QuestionId)
//                 .WithUserName(entity.UserName)
//                 .WithStatus(question.LastedStatus)
//                 .WithAction(EAction.UPDATE)
//                 .WithType(ETypeHistory.Answer, oldValue)
//                 .WithTitle("Cập nhật trả lời phản ánh/kiến nghị.")
//                 .SaveChangeHistoryQuestion();
//             return entity;
//         }
//
//         public async Task Delete(string id)
//         {
//             if (id == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
//             }
//
//
//             var entity = _context.Answers.Find(x => x.Id == id).FirstOrDefault();
//             if (entity == default)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DATA_NOT_FOUND);
//             }
//
//             var result = await BaseMongoDb.DeleteByIdsync(id);
//
//             if (!result.Success)
//             {
//                 throw new ResponseMessageException()
//                     .WithCode(EResultResponse.FAIL.ToString())
//                     .WithMessage(DefaultMessage.DELETE_FAILURE);
//             }
//
//             var question = _context.Questions.Find(x => x.Id == entity.QuestionId).FirstOrDefault();
//             await _history.WithQuestionId(entity.QuestionId).WithUserName(entity.UserName)
//                 .WithAction(EAction.DELETE)
//                 .WithStatus(question?.LastedStatus)
//                 .WithType(ETypeHistory.Answer, entity)
//                 .WithTitle("Xóa trả lời phản ánh/kiến nghị.")
//                 .SaveChangeHistoryQuestion();
//         }
//
//         public async Task<List<Answer>> Get(string questionId)
//         {
//             return await _context.Answers.Find(x => x.QuestionId == questionId)
//                 .ToListAsync();
//         }
//
//         public async Task<Answer> GetByIdQuestion(string id)
//         {
//             return await _context.Answers.Find(x => x.QuestionId == id)
//                 .FirstOrDefaultAsync();
//         }
//
//         public async Task<PagingModel<Answer>> GetPaging(AnswerParam param)
//         {
//             PagingModel<Answer> result = new PagingModel<Answer>();
//             var builder = Builders<Answer>.Filter;
//             var filter = builder.Empty;
//
//             filter = builder.And(filter, builder.Where(x => x.QuestionId == param.QuestionId));
//             if (!String.IsNullOrEmpty(param.Content))
//             {
//                 filter = builder.And(filter,
//                     builder.Where(x => x.Content.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
//             }
//
//             string sortBy = nameof(Answer.CreatedAt);
//             result.TotalRows = await _collection.CountDocumentsAsync(filter);
//             result.Data = await _collection.Find(filter)
//                 .Sort(param.SortDesc
//                     ? Builders<Answer>
//                         .Sort.Descending(sortBy)
//                     : Builders<Answer>
//                         .Sort.Ascending(sortBy))
//                 .Skip(param.Skip)
//                 .Limit(param.Limit)
//                 .ToListAsync();
//             return result;
//         }
//     }
// }
