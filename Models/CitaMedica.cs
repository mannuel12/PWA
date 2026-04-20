namespace PWA.Models;

public class CitaMedica
{
    public string LocalId { get; set; } = Guid.NewGuid().ToString();
    public int? ServerId { get; set; }

    public string Lugar { get; set; } = string.Empty;
    public string TipoConsulta { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string Notas { get; set; } = string.Empty;

    public string SyncStatus { get; set; } = "Pending";
    public bool IsDeleted { get; set; } = false;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
}