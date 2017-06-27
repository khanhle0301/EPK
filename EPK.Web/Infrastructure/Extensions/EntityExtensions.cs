using EPK.Data.Models;
using EPK.Web.Models;

namespace EPK.Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.Id = appGroupViewModel.Id;
            appGroup.Name = appGroupViewModel.Name;
            appGroup.Description = appGroupViewModel.Description;
            appGroup.Roles = appGroupViewModel.Roles;
            appGroup.DangSuDung = appGroupViewModel.DangSuDung;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel, string action = "add")
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Address = appUserViewModel.Address;
            appUser.Luong = appUserViewModel.Luong;
            appUser.GhiChu = appUserViewModel.GhiChu;
            appUser.DangSuDung = appUserViewModel.DangSuDung;
            appUser.Groups = appUserViewModel.Groups;
            appUser.PasswordHash = appUserViewModel.Password;
        }
    }
}