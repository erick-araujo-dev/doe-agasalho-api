namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class ProdutoCreateModel
    {
        public int Id { get; set; }

        public string Ativo { get; set; } = null!;

        public string Caracteristica { get; set; } = null!;

        public string Tamanho { get; set; }

        public string Tipo { get; set; }

        public string Genero { get; set; } = null!;

        public int Estoque { get; set; }
    }
}