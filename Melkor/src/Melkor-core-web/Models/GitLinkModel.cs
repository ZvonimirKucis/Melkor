using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Melkor_core_web.Models
{
    public class GitLinkModel
    {
        public string URL { get; set; }

        public GitLinkModel(string URL)
        {
            this.URL = URL;
        }

        public GitLinkModel()
        {
        }
    }
}
