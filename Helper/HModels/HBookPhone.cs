using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class HBookPhone
{




    [Required]
    [Column("Phone_Number")]
    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

}
