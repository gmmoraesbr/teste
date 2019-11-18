using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Documento)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(p => p.DataNascimento)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(p => p.TipoPessoa)
                .IsRequired()
                .HasColumnType("int");

            // 1 : 1 => Pessoa : Estacionamento
            builder.HasOne(f => f.Estacionamento)
                .WithOne(e => e.Pessoa)
                .HasForeignKey<Estacionamento>(b => b.PessoaId);

            builder.ToTable("Pessoas");
        }
    }
}