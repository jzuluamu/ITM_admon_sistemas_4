using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Core.Interfaces.Servicios;
using apiFestivos.Dominio.DTOs;
using apiFestivos.Dominio.Entidades;
using Moq;

namespace apiFestivos.Aplicacion.Servicios
{
    public class FestivoServicioTestable : FestivoServicio
    {
        public FestivoServicioTestable(IFestivoRepositorio repo) : base(repo) { }

        public FechaFestivo Invoke_ObtenerFestivo(int año, Festivo festivo)
        {
            return typeof(FestivoServicio)
                .GetMethod("ObtenerFestivo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(this, new object[] { año, festivo }) as FechaFestivo;
        }
    }
    public class ObtenerFestivoTests
    {
        [Fact]
        public void ObtenerFestivo_Tipo1_FechaEsperada()
        {
            // Arrange
            var mockRepo = new Mock<IFestivoRepositorio>();
            var servicio = new FestivoServicioTestable(mockRepo.Object);

            var festivo = new Festivo
            {
                IdTipo = 1,
                Nombre = "Año Nuevo",
                Mes = 1,
                Dia = 1
            };

            // Act
            var resultado = servicio.Invoke_ObtenerFestivo(2025, festivo);

            // Assert
            Assert.Equal(new DateTime(2025, 1, 1), resultado.Fecha);
            Assert.Equal("Año Nuevo", resultado.Nombre);
        }

        [Fact]
        public void ObtenerFestivo_Tipo2_MovibleAlLunes()
        {
            // Arrange
            var mockRepo = new Mock<IFestivoRepositorio>();
            var servicio = new FestivoServicioTestable(mockRepo.Object);

            var festivo = new Festivo
            {
                IdTipo = 2,
                Nombre = "Día de la Raza",
                Mes = 10,
                Dia = 12 
            };

            // Act
            var resultado = servicio.Invoke_ObtenerFestivo(2025, festivo);

            // Assert
            Assert.Equal(new DateTime(2025, 10, 13), resultado.Fecha); //Aproximado a lunes
        }

        [Fact]
        public void ObtenerFestivo_Tipo4_SemanaSantaLunes()
        {
            // Arrange
            var mockRepo = new Mock<IFestivoRepositorio>();
            var servicio = new FestivoServicioTestable(mockRepo.Object);

            var festivo = new Festivo
            {
                IdTipo = 4,
                Nombre = "Ascensión del Señor",
                DiasPascua = 43 
            };

            // Act
            var resultado = servicio.Invoke_ObtenerFestivo(2025, festivo);

            // Semana Santa 2025 comienza el 13 de abril, Pascua el 20
            var esperado = new DateTime(2025, 6, 2);

            // Assert
            Assert.Equal(esperado, resultado.Fecha);
        }
    }


}
