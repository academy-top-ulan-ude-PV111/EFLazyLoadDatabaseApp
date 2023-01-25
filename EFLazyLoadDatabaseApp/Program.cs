using Microsoft.EntityFrameworkCore;

namespace EFLazyLoadDatabaseApp
{
    public class City
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
    }
    public class Country
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public int CapitalId { set; get; }
        public virtual City? Capital { set; get; }
        public virtual List<Company> Companies { get; set; } = new List<Company>();
    }
    public class Company
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public int CountryId { set; get; }
        public virtual Country? Country { set; get; }
        public virtual List<Employe> Employes { get; set; } = new List<Employe>();
    }

    public class Position
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public virtual List<Employe> Employes { get; set; } = new List<Employe>();
    }
    public class Employe
    {
        public int Id { set; get; }
        public string? Name { set; get; } = null!;
        public DateTime BirthDate { set; get; }
        public int? CompanyId { set; get; } // свойство - внешний ключ
        public virtual Company? Company { set; get; } // навигационное свойство
        public int? PositionId { set; get; } // свойство - внешний ключ
        public virtual Position? Position { set; get; } // навигационное свойство
    }

    public class AppContext : DbContext
    {
        public DbSet<Employe> Employes { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public AppContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CompaniesDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            using(AppContext context = new())
            {
                //var employes = context.Employes.ToList();
                //foreach(Employe employe in employes)
                //    Console.WriteLine($"{employe.Name} {employe.Position.Title} {employe.Company.Title} {employe.Company.Country.Title} {employe.Company.Country.Capital.Title}");
                //Console.WriteLine();

                var companies = context.Companies.ToList();
                foreach (Company company in companies)
                {
                    Console.WriteLine($"{company.Title} {company?.Country?.Title}");
                    foreach(Employe employe in company.Employes)
                        Console.WriteLine($"\t{employe.Name}");
                }
                Console.WriteLine();

                
                foreach (Country country in context.Countries.ToList())
                {
                    Console.WriteLine($"{country.Title} {country?.Capital?.Title}");
                    foreach (Company company in country.Companies)
                    {
                        Console.WriteLine($"\t{company.Title}");
                        foreach (Employe employe in company.Employes)
                            Console.WriteLine($"\t\t{employe.Name}");
                        Console.WriteLine();
                    }

                }

                




                //foreach(Country country in context.Countries)
                //{
                //    Console.WriteLine($"{country.Title} - {country?.Capital?.Title}");
                //    foreach(Company company in country?.Companies)
                //    {
                //        Console.WriteLine($"\t{company.Title}");
                //        foreach(Employe employe in company.Employes)
                //            Console.WriteLine($"\t\t{employe.Name}");
                //        Console.WriteLine();
                //    }
                //}
            }
        }
    }
}