using Server.Models.Models;
using Server.Services.Dtos;

namespace Server.Services.Mappings
{
    public class SpecialtyMapper
    {
        public static SpecialtyDto? MapToSpecialtyDto(Specialty? specialty)
        {
            return specialty is null ? null : new SpecialtyDto()
            {
                SpecialtyId = specialty.SpecialtyId,
                SpecialtyName = specialty.SpecialtyName,
                FacultyId = specialty.FacultyId,
            };
        }

        public static Specialty MapToSpecialty(SpecialtyDto specailty)
        {
            return new Specialty()
            {
                SpecialtyId = specailty.SpecialtyId,
                SpecialtyName = specailty.SpecialtyName,
                FacultyId = specailty.FacultyId
            };
        }
    }
}
