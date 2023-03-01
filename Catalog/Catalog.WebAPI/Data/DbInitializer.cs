using CatalogWebAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogWebAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();

            if (!dbContext.CatalogCompanies.Any())
            {
                await dbContext.CatalogCompanies.AddRangeAsync(GetPreconfiguredCompanies());
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.CatalogGenres.Any())
            {
                await dbContext.CatalogGenres.AddRangeAsync(GetPreconfiguredGenres());
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.CatalogItems.Any())
            {
                await dbContext.CatalogItems.AddRangeAsync(GetPreconfiguredItems());
                await dbContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<CatalogCompanyEntity> GetPreconfiguredCompanies()
        {
            return new List<CatalogCompanyEntity>()
            {
                new CatalogCompanyEntity() { Id = 1, Company = "Valve" },
                new CatalogCompanyEntity() { Id = 2, Company = "Paradox Development Studio" },
                new CatalogCompanyEntity() { Id = 3, Company = "Konami" },
                new CatalogCompanyEntity() { Id = 4, Company = "Square Enix" },
                new CatalogCompanyEntity() { Id = 5, Company = "CD Projekt Red" },

                new CatalogCompanyEntity() { Id = 6, Company = "GSC Game World" }
            };
        }

        private static IEnumerable<CatalogGenreEntity> GetPreconfiguredGenres()
        {
            return new List<CatalogGenreEntity>()
            {
                new CatalogGenreEntity() { Id = 1, Genre = "First-person shooter" },
                new CatalogGenreEntity() { Id = 2, Genre = "Survival horror" },
                new CatalogGenreEntity() { Id = 3, Genre = "Grand Strategy" },
                new CatalogGenreEntity() { Id = 4, Genre = "Sports" },
                new CatalogGenreEntity() { Id = 5, Genre = "RPG" }
            };
        }

        private static IEnumerable<CatalogItemEntity> GetPreconfiguredItems()
        {
            return new List<CatalogItemEntity>()
            {
                new CatalogItemEntity() { Id = 1, Name = "Counter-Strike: Global Offensive", Price = 10.49m, Year = 2012, PictureFileName = "csgo_1.png", AvailableStock = 35, CatalogCompanyId = 1, CatalogGenreId = 1 },
                new CatalogItemEntity() { Id = 2, Name = "Europa Universalis IV", Price = 4.88m, Year = 2013, PictureFileName = "eu4_1.png", AvailableStock = 17, CatalogCompanyId = 2, CatalogGenreId = 3 },
                new CatalogItemEntity() { Id = 3, Name = "eFootball 2023", Price = 23m, Year = 2022, PictureFileName = "efootball2023_1.png", AvailableStock = 3, CatalogCompanyId = 3, CatalogGenreId = 4 },
                new CatalogItemEntity() { Id = 4, Name = "S.T.A.L.K.E.R.: Call of Pripyat", Price = 1.75m, Year = 2009, PictureFileName = "stalkerCall_1.png", AvailableStock = 56, CatalogCompanyId = 6, CatalogGenreId = 2 },
                new CatalogItemEntity() { Id = 5, Name = "Counter-Strike: Source", Price = 2.5m, Year = 2004, PictureFileName = "css_1.png", AvailableStock = 320, CatalogCompanyId = 1, CatalogGenreId = 1 },

                new CatalogItemEntity() { Id = 6, Name = "Cyberpunk 2077", Price = 18m, Year = 2020, PictureFileName = "cyberpunk_1.png", AvailableStock = 33, CatalogCompanyId = 5, CatalogGenreId = 5 },
                new CatalogItemEntity() { Id = 7, Name = "Counter-Strike 1.6", Price = 0.9m, Year = 2003, PictureFileName = "cs_1.png", AvailableStock = 123, CatalogCompanyId = 1, CatalogGenreId = 1 },
                new CatalogItemEntity() { Id = 8, Name = "Hearts Of Iron IV", Price = 3.33m, Year = 2016, PictureFileName = "hoi4_1.png", AvailableStock = 25, CatalogCompanyId = 2, CatalogGenreId = 3 },
                new CatalogItemEntity() { Id = 9, Name = "Oninaki", Price = 8m, Year = 2022, PictureFileName = "oninaki_1.png", AvailableStock = 2, CatalogCompanyId = 4, CatalogGenreId = 5 },
                new CatalogItemEntity() { Id = 10, Name = "The Witcher 3: Wild Hunt", Price = 6.35m, Year = 2015, PictureFileName = "witcher3_1.png", AvailableStock = 17, CatalogCompanyId = 5, CatalogGenreId = 5 },

                new CatalogItemEntity() { Id = 11, Name = "Sengoku", Price = 1.05m, Year = 2011, PictureFileName = "sengoku_1.png", AvailableStock = 7, CatalogCompanyId = 2, CatalogGenreId = 3 },
                new CatalogItemEntity() { Id = 12, Name = "Metal Gear Solid V: Ground Zeroes", Price = 4.95m, Year = 2014, PictureFileName = "mgs_1.png", AvailableStock = 5, CatalogCompanyId = 3, CatalogGenreId = 5 },
                new CatalogItemEntity() { Id = 13, Name = "Pro Evolution Soccer 2019", Price = 10.35m, Year = 2018, PictureFileName = "pes2019_1.png", AvailableStock = 37, CatalogCompanyId = 3, CatalogGenreId = 4 },
                new CatalogItemEntity() { Id = 14, Name = "S.T.A.L.K.E.R. 2: Heart of Chornobyl", Price = 49.99m, Year = 2023, PictureFileName = "stalker2_1.png", AvailableStock = 15, CatalogCompanyId = 6, CatalogGenreId = 2 },
                new CatalogItemEntity() { Id = 15, Name = "Victoria 3", Price = 25.05m, Year = 2022, PictureFileName = "victoria3_1.png", AvailableStock = 42, CatalogCompanyId = 2, CatalogGenreId = 3 }
            };
        }
    }
}
