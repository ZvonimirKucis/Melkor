namespace Melkor_core_builder
{
    public class BuildItem
    {
        private string dir;
        private string status;

        public BuildItem(string dir, string status)
        {
            this.dir = dir;
            this.status = status;
        }

        public string Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
