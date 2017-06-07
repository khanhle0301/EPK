using EPK.Common;
using EPK.Data.Common;
using EPK.Data.Models;
using EPK.Data.Resources;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPK.Service
{
    public interface INhanVienService
    {
        HttpResponseMessage GetAll();

        HttpResponseMessage Add(NhanVien nhanVien);

        HttpResponseMessage Update(NhanVien nhanVien);

        HttpResponseMessage GetById(int id);

        HttpResponseMessage Delete(int id);

        HttpResponseMessage DeleteMulti(string listId);
    }

    public class NhanVienService : INhanVienService
    {
        private readonly HttpClient _client;

        public NhanVienService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigHelper.GetByKey("CurrentLink"))
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CommonConstants.Token);
        }

        public HttpResponseMessage Add(NhanVien nhanVien)
        {
            return _client.PostAsJsonAsync(CurrentLink.AddNhanVien, nhanVien).Result;
        }

        public HttpResponseMessage GetById(int id)
        {
            return _client.GetAsync(CurrentLink.GetByIdNhanVien + "/" + id).Result;
        }

        public HttpResponseMessage Update(NhanVien nhanVien)
        {
            return _client.PutAsJsonAsync(CurrentLink.UpdateNhanVien, nhanVien).Result;
        }

        public HttpResponseMessage GetAll()
        {
            return _client.GetAsync(CurrentLink.GetAllNhanVien).Result;
        }

        public HttpResponseMessage Delete(int id)
        {
            return _client.DeleteAsync(CurrentLink.DeleteNhanVien + "?id=" + id).Result;
        }

        public HttpResponseMessage DeleteMulti(string listId)
        {
            return _client.DeleteAsync(CurrentLink.DeleteMultiNhanVien + "?listId=" + listId).Result;
        }

    }
}