using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(User model);
        Task<User> Update(User model);
        Task Delete(string id);
        Task<IEnumerable<User>> Get();
        Task<User> GetById(string id);
        Task<User> GetUserByIdDonVi(string id);
        Task<PagingModel<User>> GetPaging(PagingParam param);
        Task<User> GetByUserName(string userName);  
        Task<User> ChangePassword(UserVM model);
        Task<User> ChangeProfile(User model);
        Task<User> FindUserWithUserNameOrPhoneNumber(string input);
        Task ReadDataUser(string filePath);
        Task<List<DonViTreeMail>> UserTreeForDonVi();

        Task UpdateSignature(SignatureSaveVM model);
        Task<List<SignatureSave>> GetSignature(string userName);
    }
}