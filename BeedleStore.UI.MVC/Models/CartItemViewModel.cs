using BeedleStore.DATA.EF.Models;
using BeedleStore.UI.MVC.Models;

namespace BeedleStore.UI.MVC.Models
{
    public class CartItemViewModel
    {
        // Shopping Cart - Step 2
        // right clicked Models -> Add Class to create this file
        public int Qty { get; set; }
        public Product CartProd { get; set; }//Containment - using a complex datatype as a prop/field in a class
        // comples data types are classes that can hold multiple values
        // vs primitive data types, which hold a single value

        public CartItemViewModel() { }

        public CartItemViewModel(int qty, Product product)
        {
            Qty = qty;
            CartProd = product;
        }
    }
}
