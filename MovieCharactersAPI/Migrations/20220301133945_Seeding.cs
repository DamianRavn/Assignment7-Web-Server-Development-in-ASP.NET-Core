using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieCharactersAPI.Migrations
{
    public partial class Seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterMovie_Character_CharactersId",
                table: "CharacterMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterMovie_Movie_MoviesId",
                table: "CharacterMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Franchise_FranchiseId",
                table: "Movie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movie",
                table: "Movie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Franchise",
                table: "Franchise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Character",
                table: "Character");

            migrationBuilder.RenameTable(
                name: "Movie",
                newName: "Movies");

            migrationBuilder.RenameTable(
                name: "Franchise",
                newName: "Franchises");

            migrationBuilder.RenameTable(
                name: "Character",
                newName: "Characters");

            migrationBuilder.RenameColumn(
                name: "MoviesId",
                table: "CharacterMovie",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "CharactersId",
                table: "CharacterMovie",
                newName: "CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterMovie_MoviesId",
                table: "CharacterMovie",
                newName: "IX_CharacterMovie_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Movie_FranchiseId",
                table: "Movies",
                newName: "IX_Movies_FranchiseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Franchises",
                table: "Franchises",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Alias", "Gender", "ImageURL", "Name" },
                values: new object[] { 2, "Lord of Thunder", "Male", null, "Thor" });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Alias", "Gender", "ImageURL", "Name" },
                values: new object[] { 3, "Tony Stark", "Male", null, "Ironman" });

            migrationBuilder.InsertData(
                table: "Franchises",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Marvel's cinematic universe.", "MCU" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Director", "FranchiseId", "Genre", "ImageURL", "Title", "TrailerURL", "Year" },
                values: new object[] { 1, "Joss Whedon", 1, "Action", null, "The Avengers", null, 2012 });

            migrationBuilder.InsertData(
                table: "CharacterMovie",
                columns: new[] { "CharacterId", "MovieId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "CharacterMovie",
                columns: new[] { "CharacterId", "MovieId" },
                values: new object[] { 3, 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterMovie_Characters_CharacterId",
                table: "CharacterMovie",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterMovie_Movies_MovieId",
                table: "CharacterMovie",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Franchises_FranchiseId",
                table: "Movies",
                column: "FranchiseId",
                principalTable: "Franchises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterMovie_Characters_CharacterId",
                table: "CharacterMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterMovie_Movies_MovieId",
                table: "CharacterMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Franchises_FranchiseId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Franchises",
                table: "Franchises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.DeleteData(
                table: "CharacterMovie",
                keyColumns: new[] { "CharacterId", "MovieId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "CharacterMovie",
                keyColumns: new[] { "CharacterId", "MovieId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Franchises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Movies",
                newName: "Movie");

            migrationBuilder.RenameTable(
                name: "Franchises",
                newName: "Franchise");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "Character");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "CharacterMovie",
                newName: "MoviesId");

            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "CharacterMovie",
                newName: "CharactersId");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterMovie_MovieId",
                table: "CharacterMovie",
                newName: "IX_CharacterMovie_MoviesId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_FranchiseId",
                table: "Movie",
                newName: "IX_Movie_FranchiseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movie",
                table: "Movie",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Franchise",
                table: "Franchise",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Character",
                table: "Character",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterMovie_Character_CharactersId",
                table: "CharacterMovie",
                column: "CharactersId",
                principalTable: "Character",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterMovie_Movie_MoviesId",
                table: "CharacterMovie",
                column: "MoviesId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Franchise_FranchiseId",
                table: "Movie",
                column: "FranchiseId",
                principalTable: "Franchise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
