using HotelManagementSystem.Model.Entity;

namespace HotelManagementSystem.Implementation.IRepository
{
    public interface IDashBoardRepository
    {
        List<Room> GetAllRooms();
        List<Booking> GetAllBookings();
        List<Product> GetAllProducts();
        decimal GetTotalPayments();
    }
}
