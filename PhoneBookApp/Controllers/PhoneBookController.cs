using Contracts.Contracts;
using dbContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace PhoneBookApp.Controllers
{
    /// <summary>
    /// Controller for managing phone books.
    /// </summary>
    [ApiController]
    [Route("api/[Controller]")]
    public class PhoneBookController : ControllerBase, IBook
    {

        private readonly PhoneBookDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneBookController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public PhoneBookController(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all phone books from the database.
        /// </summary>
        /// <returns>
        /// Returns a list of <see cref="TBook"/> representing the phone books.
        /// </returns>
        [HttpGet("GetAllPhoneBooks")]
        public async Task<ActionResult<TBook>> GetAllPhoneBooks()
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var output = await _dbContext.TBooks.ToListAsync();
                if (output.Count > 0)
                {
                    transaction.Commit();
                    return Ok(output);
                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exception: Date {DateTime.Now} - Exception Info - {ex}");
            }
        }



        /// <summary>
        /// Retrieves phone books by searching for a given last name.
        /// </summary>
        /// <param name="search">The last name to search for.</param>
        /// <returns>Returns a list of <see cref="TBook"/> representing the phone books matching the search criteria.</returns>
        [HttpGet("GetPhoneBookBySearch/{search}")]
        public async Task<ActionResult<TBook>> GetPhoneBookBySearch([Required] string search)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {

                var output = await _dbContext.TBooks.Where(e => e.LastName.Contains(search) ).ToListAsync();

                if (output.Count > 0)
                {
                    transaction.Commit();
                    return Ok(output);
                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }
        }


        /// <summary>
        /// Handles the insertion of a new phone book entry.
        /// </summary>
        /// <param name="hBook">The phone book entry to add.</param>
        /// <returns>Returns IActionResult indicating the result of the insertion operation.</returns>
        [HttpPost("AddPhoneBook")]
        public async Task<IActionResult> AddPhoneBook([FromBody] HBook hBook)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try {

               

                if (hBook != null) {


                    HBook_Validation hBook_Validation = new()
                    {
                        PhoneNumber = hBook.PhoneNumber,
                        FirstName = hBook.FirstName,
                        LastName = hBook.LastName
                    };

                    bool accountExists = await BookAlreadyExistsAsync(hBook_Validation);

                    if (accountExists)
                        return Conflict("Contact Already Exist");


                    TBook book = new()
                    {
                        PhoneNumber = hBook.PhoneNumber,
                        RegistrationDate = DateTime.Now,
                        FirstName = hBook.FirstName,
                        LastName = hBook.LastName,

                    };

                    var output = await _dbContext.TBooks.AddAsync(book);
                    var output1 = _dbContext.SaveChanges();

                    if (output1 > 0) {
                         transaction.Commit();
                         return Ok("Inserted Successfully");
                    }
                    else
                    {
                        transaction.Rollback();
                        return BadRequest("Insertion Failed");
                    }
                     

                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            } catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }

        }


        /// <summary>
        /// Updates the phone number for a phone book entry by title.
        /// </summary>
        /// <param name="phoneNumber">The phone number to update.</param>
        /// <param name="Title">The new title information.</param>
        /// <returns>Returns IActionResult indicating the result of the update operation.</returns>
        [HttpPatch("UpdatePhoneNumberBy/{phoneNumber}")]
        public async Task<ActionResult<TBook>> UpdatePhoneNumberBy([Required][RegularExpression(@"^\d{10}$")]  string phoneNumber , [FromBody] HBookTitle Title)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
              
                var output = await _dbContext.TBooks.Where(e => e.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();

                if (output != null)
                {

                    bool status0 = await BookFullNameAlreadyExistsAsync(Title.FirstName,Title.LastName);
                    bool status1 = await BookAlPhonereadyExistsAsync(phoneNumber);

                    if (status1)
                    {
                        if (status0)
                        {
                            transaction.Rollback();
                            return Conflict("This Contact Already Exist, Use A different Title!!");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return NotFound();
                    }

                    output.FirstName = Title.FirstName;
                    output.LastName = Title.LastName;

                    var output1 = _dbContext.SaveChanges();

                    if (output1 > 0) { 
                        transaction.Commit();
                        return Ok("Updated Successfully");
                    }
                    else {
                        transaction.Rollback();
                        return BadRequest("Update Failed");
                    }


                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }
        }


        /// <summary>
        /// Updates the phone number for a phone book entry by full name.
        /// </summary>
        /// <param name="FirstName">The first name of the entry to update.</param>
        /// <param name="LastName">The last name of the entry to update.</param>
        /// <param name="Phone">The new phone number information.</param>
        /// <returns>Returns IActionResult indicating the result of the update operation.</returns>
        [HttpPatch("UpdatePhoneNumberBy/{FirstName}/{LastName}")]
        public async Task<ActionResult<TBook>> UpdatePhoneNumberBy([Required] string FirstName, [Required] string LastName, [FromBody] HBookPhone Phone)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try { 

                var output = await _dbContext.TBooks.Where(e=>e.FirstName.Equals(FirstName) && e.LastName.Equals(LastName)).FirstOrDefaultAsync();
                
                if(output != null) {


                    bool status0  = await BookFullNameAlreadyExistsAsync(FirstName, LastName);
                    bool status1 = await BookAlPhonereadyExistsAsync(Phone.PhoneNumber);

                    if (status0)
                    {
                        if (status1)
                        {
                            transaction.Rollback();
                            return Conflict("Phone Number Already Exist, Use A different Phone number!!");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return NotFound();
                    }

                    output.PhoneNumber = Phone.PhoneNumber;
                    var output1 = _dbContext.SaveChanges();

                    if (output1 > 0) {

                        transaction.Commit();
                        return Ok("Updated Successfully");
                    }
                    else 
                    {
                        transaction.Rollback();
                        return BadRequest("Update Failed");
                    }


                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }

            }catch(Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exeption: Date {DateTime.Now} - Exeption Info - {ex}");
            }
        }



        /// <summary>
        /// Updates a contact by its unique identifier.
        /// </summary>
        /// <param name="BookId">The unique identifier of the contact to update.</param>
        /// <param name="Book">The updated information for the contact.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{TBook}"/> representing the result of the update operation.
        /// </returns>
        [HttpPut("UpdateContactById/{BookId}")]
        public async Task<ActionResult<TBook>> UpdateContactById([Required] Guid BookId, [FromBody] HBook Book)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var output = await _dbContext.TBooks.Where(e => e.BookId.Equals(BookId)).FirstOrDefaultAsync();

                if (output != null)
                {
                    bool status0 = await BookFullNameAlreadyExistsAsync(Book.FirstName, Book.LastName);
                    bool status1 = await BookAlPhonereadyExistsAsync(Book.PhoneNumber);

                    if (status0 || status1)
                    {
                        transaction.Rollback();
                        return Conflict("Ups!! Contact Already Exist.");
                    }
                    else
                    {
                        output.PhoneNumber = Book.PhoneNumber;
                        output.FirstName = Book.FirstName;
                        output.LastName = Book.LastName;

                        var output1 = _dbContext.SaveChanges();

                        if (output1 > 0)
                        {
                            transaction.Commit();
                            return Ok("Updated Successfully");
                        }
                        else
                        {
                            transaction.Rollback();
                            return BadRequest("Update Failed");
                        }
                    }
                }
                else
                {
                    transaction.Rollback();
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exception: Date {DateTime.Now} - Exception Info - {ex}");
            }
        }




        /// <summary>
        /// Deletes a phone book entry by phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number to delete.</param>
        /// <returns>Returns IActionResult indicating the result of the deletion operation.</returns>
        [HttpDelete("DeleteByPhoneNumber/{phoneNumber}")]
        public async Task<ActionResult> Delete([Required][RegularExpression(@"^\d{10}$")] string phoneNumber)
        {
            
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var output0 = await _dbContext.TBooks.SingleOrDefaultAsync(e => e.PhoneNumber.Equals(phoneNumber));

                if (output0 != null)
                {
                    _dbContext.TBooks.Remove(output0);
                    await _dbContext.SaveChangesAsync(); // Save changes before committing the transaction
                    transaction.Commit();
                    return Ok("Contact Deleted Successfully");
                }
                else
                {
                    transaction.Rollback();
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Exception: Date {DateTime.Now} - Exception Info - {ex}");
            }


        }


        private async Task<bool> BookAlreadyExistsAsync(HBook_Validation book_Validation)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.FirstName.Equals(book_Validation.FirstName) && e.LastName.Equals(book_Validation.LastName) || e.PhoneNumber.Equals(book_Validation.PhoneNumber));
            return output;
        }

        private async Task<bool> BookFullNameAlreadyExistsAsync(string  FirstName, string LastName)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.FirstName.Equals(FirstName) && e.LastName.Equals(LastName));
            return output;
        }

        private async Task<bool> BookAlPhonereadyExistsAsync(string phoneNumber)
        {

            var output = await _dbContext.TBooks.AnyAsync(e => e.PhoneNumber.Equals(phoneNumber));
            return output;
        }

    }
}