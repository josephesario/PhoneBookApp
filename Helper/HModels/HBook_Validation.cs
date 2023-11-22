﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class HBook_Validation
{

    [StringLength(120)]
    public string FistName { get; set; } = null!;

    [StringLength(120)]
    public string LastName { get; set; } = null!;

    [Required]
    [Column("Phone_Number")]
    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

}