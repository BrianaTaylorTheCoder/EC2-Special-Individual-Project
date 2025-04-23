using AutoMapper;
using EC2_2100212.Models;
using EC2_2100212.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace EC2_2100212.Web.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Bag, BagViewModel>().ReverseMap();
            CreateMap<Bag, CreateBagViewModel>().ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, CreateUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, LoginViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<Material, MaterialViewModel>().ReverseMap();
            CreateMap<Material, CreateMaterialViewModel>().ReverseMap();

            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CreateCategoryViewModel>().ReverseMap();

            CreateMap<BagMaterial, BagMaterialViewModel>().ReverseMap();
            CreateMap<BagMaterial, CreateBagMaterialViewModel>().ReverseMap();

            CreateMap<BagCategory, BagCategoryViewModel>().ReverseMap();
            CreateMap<BagCategory, CreateBagCategoryViewModel>().ReverseMap();

            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<Order, CreateOrderViewModel>().ReverseMap();

        }


    }
}
