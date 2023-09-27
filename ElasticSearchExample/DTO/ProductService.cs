using Nest;

namespace ElasticSearchExample.DTO
{
    public static class ProductService
    {
       
            public static Product[] GetItems()
            {
                Product[] items = new[]
                {
                new Product
                {
                    Id = 1,
                    Quantity =1,
                    Name="Mouse",
                    Description="Mouse Description",
                    Price=1
                },
                new Product
                {
                    Id = 2,
                    Quantity =2,
                    Name="Computer",
                    Description="Computer Description",
                    Price=2
                },
                new Product
                {
                    Id = 3,
                    Quantity = 3,
                    Name="Keyboard",
                    Description="Keyboard Description",
                    Price=3
                }
            };
                return items;
            }
        }
    }

