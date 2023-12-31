﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLayer.Core.Models;

namespace NLayer.Repository.Seeds
{
    internal class ProductSeed : IEntityTypeConfiguration<Product>
    {


        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(new Product
            {
                Id = 1,
                Name = "Kalem 1",
                CategoryId = 1,
                Price = 100, Stock = 20,
                CreatedDate = DateTime.Now,

            },
            new Product
            {
                Id = 2,
                Name = "Kalem 2",
                CategoryId = 1,
                Price = 100,
                Stock = 20,
                CreatedDate = DateTime.Now,

            },
            new Product
            {
                Id = 3,
                Name = "Kalem 3",
                CategoryId = 1,
                Price = 100,
                Stock = 20,
                CreatedDate = DateTime.Now,

            },
            new Product
            {
                Id = 4,
                Name = "Kitap 1",
                CategoryId = 2,
                Price = 100,
                Stock = 20,
                CreatedDate = DateTime.Now,

            }

            );

        }
    }
}
