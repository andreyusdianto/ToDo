using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Todo_RestAPI.Models
{
    public class FileReader
    {
        private readonly IHostingEnvironment env;
        public FileReader(IHostingEnvironment env)
        {
            if (env == null)
                throw new ArgumentNullException(nameof(env));

            this.env = env;
        }

        public string ReadFile(string fileName)
        {
            var filename = Path.Combine(env.WebRootPath, fileName);
            return filename;
        }
    }
}
