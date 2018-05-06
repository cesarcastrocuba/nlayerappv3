namespace NLayerApp.Application.BlogBoundedContext.BlogModule.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg;
    using NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg;
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Infrastructure.Crosscutting.Validator;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Application.BlogBoundedContext.DTO;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using NLayerApp.Application.Seedwork.Common;

    public class BlogsService : Service<Blog,BlogDTO>, IBlogsService
    {
        protected IBlogRepository _blogRepository;
        protected IPostRepository _postRepository;

        protected ILocalization _resources;
        protected IEntityValidator _validator;

        public BlogsService(IBlogRepository blogRepository, IPostRepository postRepository)
            :base(blogRepository)
        {
            _blogRepository = blogRepository;
            _postRepository = postRepository;

            _resources = LocalizationFactory.CreateLocalResources();
            _validator = EntityValidatorFactory.CreateValidator();
        }
        public override void Dispose()
        {
            _blogRepository.Dispose();
            _postRepository.Dispose();

            base.Dispose();
        }
        public override async Task<IList<BlogDTO>> GetAllDTOAsync()
        {
            var blogs = await _blogRepository.GetAllAsync();

            if (blogs != null && blogs.Any())
            {
                return blogs.ProjectedAsCollection<BlogDTO>();
            }

            return null;
        }

        public override async Task<BlogDTO> GetDTOAsync(object id)
        {
            var blog = await _blogRepository.GetAsync(id);            
            if (blog != null) {
                return blog.ProjectedAs<BlogDTO>();
            }

            return null;
        }        

        public override async Task<BlogDTO> AddAsync(BlogDTO blogDTO)
        {
            if (blogDTO == null)
            {
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.validation_No_Records_Found_Error));
            }

            Blog blog = BlogFactory.CreateBlog(blogDTO.Name, blogDTO.Url, blogDTO.Rating);
            
            if (_validator.IsValid<Blog>(blog))
            {
                _blogRepository.Add(blog);
                
                await _blogRepository.UnitOfWork.CommitAsync();
                
                return blog.ProjectedAs<BlogDTO>();
            }
            else
            {
                throw new ApplicationValidationErrorsException(_validator.GetInvalidMessages<Blog>(blog));
            }
        }


        public override async Task<BlogDTO> ModifyAsync(object id, BlogDTO blogDTO)
        {
            if (blogDTO == null)
            {
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.validation_No_Records_Found_Error));
            }

            if ((int)id != blogDTO.BlogId)
            {
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.validation_BadRequest));
            }

            Blog blog = await _blogRepository.GetAsync(id);

            if (blog == null) return null;
            blog.Name = blogDTO.Name;
            blog.Url = blogDTO.Url;
            blog.Rating = blogDTO.Rating;

            if (_validator.IsValid<Blog>(blog))
            {
                _blogRepository.Modify(blog);
                await _blogRepository.UnitOfWork.CommitAsync();

                return blog.ProjectedAs<BlogDTO>();
            }
            else
            {
                throw new ApplicationValidationErrorsException(_validator.GetInvalidMessages<Blog>(blog));
            }
        }

        public override async Task RemoveAsync(object id)
        {
            var blog = await _blogRepository.GetAsync(id);

            if (blog == null) return;

            _blogRepository.Remove(blog);

            await _blogRepository.UnitOfWork.CommitAsync();
        }

        public async Task<IList<PostDTO>> GetPostsDTOAsync(int id)
        {
            ISpecification<Post> specification = PostSpecifications.PostsByBlog(id);
            var posts = await _postRepository.AllMatchingAsync(specification);

            if (posts != null && posts.Any())
            {
                return posts.ProjectedAsCollection<PostDTO>();
            }
            return null;
        }
        
    }
}
