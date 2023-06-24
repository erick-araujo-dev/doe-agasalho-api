using System;
using System.Collections.Generic;
using DoeAgasalhoApiV2._0.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoeAgasalhoApiV2._0.Data.Context;

public partial class DbDoeagasalhov2Context : DbContext
{
    public DbDoeagasalhov2Context()
    {
    }

    public DbDoeagasalhov2Context(DbContextOptions<DbDoeagasalhov2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Doacao> Doacoes { get; set; }

    public virtual DbSet<Endereco> Enderecos { get; set; }

    public virtual DbSet<PontoColeta> PontoColeta { get; set; }

    public virtual DbSet<PontoProduto> PontoProdutos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Tamanho> Tamanhos { get; set; }

    public virtual DbSet<Tipo> Tipos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Doacao>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ProdutoId, e.UsuarioId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("doacao");

            entity.HasIndex(e => e.ProdutoId, "fk_doacao_produto1_idx");

            entity.HasIndex(e => e.UsuarioId, "fk_doacao_usuario1_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.DataMovimento).HasColumnName("data_movimento");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");
            entity.Property(e => e.TipoMovimento)
                .HasColumnType("enum('entrada','saida')")
                .HasColumnName("tipo_movimento");

            entity.HasOne(d => d.Produto).WithMany(p => p.Doacoes)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doacao_produto1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Doacoes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doacao_usuario1");
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("endereco");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(9)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(10)
                .HasColumnName("complemento");
            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .HasColumnName("estado");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(50)
                .HasColumnName("logradouro");
            entity.Property(e => e.Numero).HasColumnName("numero");
        });

        modelBuilder.Entity<PontoColeta>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.EnderecoId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("ponto_coleta");

            entity.HasIndex(e => e.EnderecoId, "fk_ponto_coleta_endereco1_idx");

            entity.HasIndex(e => e.NomePonto, "nome_ponto_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.EnderecoId).HasColumnName("endereco_id");
            entity.Property(e => e.Ativo)
                .HasMaxLength(1)
                .HasColumnName("ativo");
            entity.Property(e => e.NomePonto)
                .HasMaxLength(50)
                .HasColumnName("nome_ponto");

            entity.HasOne(d => d.Endereco).WithMany(p => p.PontoColeta)
                .HasForeignKey(d => d.EnderecoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ponto_coleta_endereco1");
        });

        modelBuilder.Entity<PontoProduto>(entity =>
        {
            entity.HasKey(e => new { e.PontoColetaId, e.ProdutoId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("ponto_produto");

            entity.HasIndex(e => e.PontoColetaId, "fk_ponto_coleta_has_produtos_ponto_coleta1_idx");

            entity.HasIndex(e => e.ProdutoId, "fk_ponto_coleta_has_produtos_produtos1_idx");

            entity.Property(e => e.PontoColetaId).HasColumnName("ponto_coleta_id");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");

            entity.HasOne(d => d.Produto).WithMany(p => p.PontoProdutos)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ponto_coleta_has_produtos_produtos1");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.HasIndex(e => e.TamanhoId, "fk_produtos_tamanhos1_idx");

            entity.HasIndex(e => e.TipoId, "fk_produtos_tipos1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativo)
                .HasMaxLength(1)
                .HasColumnName("ativo");
            entity.Property(e => e.Cor)
                .HasMaxLength(30)
                .HasColumnName("cor");
            entity.Property(e => e.Estoque).HasColumnName("estoque");
            entity.Property(e => e.Genero)
                .HasColumnType("enum('M','F','U')")
                .HasColumnName("genero");
            entity.Property(e => e.TamanhoId).HasColumnName("tamanho_id");
            entity.Property(e => e.TipoId).HasColumnName("tipo_id");

            entity.HasOne(d => d.Tamanho).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.TamanhoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produtos_tamanhos1");

            entity.HasOne(d => d.Tipo).WithMany(p => p.Produtos)
                .HasForeignKey(d => d.TipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produtos_tipos1");
        });

        modelBuilder.Entity<Tamanho>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tamanho");

            entity.HasIndex(e => e.Nome, "descricao_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Tipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipo");

            entity.HasIndex(e => e.Nome, "descricao_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PontoColetaId, "fk_usuario_ponto_coleta1_idx");

            entity.HasIndex(e => e.Nome, "username_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativo)
                .HasMaxLength(1)
                .HasColumnName("ativo");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.PontoColetaId).HasColumnName("ponto_coleta_id");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");
            entity.Property(e => e.Tipo)
                .HasColumnType("enum('admin','normal')")
                .HasColumnName("tipo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
