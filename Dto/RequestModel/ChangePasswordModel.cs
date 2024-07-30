namespace HotelManagementSystem.Dto.RequestModel
{
    public class ChangePasswordModel
    {
        public string  CurrentPassword { get; set; }
        public string  NewPassword { get; set; }
        public string  PasswordConfirm { get; set; }
    }
}
