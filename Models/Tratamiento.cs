namespace PWA.Models;

public class Tratamiento
{
    public string LocalId { get; set; } = Guid.NewGuid().ToString();
    public int? ServerId { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Dosis { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool Activo { get; set; } = true;
    public string Notas { get; set; } = string.Empty;

    public string SyncStatus { get; set; } = "Pending";
    public bool IsDeleted { get; set; } = false;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
}