namespace PWA.Models;

public class RegistroSalud
{
    public string LocalId { get; set; } = Guid.NewGuid().ToString();
    public int? ServerId { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    public decimal Peso { get; set; }
    public int PresionSistolica { get; set; }
    public int PresionDiastolica { get; set; }
    public string Notas { get; set; } = string.Empty;

    public string SyncStatus { get; set; } = "Pending";
    public bool IsDeleted { get; set; } = false;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
}