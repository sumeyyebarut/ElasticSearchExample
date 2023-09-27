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
         new ConnectionSettings(new Uri("http://localhost:9200/"))//elastich seach çalýþtýðý url
                         .DefaultIndex("products")//indeksleme yapacagý default index adý
                         .DefaultMappingFor<Product>(m => m
                         .PropertyName(p => p.Id, "id") //id deðiþkenine hangi propertynin atýlacagýný belirlendi
             );

        private static readonly ElasticClient elasticClient = new ElasticClient(connSettings);//client deðiþkeni olusturuldu
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
                   );//ilgili indekste tutmak istediðimiz data nesnesini gönderiyoruz
            }
        } //ctor nesnesinde client üzerinden istek yaratmadan önce ayný isimde var mý diye kontrol etmemiz gerekiyor

        [HttpGet]
        [Route("GetFilterByPriceGreaterThan")]
        public List<Product> GetFilterByPriceGreaterThan()
        {
            //search metodu ile indekslenen data üzerinde filtreleme yapabiliriz
            var response = elasticClient.Search<Product>(i => i
           .Query(q => q.MatchAll())
           .PostFilter(f => f.Range(r => r.Field(fi => fi.Price).GreaterThan(2)))
            );//Price deðeri 2 den büyük olan deðerleri getirir.
            List<Product> products = new List<Product>();
            foreach (var item in response.Documents)
                products.Add(item);
            return products;

        }


    }
}