using AlpaTabApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AlpaTabApi.Dtos;

public class UserReadDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string NickName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public UserType UserType { get; set; }

    public double Balance { get; set; }
}

// restrict id, balance
public class UserWriteDto
{
    [Required]
    public string NickName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public UserType UserType { get; set; }
}
