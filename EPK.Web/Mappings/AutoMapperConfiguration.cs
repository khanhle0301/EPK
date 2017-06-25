using AutoMapper;
using EPK.Data.Models;
using EPK.Web.Models;

namespace EPK.Web.Mappings
{
    /// <summary>
    ///
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        ///
        /// </summary>
        public static void Configure()
        {
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<GiaHan, GiaHanViewModel>();
            Mapper.CreateMap<DmNhanVien, DmNhanVienViewModel>();
            Mapper.CreateMap<DmVeThang, DmVeThangViewModel>();
            Mapper.CreateMap<NhanVien, NhanVienViewModel>();
            Mapper.CreateMap<The, TheViewModel>();

            Mapper.CreateMap<Ra, RaViewModel>();
            Mapper.CreateMap<MayTinh, MayTinhViewModel>();
        }
    }
}