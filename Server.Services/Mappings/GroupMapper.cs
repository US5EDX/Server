using Server.Models.Models;
using Server.Services.Dtos.GroupDtos;
using Server.Services.Services.StaticServices;

namespace Server.Services.Mappings;

public static class GroupMapper
{
    public static GroupDto MapToGroupDto(Group group) =>
        new()
        {
            GroupId = group.GroupId,
            GroupCode = group.GroupCode,
            EduLevel = group.EduLevel,
            Course = CalcuationService.CalculateGroupCourse(group),
            DurationOfStudy = group.DurationOfStudy,
            AdmissionYear = group.AdmissionYear,
            Nonparsemester = group.Nonparsemester,
            Parsemester = group.Parsemester,
            HasEnterChoise = group.HasEnterChoise,
            ChoiceDifference = group.ChoiceDifference,
        };

    public static GroupFullInfoDto MapToGroupFullInfo(Group group) =>
        new()
        {
            GroupId = group.GroupId,
            GroupCode = group.GroupCode,
            Specialty = SpecialtyMapper.MapToNullableSpecialtyDto(group.Specialty) ??
            throw new InvalidCastException("У групи обов'язково має бути спеціальність"),
            EduLevel = group.EduLevel,
            Course = CalcuationService.CalculateGroupCourse(group),
            DurationOfStudy = group.DurationOfStudy,
            AdmissionYear = group.AdmissionYear,
            Nonparsemester = group.Nonparsemester,
            Parsemester = group.Parsemester,
            HasEnterChoise = group.HasEnterChoise,
            ChoiceDifference = group.ChoiceDifference,
            CuratorInfo = group.Curator is null ? null : WorkerMapper.MapToWorkerShortInfoDto(group.Curator)
        };

    public static Group MapToGroupWithoutCuratorId(GroupRegistryDto group) =>
        new()
        {
            GroupId = group.GroupId ?? 0,
            GroupCode = group.GroupCode,
            SpecialtyId = group.SpecialtyId,
            EduLevel = group.EduLevel,
            DurationOfStudy = group.DurationOfStudy,
            AdmissionYear = group.AdmissionYear,
            Nonparsemester = group.Nonparsemester,
            Parsemester = group.Parsemester,
            HasEnterChoise = group.HasEnterChoise,
            ChoiceDifference = group.ChoiceDifference,
        };
}