using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
using System.Linq;

namespace NLayerApp.DistributedServices.MainBoundedContext
{
    public static class DbInitializer
    {
        public static void Initialize(MainBCUnitOfWork context)
        {
            ////Demo data
            context.Database.EnsureCreated();

            // Seed the database
            if (context.Countries.Any()) return;

            //Countries
            var spain = new Country("Spain", "ES");
            spain.GenerateNewIdentity();
            context.Countries.Add(spain);

            var us = new Country("U.S.", "US");
            us.GenerateNewIdentity();
            context.Countries.Add(us);

            var uk = new Country("U.K.", "GB");
            uk.GenerateNewIdentity();
            context.Countries.Add(uk);

            var canada = new Country("Canada", "CA");
            canada.GenerateNewIdentity();
            context.Countries.Add(canada);

            var italy = new Country("Italy", "IT");
            italy.GenerateNewIdentity();
            context.Countries.Add(italy);

            var france = new Country("France", "FR");
            france.GenerateNewIdentity();
            context.Countries.Add(france);

            var argentina = new Country("Argentina", "AR");
            argentina.GenerateNewIdentity();
            context.Countries.Add(argentina);

            var russia = new Country("Russian Federation", "RUS");
            russia.GenerateNewIdentity();
            context.Countries.Add(russia);

            var israel = new Country("Israel", "IS");
            israel.GenerateNewIdentity();
            context.Countries.Add(israel);

            var brazil = new Country("Brazil", "BZ");
            brazil.GenerateNewIdentity();
            context.Countries.Add(brazil);

            ////
            //Customers

            //Cesar de la Torre
            var customer1 = CustomerFactory.CreateCustomer("Cesar", "De la Torre", "+34 1234567", "Microsoft", spain, new Address("Madrid", "28700", "Calle Club Deportivo 1", "Parque Empresarial La Finca, Edif. 1"));
            customer1.SetTheCountryReference(spain.Id);
            context.Customers.Add(customer1);

            //Unai Zorrilla
            var customer2 = CustomerFactory.CreateCustomer("Unai", "Zorrilla", "+34 1234567", "Plain Concepts", spain, new Address("Madrid", "12345", "Calle Plain", "Barrio San Chinarro"));
            customer2.SetTheCountryReference(spain.Id);
            context.Customers.Add(customer2);

            //Miguel Angel
            var customer3 = CustomerFactory.CreateCustomer("Miguel Angel", "Ramos", "+1 1234567", "Microsoft", us, new Address("Redmond", "12345", "One Microsoft Way", "Building X"));
            customer3.SetTheCountryReference(us.Id);
            context.Customers.Add(customer3);

            //Eric Evans            
            var customer4 = CustomerFactory.CreateCustomer("Eric", "Evans", "+1 1234567", "Domain Language", us, new Address("City", "12345", "DDD Street", "Building X"));
            customer4.SetTheCountryReference(us.Id);
            context.Customers.Add(customer4);

            //César Castro            
            var customer5 = CustomerFactory.CreateCustomer("César", "Castro", "+34 1234567", "Freelance", spain, new Address("Madrid", "12345", "Calle de Madrid", "Barrio de Madrid"));
            customer5.SetTheCountryReference(spain.Id);
            context.Customers.Add(customer4);

            ////
            //Bank Accounts

            //Account 001
            var bankAccountNumber001 = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var newBankAccount001 = BankAccountFactory.CreateBankAccount(customer1, bankAccountNumber001);
            newBankAccount001.DepositMoney(System.Convert.ToDecimal(5000), "Initial Balance");
            context.BankAccounts.Add(newBankAccount001);

            //Account 002
            var bankAccountNumber002 = new BankAccountNumber("1111", "2222", "3333333333", "02");
            var newBankAccount002 = BankAccountFactory.CreateBankAccount(customer2, bankAccountNumber002);
            newBankAccount002.DepositMoney(System.Convert.ToDecimal(3000), "Initial Balance");
            context.BankAccounts.Add(newBankAccount002);

            //Account 003
            var bankAccountNumber003 = new BankAccountNumber("1111", "2222", "3333333333", "02");
            var newBankAccount003 = BankAccountFactory.CreateBankAccount(customer5, bankAccountNumber003);
            newBankAccount003.DepositMoney(System.Convert.ToDecimal(10000), "Initial Balance");
            context.BankAccounts.Add(newBankAccount003);

            context.SaveChanges();
        }
    }
}
