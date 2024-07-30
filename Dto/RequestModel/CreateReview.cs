using HotelManagementSystem.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Dto.RequestModel
{
    public class CreateReview
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        public Review Rating { get; set; }
    }
}
