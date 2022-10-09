using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlpaTabApi.Helpers;

namespace AlpaTabApi.Models;

public class AlpaTabUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string NickName { get; set; } 

    public string FirstName { get; set; }

    [MinLength(2)]
    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public UserType UserType { get; set; }

    public double Balance { get; set; }

}
