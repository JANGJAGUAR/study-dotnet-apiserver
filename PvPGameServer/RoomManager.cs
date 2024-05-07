using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPGameServer;

public class RoomManager
{
    List<Room> _roomsList = new List<Room>();
    private int roomCheckMaxCount { get; set; }
    
    private int _maxRoomCount = 0;

    private int _start = 0;
    private int _end = 0;

    // private Queue<int> emptyRoom = new Queue<int>();
    // TODO 방 만들기 관련 함수가 있을 시 queue에서 번호를 뽑아 만들기가 가능해짐 (지금은 방 번호 찾아서 입장)



    public void CreateRooms(ServerOption serverOpt)
    {
        _maxRoomCount = serverOpt.RoomMaxCount;
        var startNumber = serverOpt.RoomStartNumber;
        var roomMaxUserCount = serverOpt.RoomMaxUserCount;

        roomCheckMaxCount = serverOpt.RoomCheckMaxCount;

        for(int i = 0; i < _maxRoomCount; ++i)
        {
            var roomNumber = (startNumber + i);
            var room = new Room();
            room.Init(i, roomNumber, roomMaxUserCount);

            _roomsList.Add(room);
            // emptyRoom.Enqueue(i);
        }                                   
    }


    public List<Room> GetRoomsList() 
    { 
        return _roomsList; 
    }

    public void CheckRoomList()
    {
        //끝 설정
        _end = _start + roomCheckMaxCount;
        if (_end >= _maxRoomCount)
        {
            _end = _maxRoomCount;
        }
        
        for (int idx = _start; idx < _end; idx++)
        {
            //TODO 여기서 시간을 받아도 되는걸까? 적어도 for문 바깥?
            var nowTime = DateTime.Now;
            _roomsList[idx].RoomPutStoneCheck(nowTime);
            _roomsList[idx].RoomGameCheck(nowTime);
        }
        
        //시작 설정
        _start += roomCheckMaxCount;
        if (_start >= _maxRoomCount)
        {
            _start = 0;
        }
    }
}
