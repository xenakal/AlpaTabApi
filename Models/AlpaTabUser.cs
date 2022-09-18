using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlpaTabApi.Helpers;

namespace AlpaTabApi.Models
{
    public class AlpaTabUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public double Balance { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    
    public class AlpaTabUserDTO
    {
        public int UserID { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public double Balance { get; set; }

        public AlpaTabUserDTO(AlpaTabUser user)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            NickName =user.NickName;
            LastName = user.LastName;
            Email = user.Email;
            UserType = user.UserType;
            Balance = user.Balance;
        }
    }
}
