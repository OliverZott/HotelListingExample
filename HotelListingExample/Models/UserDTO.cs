using System.ComponentModel.DataAnnotations;

namespace HotelListingExample.Models
{
    // Mapper doesn't need to know about this DTO
    public class LoginUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        //[DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters.", MinimumLength = 6)]
        public string Password { get; set; }
    }
    public class UserDto : LoginUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


    }
}
