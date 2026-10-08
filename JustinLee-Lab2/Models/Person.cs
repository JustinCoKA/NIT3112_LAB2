using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
// The assessment requires inheritance: both people share contact fields.
public abstract class Person
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Enter the person's full name.")]
    [StringLength(80, ErrorMessage = "The full name must contain no more than 80 characters.")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Enter a contact email address.")]
    [EmailAddress(ErrorMessage = "Enter an email address such as name@example.com.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Enter a contact phone number.")]
    [Phone(ErrorMessage = "Enter a valid contact phone number.")]
    public string Phone { get; set; }
}
}
