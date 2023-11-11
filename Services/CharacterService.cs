using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Jhon" }
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddChatacterDto newCharacter)
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var Character = _mapper.Map<Character>(newCharacter);
            Character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(_mapper.Map<Character>(newCharacter));
            ServiceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return ServiceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            ServiceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return ServiceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int Id)
        {
            var ServiceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == Id);
            ServiceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return ServiceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {

                var character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);

                if (character is null)
                    throw new Exception($"Character with Id '{updateCharacter.Id}' not found");

                character.Name = updateCharacter.Name;
                character.HitPoints = updateCharacter.HitPoints;
                character.Strength = updateCharacter.Strength;
                character.Defense = updateCharacter.Defense;
                character.Intelligence = updateCharacter.Intelligence;
                character.Class = updateCharacter.Class;

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}