using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dbContext.Models;

[Table("t_Book")]
[Index("PhoneNumber", Name = "UQ__t_Book__17A35CA426264E8C", IsUnique = true)]
[Index("Title", Name = "UQ__t_Book__2CB664DCEAFC2DEC", IsUnique = true)]
public partial class TBook
{
    [Key]
    [Column("Book_Id")]
    public Guid BookId { get; set; }

    [StringLength(120)]
    public string Title { get; set; } = null!;

    [Column("Phone_Number")]
    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

    [Column("Registration_Date", TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }
}
