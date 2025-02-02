using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class FacultyMapper
    {
        public static FacultyDto MapToFacultyDto(Faculty faculty)
        {
            return new FacultyDto()
            {
                FacultyId = faculty.FacultyId,
                FacultyName = faculty.FacultyName
            };
        }
    }
}
