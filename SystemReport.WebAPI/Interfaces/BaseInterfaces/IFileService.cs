using System.Threading.Tasks;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IFileService
    {
        File GetById(string id);
        Task<File> SaveFileAsync(string filePath, string fileName, string newFileName, string fileExt, long fileSize);
        Task<File> SaveFileAsync(string fileId, string filePath, string fileName, string newFileName, string fileExt, long fileSize);
    }
}