namespace DoeAgasalhoApiV2._0.Models.CustomModels
{
    public class DoacaoCreateModel
    {
        public int Id { get; set; }

        public string TipoMovimento { get; set; };

        public DateOnly DataMovimento { get; set; }

        public int Quantidade { get; set; }

        public int ProdutoId { get; set; }

        public int UsuarioId { get; set; }
    }
}
