using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;

namespace Server.Services.Services
{
    public class GroupsService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupsService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<GroupDto?> GetGroupById(uint groupId)
        {
            var group = await _groupRepository.GetById(groupId);
            return group == null ? null : GroupMapper.MapToGroupDto(group);
        }

        public async Task<IEnumerable<GroupFullInfoDto>> GetByFacultyId(uint facultyId)
        {
            var specialties = await _groupRepository.GetByFacultyId(facultyId);
            return specialties.Select(GroupMapper.MapToGroupFullInfo);
        }

        public async Task<GroupFullInfoDto> AddGroup(GroupRegistryDto group)
        {
            var newGroup = GroupMapper.MapToGroupWithoutCuratorId(group);

            if (group.CuratorId is not null)
                newGroup.CuratorId = GetWorkerIdAsByteArray(group.CuratorId);

            var addedGroup = await _groupRepository.Add(newGroup);

            return GroupMapper.MapToGroupFullInfo(addedGroup);
        }

        public async Task<GroupFullInfoDto?> UpdateGroup(GroupRegistryDto group)
        {
            var updatingGroup = GroupMapper.MapToGroupWithoutCuratorId(group);

            if (group.CuratorId is not null)
                updatingGroup.CuratorId = GetWorkerIdAsByteArray(group.CuratorId);

            var updatedGroup = await _groupRepository.Update(updatingGroup);

            if (updatedGroup is null)
                return null;

            return GroupMapper.MapToGroupFullInfo(updatedGroup);
        }

        public async Task<bool?> DeleteGroup(uint groupId)
        {
            return await _groupRepository.Delete(groupId);
        }

        private byte[] GetWorkerIdAsByteArray(string workerId)
        {
            var isSuccess = Ulid.TryParse(workerId, out Ulid ulidWorkerId);

            if (!isSuccess)
                throw new InvalidCastException("Невалідний Id");

            return ulidWorkerId.ToByteArray();
        }
    }
}
