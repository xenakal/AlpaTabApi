namespace AlpaTabApi.Dtos;

using System.ComponentModel.DataAnnotations;

public class TransactionReadDto
{
    public int Id { get; set; }

    public string NickName { get; set; }

    public double Amount { get; set; } 

    public string TransactionType { get; set; }

    [DataType(DataType.Date)]
    public DateTime Timestamp { get; set; } 

    public string Description { get; set; }
}

// restrict id
public class TransactionWriteDto
{
    public string NickName { get; set; }

    public double Amount { get; set; } 

    public string TransactionType { get; set; }

    [DataType(DataType.Date)]
    public DateTime Timestamp { get; set; } 

    public string Description { get; set; }
}
