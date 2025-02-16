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

        public static GroupShortDto MapToGroupShortDto(Group group)
        {
            return new GroupShortDto()
            {
                GroupId = group.GroupId,
                GroupCode = group.GroupCode,
            };
        }

        public static GroupWithSpecialtyDto MapToGroupWithSpecialtyDto(Group group)
        {
            return new GroupWithSpecialtyDto()
            {
                GroupId = group.GroupId,
                GroupCode = group.GroupCode,
                Specialty = SpecialtyMapper.MapToSpecialtyDto(group.Specialty),
                EduLevel = group.EduLevel,
                Course = group.Course,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester
            };
        }

        public static Group MapToGroup(GroupWithSpecialtyDto group)
        {
            return new Group()
            {
                GroupId = group.GroupId ?? 0,
                GroupCode = group.GroupCode,
                SpecialtyId = group.Specialty.SpecialtyId,
                EduLevel = group.EduLevel,
                Course = group.Course,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester
            };
        }
    }
}
