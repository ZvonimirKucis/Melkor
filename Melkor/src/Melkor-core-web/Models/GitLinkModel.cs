using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Melkor_core_web.Models
{
    public class GitLinkModel
    {
        public string URL { get; set; }
        public int Zadatak { get; set; }

        public GitLinkModel(string url, int zadatak)
        {
            URL = url;
            Zadatak = zadatak;
        }

        public GitLinkModel()
        {
        }
    }
}
