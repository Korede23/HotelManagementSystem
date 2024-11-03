using HotelManagementSystem.Dto;
using HotelManagementSystem.Implementation.Interface;
using HotelManagementSystem.Implementation.IRepository;

namespace HotelManagementSystem.Implementation.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IDashBoardRepository _dashBoardRepository;

        public DashBoardService(IDashBoardRepository dashBoardRepository)
        {
            _dashBoardRepository = dashBoardRepository;
        }

        public DashBoardDto DashBoardCount()
        {
            var roomcount = _dashBoardRepository.GetAllRooms();
            var products = _dashBoardRepository.GetAllProducts();
            var totalPayments = _dashBoardRepository.GetTotalPayments();
            var booking = _dashBoardRepository.GetAllBookings();



            var data = new DashBoardDto();
            data.TotalRooms = roomcount.Count();
            data.TotalProducts = products.Count();
            data.TotalBookings = booking.Count();
            data.TotalPayments = totalPayments;

            return data;
        }

    }
}
