using System;
using System.Collections.Generic;

namespace DoeAgasalhoApiV2._0.Models.Entities;

public partial class Tamanho
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
