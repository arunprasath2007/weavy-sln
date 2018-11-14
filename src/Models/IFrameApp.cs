using Doplace.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Weavy.Core.Attributes;
using Weavy.Core.Models;

namespace Wvy.Models
{
    [Serializable]
    [Guid("98871544-5eaf-4d5b-bc03-ee7af41f8106")]
    [App(Icon = "application", Name = "IFrame", Description = "Load any third party content here", AllowMultiple = true)]
    public class IFrameApp : App
    {
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Frame source", Description = "Full url of the resource to embed including http(s)://")]
        [Uri(ErrorMessage = "Must be a valid and fully-qualified url.")]
        public string Src { get; set; }

    }
}