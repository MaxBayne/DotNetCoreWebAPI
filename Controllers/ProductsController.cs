using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DotNetCoreWebAPI.Models;

namespace DotNetCoreWebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private List<ProductModel> _products= new List<ProductModel>
        {
            new (){ Id=1,Name="Product 1",Suppliers=new List<SupplierModel>{new(){Id=1,Name="Ahmed"},new(){Id=2,Name="ALi"},new(){Id=3,Name="Kalid"}}},
            new (){ Id=2,Name="Product 2",Suppliers=new List<SupplierModel>{new(){Id=1,Name="Mona"},new(){Id=2,Name="Hoda"},new(){Id=3,Name="Alyya"}}},
            new (){ Id=3,Name="Product 3",Suppliers=new List<SupplierModel>()},
            new (){ Id=4,Name="Product 4",Suppliers=new List<SupplierModel>{new(){Id=1,Name="Mostafa"},new(){Id=2,Name="Magied"}}},
            new (){ Id=5,Name="Product 5",Suppliers=new List<SupplierModel>{new(){Id=1,Name="Ahmed"},new(){Id=2,Name="ALi"}}}
        };
        
        #region Routeing

        



        [Route("{id:int:min(5):max(100)}")]
        public IActionResult GetProductById(int id)
        {
            return Ok($"This is Product by Id  {id}");
        }

        [Route("{id:int:range(1,5)}")]
        public IActionResult GetProductById2(int id)
        {
            return Ok($"This is Product by Id  {id}");
        }

        [Route("{id:minlength(3):maxlength(5)}")]
        public IActionResult GetProductByIdString(string id)
        {
            return Ok($"This is Product by Id String {id}");
        }

        [Route("{id:length(3)}")]
        public IActionResult GetProductByIdString2(string id)
        {
            return Ok($"This is Product by Id String {id}");
        }

        [Route(@"{email:regex()}")]
        public IActionResult GetProductByEmail(string email)
        {
            return Ok($"This is Product by Email {email}");
        }

        [Route("{id:int:required}/author/{authorId:int}")]
        public IActionResult GetAuthorNameById(int id, int authorId)
        {
            return Ok($"Author Name is Mohammed and id is {authorId} inside Product {id}");
        }

        [Route("search")]
        //[Route("[controller]/[action]")] //use Token Replacement Feature
        public IActionResult SearchProduct(string productName)
        {
            return Ok($"we found product [{productName}] inside our database");
        }

        public IActionResult Aboutus()
        {
            return Ok("Iam Mohammed Salah , iam Software Enginner");
        }

        [Route("~/printinfo")]
        public IActionResult Printinfo()
        {
            return Ok("Iam Print Info Method");
        }



        #endregion

        #region GET ROUTE (Retreive Data)

        //GET: api/v1/products
        [HttpGet]
        public IActionResult Products()
        {
            return Ok(_products);
        }

        //GET: api/v1/products/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult getProductById(int id)
        {
            return Ok(_products[--id]);
        }

        //GET: api/v1/products/{id}/info
        [HttpGet]
        [Route("{id:int}/info")]
        public IActionResult getProductInfoById(int id)
        {
            return Ok(_products[--id].Name + " Information");
        }

        //GET: api/v1/products/{id}/suppliers
        [HttpGet]
        [Route("{id:int}/suppliers")]
        public IActionResult getProductSuppliersById(int id)
        {
            return Ok(_products[--id].Suppliers);
        }

        //GET: api/v1/products/{id}/suppliers/{supplierId}
        [HttpGet]
        [Route("{id:int}/suppliers/{supplierId:int}")]
        public IActionResult getProductSupplierById(int id, int supplierId)
        {
            return Ok(_products[--id].Suppliers[--supplierId]);
        }

        #endregion

        #region POST ROUTE (Insert Data)

        //POST: api/v1/products
        [HttpPost]
        public IActionResult AddProduct(ProductModel product)
        {
            _products.Add(product);

            return CreatedAtAction(nameof(getProductById),new {id=product.Id},product);
        }

        //POST: api/v1/products/{productId}/suppliers
        [Route("{productId:int}/suppliers")]
        [HttpPost]
        public IActionResult AddSupplierForProduct(int productId,SupplierModel supplier)
        {
            var product = _products[--productId];

            if (product == null)
            {
                return NotFound($"Product Id ({productId}) Not Exist");
            }

            product.Suppliers.Add(supplier);

            return Ok(product.Suppliers);
        }

        #endregion

        #region PUT ROUTE (Update Data)

        //PUT: api/v1/products/{productId}
        [HttpPut]
        public IActionResult UpdateProduct(int productId,ProductModel product)
        {
            var currentProduct = _products[--productId];

            if (currentProduct == null)
            {
                return NotFound($"Product With Id {productId} Not Exist");
            }

            currentProduct.Id = product.Id;
            currentProduct.Name = product.Name;
            currentProduct.Suppliers = product.Suppliers;


            return Ok(currentProduct);
        }

        //PUT: api/v1/products/{productId}/suppliers/{supplierId}
        [Route("{productId:int}/suppliers/{supplierId:int}")]
        [HttpPut]
        public IActionResult UpdateSupplierForProduct(int productId,int supplierId, SupplierModel supplier)
        {
            var product = _products[--productId];
            if (product == null)
            {
                return NotFound($"Product Id ({productId}) Not Exist");
            }

            var currentSupplier = product.Suppliers[--supplierId];
            if (currentSupplier == null)
            {
                return NotFound($"Supplier Id ({supplierId}) Not Exist");
            }

            currentSupplier.Id = supplier.Id;
            currentSupplier.Name = supplier.Name;


            return Ok(product.Suppliers);
        }

        #endregion

        #region PATCH ROUTE (Update Some Data)

        //PATCH: api/v1/products/{productId}
        [HttpPatch]
        public IActionResult UpdateProductName(int productId, string productName)
        {
            var currentProduct = _products[--productId];

            if (currentProduct == null)
            {
                return NotFound($"Product With Id {productId} Not Exist");
            }

            currentProduct.Name = productName;

            return Ok(currentProduct);
        }

        #endregion

        #region DELETE ROUTE (Remove Data)

        //DELETE: api/v1/products/{productId}
        [HttpDelete]
        public IActionResult DeleteProduct(int productId)
        {
            var currentProduct = _products[--productId];

            if (currentProduct == null)
            {
                return NotFound($"Product With Id {productId} Not Exist");
            }

            _products.Remove(currentProduct);

            return Ok(_products);
        }

        //DELETE: api/v1/products/{productId}/suppliers/{supplierId}
        [Route("{productId:int}/suppliers/{supplierId:int}")]
        [HttpDelete]
        public IActionResult DeleteSupplierForProduct(int productId, int supplierId)
        {

            var product = _products[--productId];
            if (product == null)
            {
                return NotFound($"Product Id ({productId}) Not Exist");
            }

            var currentSupplier = product.Suppliers[--supplierId];
            if (currentSupplier == null)
            {
                return NotFound($"Supplier Id ({supplierId}) Not Exist");
            }

            product.Suppliers.Remove(currentSupplier);


            return Ok(product.Suppliers);
        }

        #endregion

        #region Status Codes

        //200
        [Route("ok")]
        public IActionResult OkActionResult()
        {
            return Ok("Ok Results Status Code 200");
        }

        //201
        [Route("created")]
        public IActionResult CreatedActionResult()
        {
            return Created("~/api/v1/products/",null);
        }

      

        //202
        [Route("accepted")]
        public IActionResult AcceptedActionResult()
        {
            return Accepted("http://google.com", "Accepted Results Status Code 202");
        }

        //202
        [Route("acceptedataction")]
        public IActionResult AcceptedAtActionResult()
        {
            //Return URL Based On Action
            return AcceptedAtAction("RedirectFromAcceptedAtAction");
        }
        public IActionResult RedirectFromAcceptedAtAction()
        {
            return Ok("Redirect From AcceptedAtAction");
        }

        //301 : 
        [Route("localredirectpermanent")]
        public IActionResult LocalRedirectPermanentActionResult()
        {
            return LocalRedirectPermanent("~/api/v1/products");
        }

        //302 : return the result of another action without changing the url
        [Route("localredirect")]
        public IActionResult LocalRedirectActionResult()
        {
            return LocalRedirect("~/api/v1/products");
        }

        //400
        [Route("badrequest")]
        public IActionResult BadRequestActionResult()
        {
            return BadRequest(new string("an Error OCCured man for bad Request"));
        }

        //404
        [Route("notfound")]
        public IActionResult NotFoundActionResult()
        {
            return NotFound();
        }

        #endregion
    }
}
