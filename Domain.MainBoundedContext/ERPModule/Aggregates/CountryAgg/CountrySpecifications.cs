using NLayerApp.Domain.Seedwork.Specification;
using System;
using System.Linq;

namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg
{
    /// <summary>
    /// A list of country specifications. 
    /// You can learn about Specifications, Enhanced Query Objects or repository methods
    /// reading our Architecture guide and checking the DesignNotes.txt in Domain.Seedwork project
    /// </summary>
    public static class CountrySpecifications
    {
        /// <summary>
        /// Specification for country with name or iso code like to <paramref name="text"/>
        /// </summary>
        /// <param name="text">The text to search</param>
        /// <returns>Associated specification for this criterion</returns>
        public static ISpecification<Country> CountryFullText(string text)
        {
            Specification<Country> specification = new TrueSpecification<Country>();

            if (!String.IsNullOrWhiteSpace(text))
            {
                var nameSpecification = new DirectSpecification<Country>(c => c.CountryName.ToLower().Contains(text));
                var isoCodeSpecification = new DirectSpecification<Country>(c => c.CountryISOCode.ToLower().Contains(text));

                specification &= (nameSpecification || isoCodeSpecification);
            }

            return specification;
        }
    }
}
