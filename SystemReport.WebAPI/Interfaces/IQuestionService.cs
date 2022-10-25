using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public  interface IQuestionService
    {
        Task<Question> Create(Question model);  
        Task<Question> Update(Question model);
        Task Delete(string id);
        Task<List<Question>> Get();
        Task<Question> GetById(string id);
        Task<PagingModel<Question>> GetPaging(QuestionParamFilter param);
        Task ChangeStatusQuestion(StatusQuestion model);
        
        Task<PagingModel<Question>>   GetPagingReceive (QuestionParamFilter param);
        Task<PagingModel<Question>>   GetPagingHandle (QuestionParamFilter param);
        
        Task<Question> QuestionNavigation(Question model);    
        Task<Question> NotApprove(Question model);    
    }
}