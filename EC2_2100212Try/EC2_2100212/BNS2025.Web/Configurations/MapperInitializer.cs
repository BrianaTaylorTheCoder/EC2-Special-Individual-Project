using AutoMapper;
using BNS2025.ViewModels;
using BNS2025.Models;
using Microsoft.AspNetCore.Identity;

namespace BNS2025.Web.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Currency, CurrencyViewModel>().ReverseMap();
            CreateMap<Currency, CreateCurrencyViewModel>().ReverseMap();
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<Account, CreateAccountViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, LoginViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
            CreateMap<Transaction, TransactionViewModel>().ReverseMap();
            CreateMap<Transaction, CreateTransactionViewModel>().ReverseMap();
        }

    }
}
