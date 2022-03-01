namespace MovieCharactersAPI.Models.DTO.Character
{
    public class CharacterUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Gender { get; set; }
        public string ImageURL { get; set; }
    }
}
