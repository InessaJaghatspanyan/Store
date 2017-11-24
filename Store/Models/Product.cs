using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Display(Name ="Code")]
        [Required(ErrorMessage ="{0} is required")]
        [Range(1, 99999, ErrorMessage = "{0} should be between 1-99999")]
        public int Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(15, MinimumLength = 5,ErrorMessage ="{0} should be 5-15 charecters")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required")]
        [Range(10,5000,ErrorMessage ="{0} should be between 10-5000")]
        public int Price { get; set; }

        [Display(Name = "Barcode")]
        [StringLength(13,MinimumLength =13,ErrorMessage = "{0} should be 13 charecters")]
        public string Barcode { get; set; }
    }
}