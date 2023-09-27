using ElasticSearchExample.DTO;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly ConnectionSettings connSettings =
         new ConnectionSettings(new Uri("http://localhost:9200/"))//elastich seach �al��t��� url
                         .DefaultIndex("products")//indeksleme yapacag� default index ad�
                         .DefaultMappingFor<Product>(m => m
                         .PropertyName(p => p.Id, "id") //id de�i�kenine hangi propertynin at�lacag�n� belirlendi
             );

        private static readonly ElasticClient elasticClient = new ElasticClient(connSettings);//client de�i�keni olusturuldu
        public ProductController()
        {

            if (!elasticClient.Indices.Exists("products").Exists)
            {
                elasticClient.Indices.Create("products",
                     index => index.Map<Product>(
                          x => x
                         .AutoMap()
                  ));

                elasticClient.Bulk(b => b
                  .Index("products")
                  .IndexMany(ProductService.GetItems())
                   );//ilgili indekste tutmak istedi�imiz data nesnesini g�nderiyoruz
            }
        } //ctor nesnesinde client �zerinden istek yaratmadan �nce ayn� isimde var m� diye kontrol etmemiz gerekiyor

        [HttpGet]
        [Route("GetFilterByPriceGreaterThan")]
        public List<Product> GetFilterByPriceGreaterThan()
        {
            //search metodu ile indekslenen data �zerinde filtreleme yapabiliriz
            var response = elasticClient.Search<Product>(i => i
           .Query(q => q.MatchAll())
           .PostFilter(f => f.Range(r => r.Field(fi => fi.Price).GreaterThan(2)))
            );//Price de�eri 2 den b�y�k olan de�erleri getirir.
            List<Product> products = new List<Product>();
            foreach (var item in response.Documents)
                products.Add(item);
            return products;

        }


    }
}