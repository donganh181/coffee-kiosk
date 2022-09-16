using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_kiosk_solution.Business.Services
{
    public interface IFileService
    {
        Task<string> UploadImageToFirebase(string image, string cateName, Guid id, string name);
    }
}
