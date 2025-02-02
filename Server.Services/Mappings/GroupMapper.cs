using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class GroupMapper
    {
        public static GroupDto MapToGroupDto(Group group)
        {
            return new GroupDto()
            {
                GroupId = group.GroupId,
                GroupCode = group.GroupCode,
                EduLevel = group.EduLevel,
                Course = group.Course,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester
            };
        }
    }
}
