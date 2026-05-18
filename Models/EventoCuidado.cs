using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace patinhasemdia.Models;

[Table("EVENTO_CUIDADO")]
public class EventoCuidado
{
    [Key]
    [Column("ID_EVENTO")]
    public int Id { get; set; }

    [Column("ID_PET")]
    public int PetId { get; set; }

    [ForeignKey("PetId")]
    public Pet? Pet { get; set; }

    [Required]
    [Column("TIPO_CUIDADO")]
    [MaxLength(50)]
    public string TipoCuidado { get; set; } = string.Empty;

    [Column("DATA_PREVISTA")]
    public DateTime? DataPrevista { get; set; }

    [Column("STATUS")]
    [MaxLength(30)]
    public string Status { get; set; } = "PENDENTE";

    [Column("PRIORIDADE")]
    [MaxLength(20)]
    public string? Prioridade { get; set; }

    [Column("OBSERVACAO")]
    [MaxLength(500)]
    public string? Observacao { get; set; }
}