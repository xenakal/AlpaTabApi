using AlpaTabApi.Dtos;
using AlpaTabApi.Models;
using AutoMapper;

namespace AlpaTabApi.Profiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        // Source -> Target
        CreateMap<AlpaTabTransaction, TransactionReadDto>();
        CreateMap<TransactionWriteDto, AlpaTabTransaction>();
    }
}
