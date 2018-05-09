namespace NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg
{
    using NLayerApp.Domain.Seedwork.Specification;
    using System;
    public static class PostSpecifications
    {
        public static ISpecification<Post> PostsContainsTitleOrContent(string q)
        {
            Specification<Post> specification = new TrueSpecification<Post>();

            if (!String.IsNullOrWhiteSpace(q))
            {
                specification &= 
                    new DirectSpecification<Post>(p => p.Title.ToLower().Contains(q.ToLower()) || 
                                                    p.Content.ToLower().Contains(q.ToLower()));
            }

            return specification;
        }

        public static ISpecification<Post> PostsByBlog(int id)
        {
            Specification<Post> specification = new TrueSpecification<Post>();

            if (id > 0)
            {
                specification &=
                    new DirectSpecification<Post>(p => p.BlogId == id);
            }

            return specification;
        }
    }
}
