using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class HBookTitle
{

    [Required]
    [StringLength(120)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(120)]
    public string LastName { get; set; } = null!;

}
