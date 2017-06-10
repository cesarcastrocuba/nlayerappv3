namespace NLayerApp.Application.MainBoundedContext.DTO.Profiles
{
    using AutoMapper;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;

    public class BankingProfile
        : Profile
    {
        public BankingProfile()
        {
            //bankAccount => BankAccountDTO
            CreateMap<BankAccount, BankAccountDTO>()
                .ForMember(dto => dto.BankAccountNumber, mc => mc.MapFrom(e => e.Iban))
                .PreserveReferences();

            //bankAccountActivity=>bankaccountactivityDTO
            CreateMap<BankAccountActivity, BankActivityDTO>()
                .PreserveReferences();

        }
    }
}
