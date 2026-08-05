namespace UnitransporteWeb.Models;

public record RouteLineViewModel(string Origem, string Destino, StatusEncomenda Status)
{
    public int Progresso => Status switch
    {
        StatusEncomenda.Pendente => 0,
        StatusEncomenda.Coletada => 50,
        StatusEncomenda.Entregue => 100,
        _ => 0,
    };

    public bool Cancelada => Status == StatusEncomenda.Cancelada;
}
