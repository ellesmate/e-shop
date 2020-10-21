using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.S3
{
    public class S3StorageSettings
    {
        public string Server { get; set; }
        public string Bucket { get; set; }
        public string RootPath { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
