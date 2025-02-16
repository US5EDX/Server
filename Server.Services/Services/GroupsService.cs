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

        public async Task<IEnumerable<GroupShortDto>> GetByFacultyId(uint facultyId)
        {
            var specialties = await _groupRepository.GetByFacultyId(facultyId);
            return specialties.Select(GroupMapper.MapToGroupShortDto);
        }

        public async Task<IEnumerable<GroupShortDto>> GetByFacultyIdAndCodeFilter(uint facultyId, string codeFilter)
        {
            var specialties = await _groupRepository.GetByFacultyIdAndCodeFilter(facultyId, codeFilter);
            return specialties.Select(GroupMapper.MapToGroupShortDto);
        }
    }
}
