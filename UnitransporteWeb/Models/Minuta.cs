namespace UnitransporteWeb.Models;

public class Minuta
{
    public string Id { get; set; } = "";
    public int NumeroDaMinuta { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public string NomeDoMotorista { get; set; } = "";
    public string NomeDoRecebedor { get; set; } = "";
    public string EncomendaId { get; set; } = "";
}
