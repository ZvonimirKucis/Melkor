using Microsoft.Build.Execution;

namespace Melkor_core_builder
{
    public class BuildItem
    {
        public BuildItem(string dir, bool status)
        {
            Dir = dir;
            Status = status;
        }

        public string Dir { get; set; }
        public bool Status { get; set; }
    }
}