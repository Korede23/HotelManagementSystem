using HotelManagementSystem.Implementation.IRepository;
using HotelManagementSystem.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Implementation.Repository
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DashBoardRepository(ApplicationDbContext  dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Room> GetAllRooms()
        {
            return _dbContext.Rooms.ToList();
        }

        public List<Booking> GetAllBookings()
        {
            return _dbContext.Bookings.ToList();
        }

        public List<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public decimal GetTotalPayments()
        {
            return _dbContext.Payments.Sum(payment => payment.Amount);
        }



    }
}
