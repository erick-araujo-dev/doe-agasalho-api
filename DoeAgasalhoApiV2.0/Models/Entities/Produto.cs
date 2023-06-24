using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class Produto
{
    public int Id { get; set; }

    public string Ativo { get; set; } = null!;

    public string Cor { get; set; } = null!;

    public int TamanhoId { get; set; }

    public int TipoId { get; set; }

    public string Genero { get; set; } = null!;

    public int Estoque { get; set; }

    public virtual ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();

    public virtual ICollection<PontoProduto> PontoProdutos { get; set; } = new List<PontoProduto>();

    public virtual Tamanho Tamanho { get; set; } = null!;

    public virtual Tipo Tipo { get; set; } = null!;
}
