using dbContext.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public interface IBook
    {
         Task<ActionResult<TBook>> GetAllPhoneBooks();
         Task<ActionResult<TBook>> GetPhoneBookBySearch([Required] string search);
         Task<IActionResult> AddPhoneBook(HBook hBook);
         Task<ActionResult<TBook>> UpdatePhoneNumberBy(string phoneNumber, HBookTitle Title);
         Task<ActionResult<TBook>> UpdatePhoneNumberBy(string FirstName, string LastName, HBookPhone Phone);
        Task<ActionResult<TBook>> UpdateContactById([Required] Guid BookId, [FromBody] HBook Book);



    }
}
