using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCommunity.Core.Interfaces
{
    public interface IFileService
    {
        Task UploadFile(IFormFile)
    }
}
