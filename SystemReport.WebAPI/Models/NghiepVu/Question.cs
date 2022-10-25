using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using SystemReport.WebAPI.Extensions;

namespace SystemReport.WebAPI.Models
{
    public class Question : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Code { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Content { get; set; }
        public StatusQuestion LastedStatus { get; set; } = new StatusQuestion();

        public bool IsPrivate { get; set; } = false;
        public string UserId { get; set; }
        public string UserName { get; set; }

        [BsonIgnore]
        public string UserNameShow
        {
            get { return UserName;  }
            // get { return UserName.HideString(); }
        }
        //for guess information
        [BsonIgnore] public bool IsGuess { get; set; }
        [BsonIgnore] public string PhoneNumber { get; set; }
        
        public List<AnswerModel> Answers { get; set; }
        
        // public string HuyenId { get; set; }
        // public string HuyenName { get; set; }
        public HuyenShort Huyen { get; set;  }
        
        public string IdOwner { get; set;  }
        
        [BsonIgnore]
        public bool isShow { get; set; } = false;
        
        public  XaShort Xa { get; set;  }
        // public string XaId { get; set; }
        // public string XaName { get; set; }
        
        public  LinhVucShort LinhVuc { get; set;  }
        // public string LinhVucId { get; set; }  
        // public string LinhVucName { get; set; }
        public string Address { get; set; }
        public List<FileShort> FileManagers { get; set; } = new List<FileShort>();
        

        public List<FileShort> FileImage { get; set; } = new List<FileShort>();
        
        public List<FileShort> FilSystemReport { get; set; } = new List<FileShort>();

        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; } = new List<FileShort>();
        
        public DonViShort DonVi { get; set;  } 
        
        public  string Note { get; set;  }

        // [BsonIgnore] 
        // public BrowsingStatus  browsingStatus { get; set; }  
        // public List<FileShort> UploadFilesGiaiNgan { get; set; }
    }

    // public static class DefineStatus
    // {
    //     // public const int DaTraLoiNguoiDan = 100;
    //     // public const int VuaTiepNhan = 5;
    //     // public const int DangXuLy = 1;
    //     // public const int ChoDuyet = 0;
    //     // public const int DaXuLyXong = 99;
    //     // public const int DaHuy = -1;
    //     
    //     public const int KhongTiepNhan = -1; // Khi quản trị hệ thống vấn đề không nằm trong phạm vị xử lý nên không tiếp nhận
    //     public const int ChoDuyet = 0; // Khi người dân vừa tạo phản ánh kiến nghị thì vào trạng thái chờ duyệt
    //     public const int VuaTiepNhan = 1; // Sau khi quản trị viên hệ thống xem phản ánh và chấp nhận phản ánh thì vào trạng thái vừa tiếp nhận. 
    //     public const int DangXuLy = 2; // Sau khi quản trị viên hệ thống chuyển đến cơ quan mà cơ quan tiếp nhận thì chuyển sang trạng thái đang xử lý . 
    //     // Nếu cơ quan xử lý cảm thấy vấn đề không phù hợp từ chối tiếp nhận thì trạng thái vẫn ở trạng thái Vừa tiếp nhận.
    //     public const int DaXuLyXong = 3; // Sau khi cơ quan xử lý xong thì  sẽ qua trạng thái đã xử lý xong . 
    //     public const int DaTraLoiNguoiDan = 4; // Sau khi cơ quan nhấn phản hồi người dân thì sẽ qua trạng thái đã trả lời người dân
    //     
    //     public static string GetStatusString(int status)
    //     {
    //         string result = "";
    //         switch (status)
    //         {
    //             case KhongTiepNhan:
    //                     result = "Không tiếp nhận";
    //                 break;
    //             case ChoDuyet:
    //                 result = "Chờ duyệt";
    //                 break;
    //             case VuaTiepNhan:
    //                 result = "Vừa tiếp nhận";
    //                 break;
    //             case DangXuLy:
    //                 result = "Đang xử lý";
    //                 break;
    //             case DaXuLyXong:
    //                 result = "Đã xử lý xong";
    //                 break;
    //             case DaTraLoiNguoiDan:
    //                 result = "Đã trả lời người dân";
    //                 break;
    //             default:
    //                 break;
    //         }
    //
    //         return result;
    //     }
    //
    //     public static IEnumerable<int> GetAvailableStatuses()
    //     {
    //         return new List<int>
    //         {
    //             DangXuLy, DaTraLoiNguoiDan, VuaTiepNhan, DaXuLyXong
    //         };
    //     }
    // }

    public class AnswerModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Comment { get; set; }
        public string PathFile { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        [BsonIgnore]
        public string UserNameShow
        {
            get { return UserName.HideString(); }
        }

        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public FileShort FileManager { get; set; }
    }


    public enum EQAStatus
    {
        REQUESTED,
        RESPONCED,
        ANSWERED,
        ENDED,
        NONE
    }

    public class BrowsingStatus
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        
        public  string idTo { get; set;  }
        public string tenTo { get; set;  }
    }
    
    
}