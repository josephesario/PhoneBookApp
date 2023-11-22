using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class HBook
{
    [Key]
    [Column("Book_Id")]
    [Required]
    public Guid BookId { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = null!;

    [Required]
    [Column("Phone_Number")]
    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

}
