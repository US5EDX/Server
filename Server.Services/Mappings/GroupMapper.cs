using Server.Models.Models;
using Server.Services.Dtos;
using Server.Services.Services;

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
                Course = CalcuationService.CalculateGroupCourse(group),
                DurationOfStudy = group.DurationOfStudy,
                AdmissionYear = group.AdmissionYear,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester,
                HasEnterChoise = group.HasEnterChoise
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
                Course = CalcuationService.CalculateGroupCourse(group),
                DurationOfStudy = group.DurationOfStudy,
                AdmissionYear = group.AdmissionYear,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester,
                HasEnterChoise = group.HasEnterChoise,
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
                DurationOfStudy = group.DurationOfStudy,
                AdmissionYear = group.AdmissionYear,
                Nonparsemester = group.Nonparsemester,
                Parsemester = group.Parsemester,
                HasEnterChoise = group.HasEnterChoise
            };
        }
    }
}
