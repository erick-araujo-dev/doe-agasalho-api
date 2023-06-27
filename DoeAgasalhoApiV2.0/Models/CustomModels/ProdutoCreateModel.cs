namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class ProdutoCreateModel
    {
        public int Id { get; set; }

        public string Ativo { get; set; } = null!;

        public string Caracteristica { get; set; } = null!;

        public int TamanhoId { get; set; }

        public int TipoId { get; set; }

        public string Genero { get; set; } = null!;

        public int Estoque { get; set; }
    }
}