namespace AlpaTabApi.Dtos;

using System.ComponentModel.DataAnnotations;

public class TransactionReadDto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string NickName { get; set; }

    [Required]
    public double Amount { get; set; } 

    [Required]
    public string TransactionType { get; set; }

    [DataType(DataType.Date)]
    public DateTime Timestamp { get; set; } 

    public string Description { get; set; }
}

// restrict id
public class TransactionWriteDto
{
    [Required]
    public string NickName { get; set; }

    [Required]
    public double Amount { get; set; } 

    [Required]
    public string TransactionType { get; set; }

    [DataType(DataType.Date)]
    public DateTime Timestamp { get; set; } 


    public string Description { get; set; }
}
