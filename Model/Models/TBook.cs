using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dbContext.Models;

[Table("t_Book")]
[Index("PhoneNumber", Name = "UQ__t_Book__17A35CA4B1806A93", IsUnique = true)]
public partial class TBook
{
    [Key]
    [Column("Book_Id")]
    public Guid BookId { get; set; }

    [StringLength(120)]
    public string FistName { get; set; } = null!;

    [StringLength(120)]
    public string LastName { get; set; } = null!;

    [Column("Phone_Number")]
    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

    [Column("Registration_Date", TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }
}
