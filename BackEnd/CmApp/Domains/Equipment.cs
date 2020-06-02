using CodeMash.Models;

namespace CmApp.Domains
{
    public class Equipment
    {
        [Field("code")]
        public string Code { get; set; }
        [Field("name")]
        public string Name { get; set; }
    }
}
