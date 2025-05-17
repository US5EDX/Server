using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings;

public static class FacultyMapper
{
    public static FacultyDto MapToFacultyDto(Faculty faculty) =>
        new() { FacultyId = faculty.FacultyId, FacultyName = faculty.FacultyName };

    public static Faculty MapToFaculty(FacultyDto faculty) =>
        new() { FacultyId = faculty.FacultyId, FacultyName = faculty.FacultyName };
}