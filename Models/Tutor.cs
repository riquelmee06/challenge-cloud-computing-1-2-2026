using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace patinhasemdia.Models;

[Table("TUTOR")]
public class Tutor
{
    [Key]
    [Column("ID_TUTOR")]
    public int Id { get; set; }

    [Required]
    [Column("NOME")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Column("EMAIL")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Column("TELEFONE")]
    [MaxLength(20)]
    public string? Telefone { get; set; }

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}