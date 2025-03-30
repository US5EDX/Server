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

        public static GroupFullInfoDto MapToGroupFullInfo(Group group)
        {
            return new GroupFullInfoDto()
            {
                GroupId = group.GroupId,
                GroupCode = group.GroupCode,
                Specialty = SpecialtyMapper.MapToSpecialtyDto(group.Specialty),
                EduLevel = group.EduLevel,
                Course = group.Course,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester,
                CuratorInfo = group.Curator is null ? null : WorkerMapper.MapToWorkerShortInfoDto(group.Curator)
            };
        }

        public static Group MapToGroupWithoutCuratorId(GroupRegistryDto group)
        {
            return new Group()
            {
                GroupId = group.GroupId ?? 0,
                GroupCode = group.GroupCode,
                SpecialtyId = group.SpecialtyId,
                EduLevel = group.EduLevel,
                Course = group.Course,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester
            };
        }
    }
}
