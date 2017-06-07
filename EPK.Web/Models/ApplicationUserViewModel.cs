using System;

namespace EPK.Web.Models
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicationUserViewModel
    {
        public string Id { set; get; }
        public string FullName { set; get; }
        public DateTime BirthDay { set; get; }
        public string Bio { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string Address { set; get; }

        public string PhoneNumber { set; get; }
    }
}