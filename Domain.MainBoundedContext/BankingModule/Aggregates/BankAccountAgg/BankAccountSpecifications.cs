namespace NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg
{
    using NLayerApp.Domain.Seedwork.Specification;
    using System;

    /// <summary>
    /// A list of bank account specifications. You can learn
    /// about Specifications, Enhanced Query Objects or repository methods
    /// in DesignNotes.txt in Domain.Seedwork project
    /// </summary>
    public static class BankAccountSpecifications
    {
        /// <summary>
        /// Specification for bank accounts with number like to <paramref name="ibanNumber"/>
        /// </summary>
        /// <param name="ibanNumber">The bank account number</param>
        /// <returns>Associated specification</returns>
        public static ISpecification<BankAccount> BankAccountIbanNumber(string ibanNumber)
        {
            Specification<BankAccount> specification = new TrueSpecification<BankAccount>();

            if (!String.IsNullOrWhiteSpace(ibanNumber))
            {
                specification &= new DirectSpecification<BankAccount>((b) => b.Iban
                                                                              .ToLower()
                                                                              .Contains(ibanNumber.ToLower()));
            }

            return specification;
        }
    }
}
