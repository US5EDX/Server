using Server.Models.CustomExceptions;
using Server.Models.Enums;
using Server.Models.Interfaces;
using Server.Services.Dtos.GroupDtos;
using Server.Services.Mappings;
using Server.Services.Parsers;

namespace Server.Services.Services;

public class GroupsService(IGroupRepository groupRepository)
{
    public async Task<GroupDto> GetByIdOrThrow(uint groupId)
    {
        var group = await groupRepository.GetById(groupId) ??
            throw new NotFoundException("Групу не знайдено");

        return GroupMapper.MapToGroupDto(group);
    }

    public async Task<IEnumerable<GroupFullInfoDto>> GetByFacultyId(uint facultyId, string? curatorId = null)
    {
        var byteCuratorId = UlidIdParser.ParseIdWithNull(curatorId);

        var specialties = await groupRepository.GetByFacultyAndCuratorId(facultyId, byteCuratorId);
        return specialties.Select(GroupMapper.MapToGroupFullInfo);
    }

    public async Task<GroupFullInfoDto> AddGroup(GroupRegistryDto group)
    {
        var newGroup = GroupMapper.MapToGroupWithoutCuratorId(group);

        if (group.CuratorId is not null)
            newGroup.CuratorId = UlidIdParser.ParseId(group.CuratorId);

        var addedGroup = await groupRepository.Add(newGroup);

        return GroupMapper.MapToGroupFullInfo(addedGroup);
    }

    public async Task<GroupFullInfoDto> UpdateOrThrow(GroupRegistryDto group)
    {
        if (group.GroupId is null) throw new BadRequestException("Невалідні вхідні дані");

        var updatingGroup = GroupMapper.MapToGroupWithoutCuratorId(group);

        if (group.CuratorId is not null)
            updatingGroup.CuratorId = UlidIdParser.ParseId(group.CuratorId);

        var updatedGroup = await groupRepository.Update(updatingGroup) ??
            throw new NotFoundException("Групу не знайдено");

        return GroupMapper.MapToGroupFullInfo(updatedGroup);
    }

    public async Task DeleteOrThrow(uint groupId)
    {
        var result = await groupRepository.Delete(groupId, DateTime.Today);

        result.ThrowIfFailed("Вказана група не знайдена",
            "Неможливо видалити, оскільки у групі є студенти або група ще не закінчила навчання");
    }

    public async Task DeleteGraduatedOrThrow(uint facultyId)
    {
        var isSuccess = await groupRepository.DeleteGraduated(facultyId, DateTime.Today);

        if (!isSuccess) throw new NotFoundException("Не було знайдено груп, що випустились");
    }
}
