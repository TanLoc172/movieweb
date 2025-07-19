
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
// using MovieWebsite.Data;
// using MovieWebsite.Models;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.SignalR;

// namespace MovieWebsite.Hubs
// {
//     public class WatchPartyHub : Hub
//     {
//         private readonly AppDbContext _context;

//         // Lưu trữ người dùng đang hoạt động trong mỗi phòng (inviteCode -> tập hợp các ConnectionId)
//         private static readonly Dictionary<string, HashSet<string>> _roomUsers = new Dictionary<string, HashSet<string>>();
//         // Lưu trữ tên người dùng theo ConnectionId để dễ dàng truy xuất
//         private static readonly Dictionary<string, string> _userNames = new Dictionary<string, string>();
//         // Lưu trữ mapping từ ConnectionId của client đến inviteCode của phòng họ đang tham gia
//         private static readonly Dictionary<string, string> _connectionToRoomMap = new Dictionary<string, string>();

//         public WatchPartyHub(AppDbContext context)
//         {
//             _context = context;
//         }

//         // Xử lý khi một client mới kết nối đến Hub
//         public override async Task OnConnectedAsync()
//         {
//             // Không làm gì ở đây, vì người dùng sẽ gọi JoinRoom để xác định phòng
//             await base.OnConnectedAsync();
//         }

//         // Xử lý khi một client ngắt kết nối
//         public override async Task OnDisconnectedAsync(Exception? exception)
//         {
//             // Lấy inviteCode của phòng mà client đang tham gia
//             if (_connectionToRoomMap.TryGetValue(Context.ConnectionId, out string? roomId))
//             {
//                 // Lấy tên người dùng trước khi nó bị xóa
//                 string userName = _userNames.TryGetValue(Context.ConnectionId, out var name) ? name : "Một người dùng";

//                 // Xóa người dùng khỏi danh sách phòng
//                 if (_roomUsers.ContainsKey(roomId) && _roomUsers[roomId].Contains(Context.ConnectionId))
//                 {
//                     _roomUsers[roomId].Remove(Context.ConnectionId);

//                     // Gửi tin nhắn thông báo rời đi cho những người còn lại trong phòng
//                     await Clients.Group(roomId).SendAsync("SystemMessage", $"{userName} đã rời phòng.");

//                     // Cập nhật danh sách người dùng cho tất cả mọi người trong phòng
//                     await SendUserListToRoom(roomId);
//                 }

//                 // Dọn dẹp các dictionary
//                 _connectionToRoomMap.Remove(Context.ConnectionId);
//                 _userNames.Remove(Context.ConnectionId);

//                 // Nếu phòng không còn ai, có thể xóa nó khỏi _roomUsers để tiết kiệm bộ nhớ
//                 if (_roomUsers.ContainsKey(roomId) && !_roomUsers[roomId].Any())
//                 {
//                     _roomUsers.Remove(roomId);
//                 }
//             }

//             await base.OnDisconnectedAsync(exception);
//         }

//         // Phương thức được gọi từ client khi họ vào một phòng xem chung
//         // Đổi tên tham số 'roomId' thành 'inviteCode' cho phù hợp với front-end
//         public async Task JoinRoom(string inviteCode, string userName)
//         {
//             // Kiểm tra xem phòng có tồn tại trong DB không
//             var room = await _context.WatchPartyRooms.FirstOrDefaultAsync(r => r.InviteCode == inviteCode);
//             if (room == null)
//             {
//                 // Nếu phòng không tồn tại, thông báo cho client gọi và thoát.
//                 await Clients.Caller.SendAsync("SystemMessage", $"Phòng xem chung với mã '{inviteCode}' không tồn tại.");
//                 return;
//             }

//             // Sử dụng inviteCode làm định danh phòng cho các nhóm SignalR và dictionary
//             string roomId = inviteCode;

//             // Thêm client vào nhóm SignalR của phòng
//             await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

//             // Lưu trữ thông tin người dùng vào các dictionary
//             if (!_roomUsers.ContainsKey(roomId))
//             {
//                 _roomUsers[roomId] = new HashSet<string>();
//             }
//             _roomUsers[roomId].Add(Context.ConnectionId);
//             _userNames[Context.ConnectionId] = userName;
//             _connectionToRoomMap[Context.ConnectionId] = roomId;

//             // Gửi tin nhắn thông báo người dùng mới tham gia cho tất cả mọi người trong phòng
//             await Clients.Group(roomId).SendAsync("SystemMessage", $"{userName} đã tham gia phòng.");

//             // Cập nhật danh sách người dùng cho tất cả mọi người trong phòng
//             await SendUserListToRoom(roomId);
//         }

