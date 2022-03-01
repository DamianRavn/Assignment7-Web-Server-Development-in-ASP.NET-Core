using System.Collections.Generic;

namespace MovieCharactersAPI.Models.DTO.Character
{
    public class CharacterReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
        public List<int> Movies { get; set; }
    }
}
