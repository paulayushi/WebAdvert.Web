using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Repositories
{
    public interface IFileUploader
    {
        Task<bool> FileUploader(string filename, Stream filestream);
    }
}
