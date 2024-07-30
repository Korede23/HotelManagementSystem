using HotelManagementSystem.Dto.ResponseModel;
using HotelManagementSystem.Model.Entity.Enum;

namespace HotelManagementSystem.Dto
{
    public class CustomerReviewDto
    {
        public string Comment { get; set; }
        public Review Rating { get; set; }
    }
    
}
