using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace patinhasemdia.Models;

[Table("PET")]
public class Pet
{
    [Key]
    [Column("ID_PET")]
    public int Id { get; set; }

    [Required]
    [Column("NOME")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Column("ESPECIE")]
    [MaxLength(50)]
    public string? Especie { get; set; }

    [Column("RACA")]
    [MaxLength(50)]
    public string? Raca { get; set; }

    [Column("IDADE")]
    public int? Idade { get; set; }

    [Column("ID_TUTOR")]
    public int TutorId { get; set; }

    [ForeignKey("TutorId")]
    public Tutor? Tutor { get; set; }

    public ICollection<EventoCuidado> Eventos { get; set; } = new List<EventoCuidado>();
}