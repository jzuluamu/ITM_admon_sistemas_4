using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apiFestivos.Aplicacion.Servicios;
using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Dominio.Entidades;
using Moq;
using Xunit;

public class EsFestivoServicioTests
{
    [Fact]
    public async Task EsFestivo_FechaCoincideConFestivo_DeberiaRetornarTrue()
    {
        // Arrange
        var mockRepo = new Mock<IFestivoRepositorio>();
        mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(new List<Festivo>
        {
            new Festivo { Id = 1, Nombre = "Año Nuevo", Mes = 1, Dia = 1, IdTipo = 1 }
        });

        var servicio = new FestivoServicio(mockRepo.Object);
        var fecha = new DateTime(2025, 1, 1); // Año Nuevo

        // Act
        var esFestivo = await servicio.EsFestivo(fecha);

        // Assert
        Assert.True(esFestivo);
    }

    [Fact]
    public async Task EsFestivo_FechaNoCoincideConFestivo_DeberiaRetornarFalse()
    {
        // Arrange
        var mockRepo = new Mock<IFestivoRepositorio>();
        mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(new List<Festivo>
        {
            new Festivo { Id = 1, Nombre = "Año Nuevo", Mes = 1, Dia = 1, IdTipo = 1 }
        });

        var servicio = new FestivoServicio(mockRepo.Object);
        var fecha = new DateTime(2025, 2, 1); // No festivo

        // Act
        var esFestivo = await servicio.EsFestivo(fecha);

        // Assert
        Assert.False(esFestivo);
    }
    [Fact]
    public async Task EsFestivo_CuandoEsFestivoMovible_DeberiaRetornarTrue()
    {
        // Arrange
        var mockRepo = new Mock<IFestivoRepositorio>();
        mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(new List<Festivo>
    {
        new Festivo { Nombre = "Día de la Raza", Mes = 10, Dia = 12, IdTipo = 2 }
    });

        var servicio = new FestivoServicio(mockRepo.Object);

        // Día de la Raza en 2025 cae domingo 12 de octubre, se celebra el lunes 13
        var fecha = new DateTime(2025, 10, 13);

        // Act
        var resultado = await servicio.EsFestivo(fecha);

        // Assert
        Assert.True(resultado);
    }
}

