using CodeMash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Entities
{
    [Collection("Images")]
    public class ImagesEntity : Entity
    {
        [Field("url")]
        public string Url { get; set; }
    }
}
