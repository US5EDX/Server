using Server.Models.Interfaces;
using Server.Services.Dtos;
using Server.Services.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<GroupWithSpecialtyDto>> GetByFacultyId(uint facultyId)
        {
            var specialties = await _groupRepository.GetByFacultyId(facultyId);
            return specialties.Select(GroupMapper.MapToGroupWithSpecialtyDto);
        }

        public async Task<IEnumerable<GroupShortDto>> GetByFacultyIdAndCodeFilter(uint facultyId, string codeFilter)
        {
            var specialties = await _groupRepository.GetByFacultyIdAndCodeFilter(facultyId, codeFilter);
            return specialties.Select(GroupMapper.MapToGroupShortDto);
        }

        public async Task<GroupWithSpecialtyDto> AddGroup(GroupWithSpecialtyDto group)
        {
            var newGroup = await _groupRepository.Add(GroupMapper.MapToGroup(group));

            return GroupMapper.MapToGroupWithSpecialtyDto(newGroup);
        }

        public async Task<GroupWithSpecialtyDto?> UpdateGroup(GroupWithSpecialtyDto group)
        {
            var updatedGroup = await _groupRepository.Update(GroupMapper.MapToGroup(group));

            if (updatedGroup is null)
                return null;

            return GroupMapper.MapToGroupWithSpecialtyDto(updatedGroup);
        }

        public async Task<bool?> DeleteGroup(uint groupId)
        {
            return await _groupRepository.Delete(groupId);
        }

        public async Task UpdateGroupsCourse(uint facultyId)
        {
            await _groupRepository.UpdateGroupsCourse(facultyId);
        }
    }
}
