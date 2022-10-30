using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> Create(RefreshToken model);
        Task<RefreshToken> Update(RefreshToken model);
        Task Delete(string id);


        Task<IEnumerable<RefreshToken>> GetAllAsync();
        Task<RefreshToken> GetById(string token);
        Task<RefreshToken> GetByUserId(string userId);
        Task<RefreshToken> GetByJwtId(string jwtId);
    }
}