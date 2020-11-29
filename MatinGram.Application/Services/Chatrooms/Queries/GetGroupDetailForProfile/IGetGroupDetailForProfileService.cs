using MatinGram.Application.Interfaces;
using MatinGram.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Application.Services.Chatrooms.Queries.GetGroupDetailForProfile
{
    public interface IGetGroupDetailForProfileService
    {
        Task<ResultDto<GroupDetailForProfileDto>> ExecuteAsync(long myUserId, Guid chatroomGuid);
    }

    public class GetGroupDetailForProfileService : IGetGroupDetailForProfileService
    {
        private readonly IDataBaseContext _context;
        public GetGroupDetailForProfileService(IDataBaseContext context)
        {
            _context = context;
        }
        public async Task<ResultDto<GroupDetailForProfileDto>> ExecuteAsync(long myUserId, Guid chatroomGuid)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var chatroom = await _context.Chatrooms
                    .FirstOrDefaultAsync(c => c.Guid == chatroomGuid);

                    #region --validation--

                    if (chatroom == null || !_context.Users.Any(u => u.Id == myUserId))
                    {
                        return new ResultDto<GroupDetailForProfileDto>()
                        {
                            Status = Common.Enums.ServiceStatus.NotFound,
                        };
                    }

                    #endregion

                    #region --find my level--
                    MemberLevel myLevel = GetLevel(myUserId, chatroom);
                    #endregion

                    #region --find chatroom image--

                    var chatroomImage = await _context.ChatroomImages
                    .Where(u => u.ChatroomId == chatroom.Id)
                    .OrderBy(u => u.InsertTime)
                    .LastOrDefaultAsync();

                    string chatroomImageName = "Images/ChatroomImages/Default.png";

                    if (chatroomImage != null)
                    {
                        chatroomImageName = chatroomImage.ImageName;
                    }

                    #endregion

                    #region fill Data 
                    GroupDetailForProfileDto Data = new GroupDetailForProfileDto();

                    Data.ImageName = chatroomImageName;
                    Data.GroupName = chatroom.Name;
                    Data.MyLevel = myLevel;

                    #region --Find members--

                    var userInChatrooms = await _context.UserInChatrooms
                    .Where(u => u.ChatroomId == chatroom.Id)
                    .ToListAsync();

                    List<GroupMemberDto> members = new List<GroupMemberDto>();


                    foreach (var item in userInChatrooms)
                    {
                        GroupMemberDto newMember = new GroupMemberDto();

                        var userMember = await _context.Users.FindAsync(item.UserId);

                        newMember.Name = userMember.Name;
                        newMember.HashKey = userMember.HashKey;
                        newMember.MemberLevel = GetLevel(userMember.Id,chatroom);
                        #region find image

                        var memberImage = await _context.UserImages
                        .Where(u => u.UserId == userMember.Id)
                        .OrderBy(u => u.InsertTime)
                        .LastOrDefaultAsync();


                        var memberImageName = "Images/UserImages/Default.png";
                        if (memberImage!=null)
                        {
                            memberImageName = memberImage.ImageName;
                        }


                        #endregion
                        newMember.ImageName = memberImageName;

                        members.Add(newMember);
                    }

                    #endregion

                    Data.Members = members;

                    #endregion

                    return new ResultDto<GroupDetailForProfileDto>()
                    {
                        Data = Data,
                        Status = Common.Enums.ServiceStatus.Success,
                    };

                }
                catch (Exception)
                {
                    return new ResultDto<GroupDetailForProfileDto>()
                    {
                        Status = Common.Enums.ServiceStatus.SystemError,
                    };
                }
            });
        }

        private MemberLevel GetLevel(long UserId, Domain.Entities.Chatrooms.Chatroom chatroom)
        {
            MemberLevel Level = MemberLevel.Member;
            if (chatroom.CreatorId == UserId)
            {
                Level = MemberLevel.Master;
            }
            else
            {
                if (_context.AdminInChatrooms.Any(a => a.UserId == UserId && a.ChatroomId == chatroom.Id))
                {
                    Level = MemberLevel.Admin;
                }
            }

            return Level;
        }
    }

    public record GroupDetailForProfileDto
    {
        public string ImageName { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<GroupMemberDto> Members { get; set; }
        public MemberLevel MyLevel { get; set; }
    }

    public record GroupMemberDto
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string HashKey { get; set; }
        public MemberLevel MemberLevel { get; set; }
    }

    public enum MemberLevel
    {
        Member,
        Admin,
        Master
    }

}
