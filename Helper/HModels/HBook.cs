using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbContext.Models;
public partial class HBook
{

    [Required]
    [StringLength(120)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(120)]
    public string LastName { get; set; } = null!;

    [Required]
    [Column("Phone_Number")]
    [RegularExpression(@"^\d{10}$")]
    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

}
