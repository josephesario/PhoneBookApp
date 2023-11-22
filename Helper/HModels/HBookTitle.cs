using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class HBookTitle
{
    [Required]
    [StringLength(120)]
    public string Title { get; set; } = null!;

}
