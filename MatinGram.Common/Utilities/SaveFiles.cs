using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatinGram.Common.Utilities
{
    public static class SaveFiles
    {
        public static UploadDto UploadFile(this IFormFile file, string folder, IHostingEnvironment environment)
        {
            if (file != null)
            {
                //string folder = $@"images\ProductImages\";
                var uploadsRootFolder = Path.Combine(environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }


                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }

        public static Task<UploadDto> UploadFileAsync(this IFormFile file, string folder, IHostingEnvironment environment)
        {
            return Task.Run(() =>
            {
                if (file != null)
                {
                    //string folder = $@"images\ProductImages\";
                    var uploadsRootFolder = Path.Combine(environment.WebRootPath, folder);
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }


                    if (file == null || file.Length == 0)
                    {
                        return new UploadDto()
                        {
                            Status = false,
                            FileNameAddress = "",
                        };
                    }

                    string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                    var filePath = Path.Combine(uploadsRootFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    return new UploadDto()
                    {
                        FileNameAddress = folder + fileName,
                        Status = true,
                    };
                }
                return null;
            });
        }
    }

    public record UploadDto
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }
}
