using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class EstacionamentoMapping : IEntityTypeConfiguration<Estacionamento>
    {
        public void Configure(EntityTypeBuilder<Estacionamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Marca)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Modelo)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Placa)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Manobrado)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(p => p.PessoaId)
                .IsRequired();


            builder.ToTable("Estacionamentos");
        }
    }
}