//         // Phương thức được gọi khi client gửi tin nhắn chat
//         public async Task SendMessage(string inviteCode, string userName, string message)
//         {
//             // Lưu tin nhắn vào cơ sở dữ liệu
//             var room = await _context.WatchPartyRooms.FirstOrDefaultAsync(r => r.InviteCode == inviteCode);
//             if (room != null)
//             {
//                 var chatMessage = new ChatMessage
//                 {
//                     RoomId = room.Id,
//                     UserName = userName,
//                     Message = message,
//                     Timestamp = DateTime.Now
//                 };
//                 _context.ChatMessages.Add(chatMessage);
//                 await _context.SaveChangesAsync();
//             }

//             // Gửi tin nhắn đến tất cả các client trong phòng
//             // Front-end sẽ nhận và hiển thị tin nhắn này.
//             await Clients.Group(inviteCode).SendAsync("ReceiveMessage", userName, message);
//         }

//         // Phương thức được gọi khi người điều khiển (hoặc client khác) thay đổi trạng thái video
//         public async Task SyncVideoState(string inviteCode, string state, double currentTime)
//         {
//             // Gửi cập nhật trạng thái video đến tất cả các client khác trong phòng
//             // Dùng 'Clients.Group' thay vì 'Clients.OthersInGroup' để đảm bảo mọi client,
//             // kể cả người điều khiển ban đầu, đều nhận được trạng thái mới nhất.
//             await Clients.Group(inviteCode).SendAsync("ReceiveVideoState", state, currentTime);
//         }

//         // Helper method để gửi danh sách người dùng hiện tại trong một phòng
//         private async Task SendUserListToRoom(string roomId)
//         {
//             if (_roomUsers.TryGetValue(roomId, out HashSet<string>? userConnections))
//             {
//                 var userList = new List<string>();
//                 foreach (var connectionId in userConnections)
//                 {
//                     // Lấy tên người dùng từ ConnectionId
//                     if (_userNames.TryGetValue(connectionId, out string? userName))
//                     {
//                         userList.Add(userName);
//                     }
//                 }

