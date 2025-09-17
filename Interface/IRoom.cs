
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Interface
{
    public interface IRoom : IGenericRepository<Cls_Room>
    {

         Task<List<DTO_Rooms>> ShowRooms();
        Task<DTO_Rooms?> ShowRoomById(int id);
        Task<DTO_Rooms> CreateRoom(DTO_CreateRoom dto);

        Task<DTO_Rooms> Edit(int ID, DTO_CreateRoom DTO_Request); 
    }
}
