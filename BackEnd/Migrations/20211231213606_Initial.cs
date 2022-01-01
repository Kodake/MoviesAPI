using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace BackEnd.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Biografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ubicacion = table.Column<Point>(type: "geography", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Identificador);
                });

            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnCartelera = table.Column<bool>(type: "bit", nullable: false),
                    FechaEstreno = table.Column<DateTime>(type: "date", nullable: false),
                    PosterURL = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinesOfertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicio = table.Column<DateTime>(type: "date", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "date", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinesOfertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinesOfertas_Cines_CineId",
                        column: x => x.CineId,
                        principalTable: "Cines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalasDeCine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoSalaDeCine = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Precio = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    CineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalasDeCine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalasDeCine_Cines_CineId",
                        column: x => x.CineId,
                        principalTable: "Cines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneroPelicula",
                columns: table => new
                {
                    GenerosIdentificador = table.Column<int>(type: "int", nullable: false),
                    PeliculasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroPelicula", x => new { x.GenerosIdentificador, x.PeliculasId });
                    table.ForeignKey(
                        name: "FK_GeneroPelicula_Generos_GenerosIdentificador",
                        column: x => x.GenerosIdentificador,
                        principalTable: "Generos",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneroPelicula_Peliculas_PeliculasId",
                        column: x => x.PeliculasId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculasActores",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    Personaje = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasActores", x => new { x.PeliculaId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_PeliculasActores_Actores_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasActores_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculaSalaDeCine",
                columns: table => new
                {
                    PeliculasId = table.Column<int>(type: "int", nullable: false),
                    SalasDeCineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaSalaDeCine", x => new { x.PeliculasId, x.SalasDeCineId });
                    table.ForeignKey(
                        name: "FK_PeliculaSalaDeCine_Peliculas_PeliculasId",
                        column: x => x.PeliculasId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaSalaDeCine_SalasDeCine_SalasDeCineId",
                        column: x => x.SalasDeCineId,
                        principalTable: "SalasDeCine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actores",
                columns: new[] { "Id", "Biografia", "FechaNacimiento", "Nombre" },
                values: new object[,]
                {
                    { 1, "Thomas Stanley Holland (Kingston upon Thames, Londres; 1 de junio de 1996), conocido simplemente como Tom Holland, es un actor, actor de voz y bailarín británico.", new DateTime(1996, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tom Holland" },
                    { 2, "Samuel Leroy Jackson (Washington D. C., 21 de diciembre de 1948), conocido como Samuel L. Jackson, es un actor y productor de cine, televisión y teatro estadounidense. Ha sido candidato al premio Óscar, a los Globos de Oro y al Premio del Sindicato de Actores, así como ganador de un BAFTA al mejor actor de reparto.", new DateTime(1948, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samuel L. Jackson" },
                    { 3, "Robert John Downey Jr. (Nueva York, 4 de abril de 1965) es un actor, actor de voz, productor y cantante estadounidense. Inició su carrera como actor a temprana edad apareciendo en varios filmes dirigidos por su padre, Robert Downey Sr., y en su infancia estudió actuación en varias academias de Nueva York.", new DateTime(1965, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Robert Downey Jr." },
                    { 4, null, new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chris Evans" },
                    { 5, null, new DateTime(1972, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dwayne Johnson" },
                    { 6, null, new DateTime(2000, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Auli'i Cravalho" },
                    { 7, null, new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scarlett Johansson" },
                    { 8, null, new DateTime(1964, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Keanu Reeves" }
                });

            migrationBuilder.InsertData(
                table: "Cines",
                columns: new[] { "Id", "Nombre", "Ubicacion" },
                values: new object[,]
                {
                    { 1, "Agora Mall", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.9388777 18.4839233)") },
                    { 2, "Sambil", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.911582 18.482455)") },
                    { 3, "Megacentro", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.856309 18.506662)") },
                    { 4, "Acropolis", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.939248 18.469649)") }
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Identificador", "Nombre" },
                values: new object[,]
                {
                    { 1, "Acción" },
                    { 2, "Animación" },
                    { 3, "Comedia" },
                    { 4, "Ciencia ficción" },
                    { 5, "Drama" }
                });

            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Id", "EnCartelera", "FechaEstreno", "PosterURL", "Titulo" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2012, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://upload.wikimedia.org/wikipedia/en/8/8a/The_Avengers_%282012_film%29_poster.jpg", "Avengers" },
                    { 2, false, new DateTime(2017, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://upload.wikimedia.org/wikipedia/en/9/98/Coco_%282017_film%29_poster.jpg", "Coco" },
                    { 3, false, new DateTime(2021, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://upload.wikimedia.org/wikipedia/en/0/00/Spider-Man_No_Way_Home_poster.jpg", "Spider-Man: No way home" },
                    { 4, false, new DateTime(2019, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://upload.wikimedia.org/wikipedia/en/0/00/Spider-Man_No_Way_Home_poster.jpg", "Spider-Man: Far From Home" },
                    { 5, true, new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Local), "https://upload.wikimedia.org/wikipedia/en/5/50/The_Matrix_Resurrections.jpg", "The Matrix Resurrections" }
                });

            migrationBuilder.InsertData(
                table: "CinesOfertas",
                columns: new[] { "Id", "CineId", "FechaFin", "FechaInicio", "PorcentajeDescuento" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 1, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Local), 10m },
                    { 2, 4, new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Local), 15m }
                });

            migrationBuilder.InsertData(
                table: "GeneroPelicula",
                columns: new[] { "GenerosIdentificador", "PeliculasId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 2, 2 },
                    { 3, 3 },
                    { 3, 4 },
                    { 4, 1 },
                    { 4, 3 },
                    { 4, 4 },
                    { 4, 5 },
                    { 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "PeliculasActores",
                columns: new[] { "ActorId", "PeliculaId", "Orden", "Personaje" },
                values: new object[,]
                {
                    { 3, 1, 2, "Iron Man" },
                    { 4, 1, 1, "Capitán América" },
                    { 7, 1, 3, "Black Widow" },
                    { 1, 3, 1, "Peter Parker" },
                    { 1, 4, 1, "Peter Parker" },
                    { 2, 4, 2, "Samuel L. Jackson" },
                    { 8, 5, 1, "Neo" }
                });

            migrationBuilder.InsertData(
                table: "SalasDeCine",
                columns: new[] { "Id", "CineId", "Precio", "TipoSalaDeCine" },
                values: new object[,]
                {
                    { 1, 1, 220m, 1 },
                    { 2, 1, 320m, 2 },
                    { 3, 2, 200m, 1 },
                    { 4, 2, 290m, 2 },
                    { 5, 3, 250m, 1 },
                    { 6, 3, 330m, 2 },
                    { 7, 3, 450m, 3 },
                    { 8, 4, 250m, 1 }
                });

            migrationBuilder.InsertData(
                table: "PeliculaSalaDeCine",
                columns: new[] { "PeliculasId", "SalasDeCineId" },
                values: new object[,]
                {
                    { 5, 1 },
                    { 5, 2 },
                    { 5, 3 },
                    { 5, 4 },
                    { 5, 5 },
                    { 5, 6 },
                    { 5, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinesOfertas_CineId",
                table: "CinesOfertas",
                column: "CineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneroPelicula_PeliculasId",
                table: "GeneroPelicula",
                column: "PeliculasId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_ActorId",
                table: "PeliculasActores",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaSalaDeCine_SalasDeCineId",
                table: "PeliculaSalaDeCine",
                column: "SalasDeCineId");

            migrationBuilder.CreateIndex(
                name: "IX_SalasDeCine_CineId",
                table: "SalasDeCine",
                column: "CineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinesOfertas");

            migrationBuilder.DropTable(
                name: "GeneroPelicula");

            migrationBuilder.DropTable(
                name: "PeliculasActores");

            migrationBuilder.DropTable(
                name: "PeliculaSalaDeCine");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Actores");

            migrationBuilder.DropTable(
                name: "Peliculas");

            migrationBuilder.DropTable(
                name: "SalasDeCine");

            migrationBuilder.DropTable(
                name: "Cines");
        }
    }
}