//                 // Gửi danh sách tên người dùng tới tất cả các client trong nhóm
//                 // Client sẽ sử dụng danh sách này để cập nhật giao diện.
//                 await Clients.Group(roomId).SendAsync("UpdateUserList", userList);
//             }
//             else
//             {
//                 // Nếu phòng không còn ai hoặc chưa có người dùng, gửi danh sách rỗng
//                 await Clients.Group(roomId).SendAsync("UpdateUserList", new List<string>());
//             }
//         }

        
//     }
// }


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Data;
using MovieWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MovieWebsite.Hubs
{
    public class WatchPartyHub : Hub
    {
        private readonly AppDbContext _context;

        // Lưu trữ người dùng đang hoạt động trong mỗi phòng (inviteCode -> tập hợp các ConnectionId)
        private static readonly Dictionary<string, HashSet<string>> _roomUsers = new Dictionary<string, HashSet<string>>();
        // Lưu trữ tên người dùng theo ConnectionId để dễ dàng truy xuất
        private static readonly Dictionary<string, string> _userNames = new Dictionary<string, string>();
        // Lưu trữ mapping từ ConnectionId của client đến inviteCode của phòng họ đang tham gia
        private static readonly Dictionary<string, string> _connectionToRoomMap = new Dictionary<string, string>();

        public WatchPartyHub(AppDbContext context)
        {
            _context = context;
        }

        // Xử lý khi một client mới kết nối đến Hub
        public override async Task OnConnectedAsync()
        {
            // Không làm gì ở đây, vì người dùng sẽ gọi JoinRoom để xác định phòng
            await base.OnConnectedAsync();
        }

        // Xử lý khi một client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Lấy inviteCode của phòng mà client đang tham gia
            if (_connectionToRoomMap.TryGetValue(Context.ConnectionId, out string? roomId))
            {
                // Lấy tên người dùng trước khi nó bị xóa
                string userName = _userNames.TryGetValue(Context.ConnectionId, out var name) ? name : "Một người dùng";

                // Xóa người dùng khỏi danh sách phòng
                if (_roomUsers.ContainsKey(roomId) && _roomUsers[roomId].Contains(Context.ConnectionId))
                {
                    _roomUsers[roomId].Remove(Context.ConnectionId);

                    // Gửi tin nhắn thông báo rời đi cho những người còn lại trong phòng
                    await Clients.Group(roomId).SendAsync("SystemMessage", $"{userName} đã rời phòng.");

                    // Cập nhật danh sách người dùng cho tất cả mọi người trong phòng
                    await SendUserListToRoom(roomId);
                }

                // Dọn dẹp các dictionary
                _connectionToRoomMap.Remove(Context.ConnectionId);
                _userNames.Remove(Context.ConnectionId);

                // Nếu phòng không còn ai, có thể xóa nó khỏi _roomUsers để tiết kiệm bộ nhớ
                if (_roomUsers.ContainsKey(roomId) && !_roomUsers[roomId].Any())
                {
                    _roomUsers.Remove(roomId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Phương thức được gọi từ client khi họ vào một phòng xem chung
        // Đổi tên tham số 'roomId' thành 'inviteCode' cho phù hợp với front-end
        public async Task JoinRoom(string inviteCode, string userName)
        {
            // Kiểm tra xem phòng có tồn tại trong DB không
            var room = await _context.WatchPartyRooms.FirstOrDefaultAsync(r => r.InviteCode == inviteCode);
            if (room == null)
            {
                // Nếu phòng không tồn tại, thông báo cho client gọi và thoát.
                await Clients.Caller.SendAsync("SystemMessage", $"Phòng xem chung với mã '{inviteCode}' không tồn tại.");
                return;
            }

            // Sử dụng inviteCode làm định danh phòng cho các nhóm SignalR và dictionary
            string roomId = inviteCode;

            // Thêm client vào nhóm SignalR của phòng
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            // Lưu trữ thông tin người dùng vào các dictionary
            if (!_roomUsers.ContainsKey(roomId))
            {
                _roomUsers[roomId] = new HashSet<string>();
            }
            _roomUsers[roomId].Add(Context.ConnectionId);
            _userNames[Context.ConnectionId] = userName;
            _connectionToRoomMap[Context.ConnectionId] = roomId;

            // Gửi tin nhắn thông báo người dùng mới tham gia cho tất cả mọi người trong phòng
            await Clients.Group(roomId).SendAsync("SystemMessage", $"{userName} đã tham gia phòng.");

            // Cập nhật danh sách người dùng cho tất cả mọi người trong phòng
            await SendUserListToRoom(roomId);
        }

        // Phương thức được gọi khi client gửi tin nhắn chat
        public async Task SendMessage(string inviteCode, string userName, string message)
        {
            // Lưu tin nhắn vào cơ sở dữ liệu
            var room = await _context.WatchPartyRooms.FirstOrDefaultAsync(r => r.InviteCode == inviteCode);
            if (room != null)
            {
                var chatMessage = new ChatMessage
                {
                    RoomId = room.Id,
                    UserName = userName,
                    Message = message,
                    Timestamp = DateTime.Now
                };
                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();
            }

            // Gửi tin nhắn đến tất cả các client trong phòng
            // Front-end sẽ nhận và hiển thị tin nhắn này.
            await Clients.Group(inviteCode).SendAsync("ReceiveMessage", userName, message);
        }

        // Phương thức được gọi khi người điều khiển (hoặc client khác) thay đổi trạng thái video
        public async Task SyncVideoState(string inviteCode, string state, double currentTime)
        {
            // Gửi cập nhật trạng thái video đến tất cả các client khác trong phòng
            // Dùng 'Clients.Group' thay vì 'Clients.OthersInGroup' để đảm bảo mọi client,
            // kể cả người điều khiển ban đầu, đều nhận được trạng thái mới nhất.
            await Clients.Group(inviteCode).SendAsync("ReceiveVideoState", state, currentTime);
        }

        // Helper method để gửi danh sách người dùng hiện tại trong một phòng
        private async Task SendUserListToRoom(string roomId)
        {
            if (_roomUsers.TryGetValue(roomId, out HashSet<string>? userConnections))
            {
                var userList = new List<string>();
                foreach (var connectionId in userConnections)
                {
                    // Lấy tên người dùng từ ConnectionId
                    if (_userNames.TryGetValue(connectionId, out string? userName))
                    {
                        userList.Add(userName);
                    }
                }

                // Gửi danh sách tên người dùng tới tất cả các client trong nhóm
                // Client sẽ sử dụng danh sách này để cập nhật giao diện.
                await Clients.Group(roomId).SendAsync("UpdateUserList", userList);
            }
            else
            {
                // Nếu phòng không còn ai hoặc chưa có người dùng, gửi danh sách rỗng
                await Clients.Group(roomId).SendAsync("UpdateUserList", new List<string>());
            }
        }

        // ===== PHƯƠNG THỨC MỚI ĐỂ CHUYỂN TẬP =====
        public async Task ChangeEpisode(string roomCode,  int newEpisodeId)
        {
            var room = await _context.WatchPartyRooms
        .Include(r => r.Movie)
        .FirstOrDefaultAsync(r => r.InviteCode == roomCode);

            if (room != null)
            {
                // Kiểm tra xem episode mới có thuộc về phim của phòng này không
                bool episodeExistsInMovie = await _context.Episodes
                    .AnyAsync(e => e.Id == newEpisodeId && e.MovieId == room.MovieId);

                if (episodeExistsInMovie)
                {
                    // Cập nhật tập phim hiện tại của phòng
                    room.CurrentEpisodeId = newEpisodeId;
                    await _context.SaveChangesAsync();

                    // Gửi lệnh ReloadPage đến tất cả client
                    await Clients.Group(roomCode).SendAsync("ReloadPage");
                }
            }
        }
    }
}