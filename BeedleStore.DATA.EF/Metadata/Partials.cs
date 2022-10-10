using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeedleStore.DATA.EF.Models
{
	//internal class Partials
	//{
	//}

	[ModelMetadataType(typeof(CategoryMetadata))]
	public partial class Category { }

	[ModelMetadataType(typeof(CategoryMetadata))]
	public partial class Product { }

	[ModelMetadataType(typeof(CategoryMetadata))]
	public partial class Supplier { }
}
