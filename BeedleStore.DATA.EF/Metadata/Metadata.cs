using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BeedleStore.DATA.EF.Models
{
    //internal class Metadata
    //{
    //}
    #region Category
    public class CategoryMetadata
	{
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [StringLength(50, ErrorMessage = "*Must be 50 characters or less")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "*Cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string? CategoryDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
    #endregion

    #region Order
    public class OrderMetadata
    {
        //nothing needed - this is a PK
        public int OrderId { get; set; }

        //no metadata needed for FKs - as they are represented in a View by a dropdown list
        public string UserId { get; set; } = null!;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]//0:d => MM/dd/yyyy
        [Display(Name = "Order Date")]
        [Required]
        public DateTime OrderDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Ship To")]
        [Required]
        public string ShipToName { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "City")]
        [Required]
        public string ShipCity { get; set; } = null!;

        [StringLength(2)]
        [Display(Name = "State")]
        public string? ShipState { get; set; }

        [StringLength(5)]
        [Display(Name = "Zip")]
        [Required]
        [DataType(DataType.PostalCode)]
        public string ShipZip { get; set; } = null!;
    }
    #endregion

    #region Product
    public class ProductMetadata
    {
        public int ProductId { get; set; }

        [StringLength(100, ErrorMessage = "*Cannot Exceed 100 characters")]
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "*Required")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "Price")]
        [Required(ErrorMessage = "*Required")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "*Cannot Exceed 500 characters")]
        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Discontinued")]
        public bool IsDiscontinued { get; set; }

        [Display(Name = "Amount Sold")]
        public int? AmountSold { get; set; }

        [Display(Name = "In Stock")]
        [Required(ErrorMessage = "*Required")]
        public short UnitsInStock { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "On Order")]
        public short UnitsOnOrder { get; set; }

        [Display(Name = "Product Received")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateReceived { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }

        [StringLength(75)]
        [Display(Name = "Image")]
        public string? ProductImage { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
    }
    #endregion

    #region Supplier
    public class SupplierMetadata
    {
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [StringLength(100, ErrorMessage = "*Cannot exceed 100 characters")]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; } = null!;

        [Required(ErrorMessage = "*Required")]
        [StringLength(100, ErrorMessage = "*Cannot exceed 100 characters")]
        public string Location { get; set; } = null!;

        [StringLength(24, ErrorMessage = "*Must be a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
    #endregion

    #region UserDetail
    public class UserDetailMetadata
    {
        public string UserId { get; set; } = null!;
        [StringLength(50)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; } = null!;
        [StringLength(150)]
        public string? Address { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [StringLength(2)]
        public string? State { get; set; }
        [StringLength(5)]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }
        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
    }
    #endregion

}
